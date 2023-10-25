using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class CoachDTO : PersonDTO
    {
        public int Id { get; set; }

        public int? PostId { get; set; } = null;
        public int? SpecialityId { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Photo { get; set; } = null;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual List<int> trainingsId { get; set; }
        public virtual List<int> groupsId { get; set; }

    }
}
