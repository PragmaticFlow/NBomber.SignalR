using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Text.Json;
using System.Threading.Channels;

namespace NBomber.SignalR;

/// <summary>
/// Provides a wrapper around an <see cref="HubConnection"/> for managing SignalR communication,
/// including connecting, invoking methods on the server, and calculating message sizes for performance testing.
/// </summary>
public class SignalRConnection
{
    private readonly Channel<Response<object>> _channel = Channel.CreateUnbounded<Response<object>>();

    /// <summary>
    /// Gets the underlying <see cref="HubConnection"/> used for communication with the SignalR Hub.
    /// </summary>
    public HubConnection Connection { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalRConnection"/> class.
    /// </summary>
    /// <param name="connection">The SignalR hub connection to wrap.</param>
    public SignalRConnection(HubConnection connection)
    {
        Connection = connection;
    }

    /// <summary>
    /// Reads a response from the internal channel asynchronously.
    /// This is typically used to receive server-to-client messages.
    /// </summary>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the response.</returns>
    public async Task<Response<object>> Receive()
    {
        return await _channel.Reader.ReadAsync();
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
    public async Task<Response<object>> InvokeAsync(string methodName, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
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
    public async Task<Response<object>> InvokeAsync(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
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
    public async Task<Response<TResult>> InvokeAsync<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
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
        var requestBaseLength = 57; // {"type":1,"invocationId":"123","target":"","arguments":}

        int size = requestBaseLength + methodName.Length;

        string argsJson = JsonSerializer.Serialize(args);
        size += System.Text.Encoding.UTF8.GetByteCount(argsJson);

        return size;
    }

    private int CalculateResponseSize(object? payload = null, bool isError = false)
    {
        var responseBaseLength = 33; // {"type":3,"invocationId":"123",}
        var resultLength = 9;  // "result":
        var errorLength = 8;   // "error":

        int size = responseBaseLength + (isError ? errorLength : resultLength);

        string payloadJson = JsonSerializer.Serialize(payload);
        size += System.Text.Encoding.UTF8.GetByteCount(payloadJson);

        return size;
    }

    public static int CalculateServerInvocationSize(string methodName, params object?[] args)
    {
        var baseLength = 34; // {"type":1,"target":"","arguments":}

        int size = baseLength + methodName.Length;

        string argsJson = JsonSerializer.Serialize(args);
        size += System.Text.Encoding.UTF8.GetByteCount(argsJson);

        return size;
    }
}