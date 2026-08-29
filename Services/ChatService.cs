using MoodooApp.Models;
using System.Collections.ObjectModel;

namespace MoodooApp.Services;

public class ChatService
{
    public ObservableCollection<ChatMessage> Messages { get; private set; } = new();

    public ChatService()
    {
        Messages.Add(new ChatMessage { Sender = "bot", Text = "Hey! 👋 I'm Moodie, your personal AI companion. I'm here to listen, support, and help you understand your emotions." });
        Messages.Add(new ChatMessage { Sender = "user", Text = "Hi Moodie! I've been feeling a bit stressed lately." });
        Messages.Add(new ChatMessage { Sender = "bot", Text = "I'm really glad you reached out. Stress can feel overwhelming, but you're not alone. Can you tell me more about what's been stressing you out?" });
        Messages.Add(new ChatMessage { Sender = "user", Text = "Work has been really hectic. Too many deadlines." });
        Messages.Add(new ChatMessage { Sender = "bot", Text = "That sounds really tough. When work piles up like that, it can drain your energy and affect your mood. One thing that often helps is to break down your tasks into smaller steps. Have you tried that?" });
    }

    public async Task SendMessageAsync(string text)
    {
        Messages.Add(new ChatMessage { Sender = "user", Text = text });
        await Task.Delay(1200);

        var responses = new[]
        {
            "I hear you. Thank you for sharing that with me. How did that make you feel?",
            "That's really insightful! It's great that you're paying attention to your emotions.",
            "I understand. Remember, it's okay to take breaks and be kind to yourself.",
            "That's a really positive way to look at it! Have you been able to do anything today that brought you joy?",
            "I'm always here for you. Have you tried logging that in your mood journal?",
            "It sounds like you're making real progress. Keep going, you're doing great! 💜",
        };

        var rand = new Random();
        Messages.Add(new ChatMessage { Sender = "bot", Text = responses[rand.Next(responses.Length)] });
    }
}
