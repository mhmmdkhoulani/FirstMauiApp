using TestMauiApp.ViewModels;

namespace TestMauiApp.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MonkeysViewModel monkeyViewModel)
	{
        InitializeComponent();
		BindingContext = monkeyViewModel;
    }

}

