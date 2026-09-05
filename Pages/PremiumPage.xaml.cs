using Microsoft.Maui.Controls;
using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class PremiumPage : ContentPage
{
    public PremiumPage(PremiumViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
