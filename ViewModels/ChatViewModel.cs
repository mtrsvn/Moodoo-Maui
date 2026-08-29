using MoodooApp.Models;
using MoodooApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoodooApp.ViewModels;

public partial class ChatViewModel : BaseViewModel
{
    private readonly ChatService _chatService;

    public ObservableCollection<ChatMessage> Messages => _chatService.Messages;

    [ObservableProperty]
    private string _newMessage = string.Empty;

    public ChatViewModel(ChatService chatService)
    {
        Title = "Moodie";
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessage))
            return;

        var text = NewMessage;
        NewMessage = string.Empty;
        
        IsBusy = true;
        try
        {
            await _chatService.SendMessageAsync(text);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
