using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace PokeChat.Web.Components.Chat;

/// <summary>
/// Dummy chat bot component. Provides a fully functional UI
/// ready for Semantic Kernel integration in a future feature.
/// </summary>
public partial class ChatBot : IAsyncDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private readonly List<ChatMessage> _messages = [];
    private readonly List<string> _quickReplies =
    [
        "Who is Pikachu?",
        "What are the starter Pokémon?",
        "Tell me about Legendary Pokémon",
        "What is the strongest Pokémon?"
    ];

    private readonly List<string> _botResponses =
    [
        "Pika pika! That's a great question, Trainer! 🎉",
        "As a Pokémon expert, I can tell you that's a fascinating topic!",
        "Interesting! Did you know that there are over 1,000 Pokémon species?",
        "Great question! Let me think... ⚡ Even Pikachu would be impressed!",
        "The Pokédex has a lot to say about that! Stay curious, Trainer!",
        "Legendary Pokémon are rare and incredibly powerful. Tread carefully!",
        "Every Pokémon has a unique ability. That's what makes them special! ✨"
    ];

    private string _userInput = string.Empty;
    private bool _isTyping;
    private bool _showQuickReplies = true;
    private ElementReference _messagesContainer;
    private readonly Random _random = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        _messages.Add(new ChatMessage
        {
            Role = ChatRole.Bot,
            Content = "Hello, Trainer! ⚡ I'm PokéBot, your Pokémon assistant. Ask me anything about the Pokémon world!"
        });
    }

    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(_userInput) || _isTyping)
            return;

        var userMessage = _userInput.Trim();
        _userInput = string.Empty;
        _showQuickReplies = false;

        _messages.Add(new ChatMessage
        {
            Role = ChatRole.User,
            Content = userMessage
        });

        _isTyping = true;
        StateHasChanged();

        await ScrollToBottomAsync();
        await Task.Delay(_random.Next(800, 1800));

        _messages.Add(new ChatMessage
        {
            Role = ChatRole.Bot,
            Content = _botResponses[_random.Next(_botResponses.Count)]
        });

        _isTyping = false;
        StateHasChanged();

        await ScrollToBottomAsync();
    }

    private async Task SendQuickReply(string message)
    {
        _userInput = message;
        await SendMessageAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !e.ShiftKey)
            await SendMessageAsync();
    }

    private void ClearConversation()
    {
        _messages.Clear();
        _showQuickReplies = true;
        _messages.Add(new ChatMessage
        {
            Role = ChatRole.Bot,
            Content = "Conversation cleared! ⚡ Ready for a fresh start, Trainer!"
        });
    }

    private async Task ScrollToBottomAsync()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom", _messagesContainer);
        }
        catch (JSException)
        {
            // Ignore JS interop errors during prerendering
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
