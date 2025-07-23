using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyBooking.Web.Pages.AdminBussiness
{
    public class ManagemovieModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<MovieBuyerInfo> MovieBuyers { get; set; } = new();
        public BuyerInfo TopSpender { get; set; }
        public MovieBuyerInfo MostPurchasedMovie { get; set; }
        public string Filter { get; set; }

        public ManagemovieModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync(string filter = null)
        {
            Filter = filter;
            var client = _httpClientFactory.CreateClient();
            var url = "https://localhost:7087/api/admin/dashboard/moviebuyers";
            if (!string.IsNullOrEmpty(filter))
                url += $"?filter={filter}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("movieBuyers", out var movieBuyersElem) && movieBuyersElem.ValueKind == JsonValueKind.Array)
                {
                    MovieBuyers = new List<MovieBuyerInfo>();
                    foreach (var movieElem in movieBuyersElem.EnumerateArray())
                    {
                        var buyers = new List<BuyerInfo>();
                        if (movieElem.TryGetProperty("buyers", out var buyersElem) && buyersElem.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var buyerElem in buyersElem.EnumerateArray())
                            {
                                buyers.Add(new BuyerInfo
                                {
                                    UserId = buyerElem.GetProperty("userId").GetInt32(),
                                    FullName = buyerElem.GetProperty("fullName").GetString(),
                                    TotalPaid = buyerElem.GetProperty("totalPaid").GetDecimal()
                                });
                            }
                        }
                        MovieBuyers.Add(new MovieBuyerInfo
                        {
                            MovieId = movieElem.GetProperty("movieId").GetInt32(),
                            MovieTitle = movieElem.GetProperty("movieTitle").GetString(),
                            Buyers = buyers
                        });
                    }
                }

                if (root.TryGetProperty("topSpender", out var tsElem) && tsElem.ValueKind != JsonValueKind.Null)
                {
                    TopSpender = new BuyerInfo
                    {
                        UserId = tsElem.GetProperty("userId").GetInt32(),
                        FullName = tsElem.GetProperty("fullName").GetString(),
                        TotalPaid = tsElem.GetProperty("totalPaid").GetDecimal()
                    };
                }
                if (root.TryGetProperty("mostPurchasedMovie", out var mpElem) && mpElem.ValueKind != JsonValueKind.Null)
                {
                    var buyers = new List<BuyerInfo>();
                    if (mpElem.TryGetProperty("buyers", out var buyersElem) && buyersElem.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var buyerElem in buyersElem.EnumerateArray())
                        {
                            buyers.Add(new BuyerInfo
                            {
                                UserId = buyerElem.GetProperty("userId").GetInt32(),
                                FullName = buyerElem.GetProperty("fullName").GetString(),
                                TotalPaid = buyerElem.GetProperty("totalPaid").GetDecimal()
                            });
                        }
                    }
                    MostPurchasedMovie = new MovieBuyerInfo
                    {
                        MovieId = mpElem.GetProperty("movieId").GetInt32(),
                        MovieTitle = mpElem.GetProperty("movieTitle").GetString(),
                        Buyers = buyers
                    };
                }
            }
        }

        public class MovieBuyerInfo
        {
            public int MovieId { get; set; }
            public string MovieTitle { get; set; }
            public List<BuyerInfo> Buyers { get; set; } = new();
        }
        public class BuyerInfo
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public decimal TotalPaid { get; set; }
        }
    }
}
