using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class TimetableDTO
    {
        public int Id { get; set; }
        public List<int> TimesId { get; set; } = new();
        public List<int>? SheduleId { get; set; }  =new();
    }
}
