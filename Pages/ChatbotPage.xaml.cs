using System.Collections.Specialized;
using MoodooApp.ViewModels;

namespace MoodooApp.Pages;

public partial class ChatbotPage : ContentPage
{
    private bool _isAnimatingDots;

    public ChatbotPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        if (viewModel.Messages != null)
        {
            viewModel.Messages.CollectionChanged += OnMessagesCollectionChanged;
        }
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChatViewModel.IsBusy))
        {
            var vm = (ChatViewModel)BindingContext;
            if (vm.IsBusy)
            {
                StartDotAnimation();
            }
            else
            {
                StopDotAnimation();
            }
        }
    }

    private async void StartDotAnimation()
    {
        if (_isAnimatingDots) return;
        _isAnimatingDots = true;

        while (_isAnimatingDots)
        {
            if (Dot1 == null || Dot2 == null || Dot3 == null) break;

            _ = AnimateDot(Dot1);
            await Task.Delay(150);
            if (!_isAnimatingDots) break;

            _ = AnimateDot(Dot2);
            await Task.Delay(150);
            if (!_isAnimatingDots) break;

            _ = AnimateDot(Dot3);
            
            await Task.Delay(600);
        }
    }

    private async Task AnimateDot(BoxView dot)
    {
        await dot.TranslateTo(0, -5, 250, Easing.SinOut);
        await dot.TranslateTo(0, 0, 250, Easing.SinIn);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Unfocus();
        ScrollToBottom();
    }

    private void OnMessagesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(100);
            ScrollToBottom();
        });
    }

    private void ScrollToBottom()
    {
        ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, true);
    }

    private void StopDotAnimation()
    {
        _isAnimatingDots = false;
        Dot1?.TranslateTo(0, 0, 100);
        Dot2?.TranslateTo(0, 0, 100);
        Dot3?.TranslateTo(0, 0, 100);
    }
}