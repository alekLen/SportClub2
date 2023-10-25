using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class SaltDTO
    {
        public int Id { get; set; }
        public string? salt { get; set; }
        public int? userId { get; set; }
        public int? adminId { get; set; }
        public int? coachId { get; set; }
    }
}
