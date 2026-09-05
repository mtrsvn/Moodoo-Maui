using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MoodooApp.ViewModels;

public partial class PremiumViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _isMonthlySelected = true;

    [ObservableProperty]
    private bool _isYearlySelected = false;

    public PremiumViewModel()
    {
        Title = "Premium";
    }

    [RelayCommand]
    private void SelectMonthly()
    {
        IsMonthlySelected = true;
        IsYearlySelected = false;
    }

    [RelayCommand]
    private void SelectYearly()
    {
        IsMonthlySelected = false;
        IsYearlySelected = true;
    }

    [RelayCommand]
    private async Task SubscribeAsync()
    {
        var plan = IsMonthlySelected ? "Monthly" : "Yearly";
        await Application.Current.MainPage.DisplayAlert("Subscription", $"You selected the {plan} plan. (Demo)", "OK");
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
