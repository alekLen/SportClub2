using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class User : Person
    {
        public int Id { get; set; }
       
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<TrainingInd> trainingInds { get; set; }
        public List<Group> groups { get; set; }
    }
}
