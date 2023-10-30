using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public  class Coach : Person
    {
        public int Id { get; set; }
        public Post? Post { get; set; } = null;
        public Speciality? Speciality { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Photo { get; set; } = null;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual List<TrainingInd>? trainingInds { get; set; }=new List<TrainingInd>();
        public virtual List<Group> groups { get; set; }=new List<Group>() { };

    }
}
