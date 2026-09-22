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
