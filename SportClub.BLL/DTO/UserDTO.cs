using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class UserDTO : PersonDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;      
        public List<int> trainingId { get; set; }
        public List<int> groupsId { get; set; }
    }
}
