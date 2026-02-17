using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Threading.Channels;

namespace NBomber.SignalR;

/// <summary>
/// Specifies the protocol used for SignalR hub communication.
/// </summary>
public enum HubProtocolFormat
{
    /// <summary>
    /// Uses JSON serialization for hub messages.
    /// </summary>
    Json,

    /// <summary>
    /// Uses MessagePack binary serialization for hub messages.
    /// </summary>
    MessagePack
}

/// <summary>
/// Provides a wrapper around an <see cref="HubConnection"/> for managing SignalR communication,
/// including connecting, invoking methods on the server, and calculating message sizes for performance testing.
/// </summary>
public class SignalRConnection : IDisposable
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
    /// Asynchronously receives a PUSH message from SignalR server.
    /// </summary>
    /// <param name="token">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Response{T}"/> containing the received <see cref="object"/>.
    /// </returns>
    /// <exception cref="IgnoreMeasurementException">
    /// Thrown when the operation is cancelled by the token.
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

    /// <summary>
    /// Invokes a hub method on the server with no parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName);

        try
        {
            await Connection.InvokeAsync(methodName, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 1 parameter.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 2 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 3 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 4 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 5 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 6 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 7 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 8 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 9 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 10 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        try
        {
            await Connection.InvokeAsync(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken);

            var responseSize = CalculateResponseSize();

            return Response.Ok(sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with no parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 1 parameter and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 2 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 3 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 4 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 5 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 6 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 7 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 8 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 9 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken);

            var responseSize = CalculateResponseSize(result);

            return Response.Ok(payload: result, sizeBytes: requestSize + responseSize);
        }
        catch (Exception ex)
        {
            var responseSize = CalculateResponseSize(ex.Message, isError: true);

            return Response.Fail<TResult>(sizeBytes: requestSize + responseSize, message: ex.Message);
        }
    }

    /// <summary>
    /// Invokes a hub method on the server with 10 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public async Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
    {
        var requestSize = CalculateRequestSize(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        try
        {
            var result = await Connection.InvokeAsync<TResult>(methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken);

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
    /// Releases resources used by the SignalR connection, including disposing of the connection instance.
    /// </summary>
    public async void Dispose()
    {
        await Connection.DisposeAsync();
    }
}