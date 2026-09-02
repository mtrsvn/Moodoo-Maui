using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class MoodDetailPage : ContentPage
{
    public MoodDetailPage(MoodDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
