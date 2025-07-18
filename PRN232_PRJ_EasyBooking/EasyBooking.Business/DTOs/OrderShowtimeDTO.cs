namespace EasyBooking.Business.DTOs
{
    public class OrderShowtimeDTO
    {
        public class Cinema
        {
            public int CinemaId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
        }
        public class Movie
        {
            public int MovieId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public string PosterUrl { get; set; }
            public string Status { get; set; }
            public List<string> Genres { get; set; }
            public List<string> Showtimes { get; set; } // ISO string
        }
        public class Showtime
        {
            public int ShowtimeId { get; set; }
            public int MovieId { get; set; }
            public int RoomId { get; set; }
            public DateTime StartTime { get; set; }
            public decimal? Price { get; set; }
        }
        public class Room
        {
            public int RoomId { get; set; }
            public int CinemaId { get; set; }
            public string Name { get; set; }
        }
    }
} 