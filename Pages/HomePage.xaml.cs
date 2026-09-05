using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private bool _isFabExpanded = false;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDataCommand.Execute(null);

        // Reset FAB state on return
        if (_isFabExpanded)
        {
            CloseSpeedDial(false);
        }
    }

    private async void OnFabTapped(object? sender, EventArgs e)
    {
        if (_isFabExpanded)
        {
            await CloseSpeedDial(true);
        }
        else
        {
            await OpenSpeedDial();
        }
    }

    private async void OnBackdropTapped(object? sender, EventArgs e)
    {
        if (_isFabExpanded)
        {
            await CloseSpeedDial(true);
        }
    }

    private async void OnLogMoodOptionTapped(object? sender, EventArgs e)
    {
        await CloseSpeedDial(false);
        _viewModel.OpenMoodLogCommand.Execute(null);
    }

    private async void OnChatbotOptionTapped(object? sender, EventArgs e)
    {
        await CloseSpeedDial(false);
        _viewModel.OpenChatbotCommand.Execute(null);
    }

    private async Task OpenSpeedDial()
    {
        _isFabExpanded = true;
        FabBackdrop.InputTransparent = false;
        SpeedDialMenu.InputTransparent = false;

        var rotateTask = FabIcon.RotateToAsync(45, 200, Easing.CubicOut);
        var backdropFade = FabBackdrop.FadeToAsync(1, 200);

       
        var menuFade = SpeedDialMenu.FadeToAsync(1, 220, Easing.CubicOut);
        var menuScale = SpeedDialMenu.ScaleToAsync(1, 220, Easing.SpringOut);
        var menuTrans = SpeedDialMenu.TranslateToAsync(0, 0, 220, Easing.CubicOut);

        await Task.WhenAll(rotateTask, backdropFade, menuFade, menuScale, menuTrans);
    }

    private async Task CloseSpeedDial(bool animated)
    {
        _isFabExpanded = false;
        FabBackdrop.InputTransparent = true;
        SpeedDialMenu.InputTransparent = true;

        if (!animated)
        {
            FabIcon.Rotation = 0;
            FabBackdrop.Opacity = 0;
            SpeedDialMenu.Opacity = 0;
            SpeedDialMenu.Scale = 0.6;
            SpeedDialMenu.TranslationY = 25;
            return;
        }

        var rotateTask = FabIcon.RotateToAsync(0, 180, Easing.CubicIn);
        var backdropFade = FabBackdrop.FadeToAsync(0, 180);

        var menuFade = SpeedDialMenu.FadeToAsync(0, 180, Easing.CubicIn);
        var menuScale = SpeedDialMenu.ScaleToAsync(0.6, 180, Easing.CubicIn);
        var menuTrans = SpeedDialMenu.TranslateToAsync(0, 25, 180, Easing.CubicIn);

        await Task.WhenAll(rotateTask, backdropFade, menuFade, menuScale, menuTrans);
    }
}
