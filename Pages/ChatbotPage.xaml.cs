using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class ChatbotPage : ContentPage
{
    public ChatbotPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
