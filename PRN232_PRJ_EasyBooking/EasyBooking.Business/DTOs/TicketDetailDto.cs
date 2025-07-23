using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Business.DTOs
{
    public class TicketDetailDto : TicketDto
    {
        public string MovieTitle { get; set; }
        public string RoomName { get; set; }
        public string CinemaName { get; set; }
        public DateTime? ShowtimeStart { get; set; }
    }
}
