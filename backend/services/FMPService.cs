using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.interfaces;
using backend.models;
using DotNetEnv;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace backend.services
{
    public class FMPService : IFMPService
    {
        private HttpClient _httpClient;
        private FMPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stock?> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                Env.Load();

                var config = Environment.GetEnvironmentVariable("FMPKey") ?? "";

                if (string.IsNullOrWhiteSpace(config))
                    return null;

                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={config}");


                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}