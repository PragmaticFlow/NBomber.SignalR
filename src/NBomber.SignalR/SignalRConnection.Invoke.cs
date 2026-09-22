using NBomber.Contracts;

namespace NBomber.SignalR;

public partial class SignalRConnection
{
    /// <summary>
    /// Invokes a hub method on the server with no parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 1 parameter.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 2 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 3 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 4 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 5 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 6 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5, arg6], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 7 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 8 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 9 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 10 parameters.
    /// </summary>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<object>> Invoke(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
        => InvokeCore(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with no parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 1 parameter and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 2 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 3 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 4 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 5 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 6 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5, arg6], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 7 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 8 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 9 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9], cancellationToken);

    /// <summary>
    /// Invokes a hub method on the server with 10 parameters and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the server method.</typeparam>
    /// <param name="methodName">The name of the server method to invoke.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public Task<Response<TResult>> Invoke<TResult>(string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default)
        => InvokeCore<TResult>(methodName, [arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10], cancellationToken);
}
