using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
        public int CoachId { get; set; }
        public int trainingId { get; set; }
        public List<int> usersId { get; set; } = new();
    }
}
