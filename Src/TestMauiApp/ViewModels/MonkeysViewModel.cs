
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TestMauiApp.Models;
using TestMauiApp.Services;
using TestMauiApp.Views;

namespace TestMauiApp.ViewModels
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        private readonly IMonkeyService _monkeyService;
        public ObservableCollection<Monkey> Monkeys { get; } = new();

        IConnectivity connectivity;
        IGeolocation geolocation;
        public MonkeysViewModel(IMonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            _monkeyService = monkeyService;
            Title = "Monkey Viewer";
            this.connectivity = connectivity;
            this.geolocation = geolocation;
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetClosestMonkeyAsync()
        {
            
            if (IsBusy || Monkeys.Count == 0)
                return;

            try
            {
                var location = await geolocation.GetLastKnownLocationAsync();

                if(location == null)
                {
                    location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30),
                        });
                }

                if (location == null)
                {
                    await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey because of the geolocation is null", "Ok");
                    return;
                }

               
                var first = Monkeys.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Kilometers)).FirstOrDefault();

                if (first is null)
                    return;
                await Shell.Current.DisplayAlert("Closest Monkey", $"{first.Name} in {first.Location}", "Ok");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey: {ex.Message}", "Ok");

            }
            finally
            {
                IsBusy = false;
                
            }
        }


        [RelayCommand]
        async Task GoToMonkeyDetailsAsync(Monkey monkey)
        {
            if (monkey is null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, 
                new Dictionary<string, object>
                {
                    {"Monkey", monkey},
                });
        }

        [RelayCommand]
        async Task GetMonkeysAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if(connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    
                    await Shell.Current.DisplayAlert("Internet Issue!", $"Check your internet and try again", "Ok");
                    return;
                }


                IsBusy = true;
                var monkeys = await _monkeyService.GetMonkeyListAsync();

                if (Monkeys.Count != 0)
                    Monkeys.Clear();

                foreach (var monkey in monkeys)
                {
                    Monkeys.Add(monkey);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get monkyes: {ex.Message}", "Ok");
               
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
