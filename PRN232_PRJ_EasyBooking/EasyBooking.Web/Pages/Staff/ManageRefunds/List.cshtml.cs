using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System;
using System.Linq;

namespace EasyBooking.Web.Pages.Staff.ManageRefunds
{
    public class ListModel : PageModel
    {
        public List<RefundHistoryDto> Refunds { get; set; } = new();
        public List<MovieDto> Movies { get; set; } = new();
        public List<CinemaDto> Cinemas { get; set; } = new();
        
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? SelectedMovieId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? SelectedCinemaId { get; set; }
        
        public int TotalRefunds { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public int TodayRefunds { get; set; }
        public decimal AverageRefundAmount { get; set; }

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                
                // Lấy danh sách hoàn vé
                var refundUrl = "api/staff/refunds";
                if (FromDate.HasValue || ToDate.HasValue || SelectedMovieId.HasValue || SelectedCinemaId.HasValue)
                {
                    var queryParams = new List<string>();
                    if (FromDate.HasValue) queryParams.Add($"fromDate={FromDate.Value:yyyy-MM-dd}");
                    if (ToDate.HasValue) queryParams.Add($"toDate={ToDate.Value:yyyy-MM-dd}");
                    if (SelectedMovieId.HasValue) queryParams.Add($"movieId={SelectedMovieId.Value}");
                    if (SelectedCinemaId.HasValue) queryParams.Add($"cinemaId={SelectedCinemaId.Value}");
                    
                    if (queryParams.Count > 0)
                    {
                        refundUrl += "?" + string.Join("&", queryParams);
                    }
                }
                
                var refundRes = await client.GetAsync(refundUrl);
                if (refundRes.IsSuccessStatusCode)
                {
                    var json = await refundRes.Content.ReadAsStringAsync();
                    Refunds = JsonSerializer.Deserialize<List<RefundHistoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                
                // Lấy danh sách phim
                var movieRes = await client.GetAsync("api/staff/movies");
                if (movieRes.IsSuccessStatusCode)
                {
                    var movieJson = await movieRes.Content.ReadAsStringAsync();
                    Movies = JsonSerializer.Deserialize<List<MovieDto>>(movieJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                
                // Lấy danh sách rạp
                var cinemaRes = await client.GetAsync("api/staff/cinemas");
                if (cinemaRes.IsSuccessStatusCode)
                {
                    var cinemaJson = await cinemaRes.Content.ReadAsStringAsync();
                    Cinemas = JsonSerializer.Deserialize<List<CinemaDto>>(cinemaJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                
                // Tính toán thống kê
                CalculateStatistics();
            }
        }
        
        private void CalculateStatistics()
        {
            if (Refunds != null && Refunds.Any())
            {
                TotalRefunds = Refunds.Count;
                TotalRefundAmount = Refunds.Sum(r => r.RefundAmount);
                TodayRefunds = Refunds.Count(r => r.RefundTime.Date == DateTime.Today);
                AverageRefundAmount = TotalRefundAmount / TotalRefunds;
            }
        }
    }
} 