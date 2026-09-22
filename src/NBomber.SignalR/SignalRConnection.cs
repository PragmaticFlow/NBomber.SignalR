using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Threading.Channels;

namespace NBomber.SignalR;

/// <summary>
/// Provides a wrapper around an <see cref="HubConnection"/> for managing SignalR communication,
/// including connecting, invoking methods on the server, and calculating message sizes for performance testing.
/// </summary>
public partial class SignalRConnection : IAsyncDisposable
{
    private readonly Channel<Response<object>> _channel = Channel.CreateUnbounded<Response<object>>();
    private readonly IHubProtocol _hubProtocol;

    /// <summary>
    /// Gets the underlying <see cref="HubConnection"/> used for communication with the SignalR Hub.
    /// </summary>
    public HubConnection Connection { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalRConnection"/> class
    /// using <see cref="HubProtocolFormat.Json"/> as the default hub protocol.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/> to wrap.</param>
    /// <param name="hubProtocolType">The <see cref="HubProtocolFormat"/> to use for serializing hub messages.</param>
    public SignalRConnection(HubConnection connection, HubProtocolFormat hubProtocolType = HubProtocolFormat.Json)
    {
        Connection = connection;

        _hubProtocol = hubProtocolType switch
        {
            HubProtocolFormat.Json => new JsonHubProtocol(),
            HubProtocolFormat.MessagePack => new MessagePackHubProtocol(),
            _ => throw new ArgumentException("Unsupported protocol type")
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalRConnection"/> class using a custom <see cref="IHubProtocol"/>.
    /// Use this overload when the connection is configured with a protocol that <see cref="HubProtocolFormat"/> does not cover.
    /// </summary>
    /// <param name="connection">The <see cref="HubConnection"/> to wrap.</param>
    /// <param name="hubProtocol">
    /// The protocol used to serialize hub messages when calculating message sizes.
    /// It should match the protocol the <paramref name="connection"/> was built with, otherwise the reported sizes will be inaccurate.
    /// </param>
    public SignalRConnection(HubConnection connection, IHubProtocol hubProtocol)
    {
        Connection = connection;
        _hubProtocol = hubProtocol;
    }

    /// <summary>
    /// Asynchronously receives a PUSH message from SignalR server.
    /// </summary>
    /// <param name="token">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Response{T}"/> containing the received <see cref="object"/>.
    /// </returns>
    /// <exception cref="IgnoreMeasurementException">
    /// Thrown when the operation is cancelled by the token, or when the connection is disposed while waiting.
    /// </exception>
    public async ValueTask<Response<object>> Receive(CancellationToken token)
    {
        try
        {
            return await _channel.Reader.ReadAsync(token);
        }
        catch (OperationCanceledException)
        {
            throw new IgnoreMeasurementException();
        }
        catch (ChannelClosedException)
        {
            throw new IgnoreMeasurementException();
        }
    }

    /// <summary>
    /// Writes a response to the internal channel.
    /// This is typically used to queue server-to-client messages for processing.
    /// </summary>
    /// <param name="response">The response to write to the channel.</param>
    public void Write(Response<object> response)
    {
        _channel.Writer.TryWrite(response);
    }

    private async Task<Response<object>> InvokeCore(string methodName, object?[] args, CancellationToken cancellationToken)
    {
        var requestSize = CalculateRequestSize(methodName, args);

        try
        {
            await Connection.InvokeCoreAsync(methodName, args, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    private async Task<Response<TResult>> InvokeCore<TResult>(string methodName, object?[] args, CancellationToken cancellationToken)
    {
        var requestSize = CalculateRequestSize(methodName, args);

        try
        {
            var result = await Connection.InvokeCoreAsync<TResult>(methodName, args, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    private int CalculateRequestSize(string methodName, params object?[] args)
    {
        var message = new InvocationMessage("???", methodName, args);
        var bytes = _hubProtocol.GetMessageBytes(message);
        return bytes.Length;
    }

    private int CalculateResponseSize(object? payload = null, bool isError = false)
    {
        HubMessage message = isError
            ? CompletionMessage.WithError("???", payload?.ToString() ?? "")
            : CompletionMessage.WithResult("???", payload);

        var bytes = _hubProtocol.GetMessageBytes(message);
        return bytes.Length;
    }

    /// <summary>
    /// Calculates the size in bytes of a SignalR message for the specified method and arguments.
    /// This is useful for tracking message sizes in performance testing scenarios.
    /// </summary>
    /// <param name="methodName">The name of the hub method.</param>
    /// <param name="args">The arguments passed to the hub method.</param>
    /// <returns>The size of the serialized message in bytes.</returns>
    public int CalculateMessageSize(string methodName, params object?[] args)
    {
        var message = new InvocationMessage(methodName, args);
        var bytes = _hubProtocol.GetMessageBytes(message);
        return bytes.Length;
    }

    /// <summary>
    /// Asynchronously starts the SignalR connection to the hub.
    /// </summary>
    /// <returns>
    /// A <see cref="Response{T}"/> indicating successful connection.
    /// </returns>
    public async Task<Response<object>> Start()
    {
        await Connection.StartAsync();
        return Response.Ok();
    }

    /// <summary>
    /// Asynchronously stops the SignalR connection to the hub.
    /// </summary>
    /// <returns>
    /// A <see cref="Response{T}"/> indicating successful disconnection.
    /// </returns>
    public async Task<Response<object>> Stop()
    {
        await Connection.StopAsync();
        return Response.Ok();
    }

    /// <summary>
    /// Asynchronously releases resources used by the SignalR connection, including disposing of the connection instance.
    /// Any pending <see cref="Receive"/> call is terminated with <see cref="IgnoreMeasurementException"/>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        _channel.Writer.TryComplete();
        await Connection.DisposeAsync();
    }
}
