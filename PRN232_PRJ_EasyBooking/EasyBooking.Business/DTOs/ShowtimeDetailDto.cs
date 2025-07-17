using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Business.DTOs
{
    public class ShowtimeDetailDto
    {
       public int ShowtimeId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string RoomName { get; set; }
        public string CinemaName { get; set; }
    }
}
