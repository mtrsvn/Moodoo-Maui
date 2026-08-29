using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class InsightsPage : ContentPage
{
    public InsightsPage(InsightsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
