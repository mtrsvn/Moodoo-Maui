namespace MoodooApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Pages.MoodLogPage), typeof(Pages.MoodLogPage));
		Routing.RegisterRoute(nameof(Pages.HistoryPage), typeof(Pages.HistoryPage));
		Routing.RegisterRoute(nameof(Pages.MoodDetailPage), typeof(Pages.MoodDetailPage));
		Routing.RegisterRoute(nameof(Pages.ChatbotPage), typeof(Pages.ChatbotPage));
	}
}
