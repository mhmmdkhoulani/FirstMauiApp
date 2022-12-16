using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestMauiApp.Models;

namespace TestMauiApp.Services
{
    public interface IMonkeyService
    {
        Task<List<Monkey>> GetMonkeyListAsync();
    }

    public class MonkeyService : IMonkeyService
    {
        HttpClient httpClient;

        public MonkeyService()
        {
            httpClient = new HttpClient();
        }

        List<Monkey> monkeyList = new List<Monkey>();
        public async Task<List<Monkey>> GetMonkeyListAsync()
        {
            if (monkeyList?.Count > 0)
                return monkeyList;

            var url = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            }
            return monkeyList;
        }
    }
}
