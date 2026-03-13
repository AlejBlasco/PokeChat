namespace PokeChat.Web.Components.Chat;

/// <summary>
/// Represents the role of a chat message sender.
/// </summary>
public enum ChatRole
{
    /// <summary>Message sent by the user.</summary>
    User,

    /// <summary>Message sent by the PokéBot assistant.</summary>
    Bot
}

/// <summary>
/// Represents a single message in the chat conversation.
/// </summary>
public sealed class ChatMessage
{
    /// <summary>Gets the unique identifier of the message.</summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>Gets the role of the message sender.</summary>
    public ChatRole Role { get; init; }

    /// <summary>Gets the message content.</summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>Gets the timestamp when the message was created.</summary>
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
