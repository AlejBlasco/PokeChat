using System.Net;
using System.Text;

namespace PokeChat.ApplicationTests.Helpers;

/// <summary>
/// Fake <see cref="HttpMessageHandler"/> for unit testing HTTP clients.
/// Supports a fixed response or a queue of sequential responses.
/// </summary>
public sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage>? _queue;
    private readonly HttpStatusCode _statusCode;
    private readonly string _body;

    /// <summary>Gets the number of times <see cref="SendAsync"/> was called.</summary>
    public int CallCount { get; private set; }

    /// <summary>Initializes a handler that always returns the same status and body.</summary>
    public FakeHttpMessageHandler(HttpStatusCode statusCode, string body)
    {
        _statusCode = statusCode;
        _body = body;
    }

    /// <summary>Initializes a handler that returns responses from a queue in order.</summary>
    public FakeHttpMessageHandler(Queue<HttpResponseMessage> responses)
    {
        _queue = responses;
        _statusCode = HttpStatusCode.OK;
        _body = string.Empty;
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        CallCount++;

        if (_queue is not null && _queue.Count > 0)
            return Task.FromResult(_queue.Dequeue());

        var response = new HttpResponseMessage(_statusCode);
        if (_statusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(_body))
            response.Content = new StringContent(_body, Encoding.UTF8, "application/json");

        return Task.FromResult(response);
    }
}
