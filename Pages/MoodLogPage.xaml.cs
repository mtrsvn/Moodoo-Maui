using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class MoodLogPage : ContentPage
{
    public MoodLogPage(MoodViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
