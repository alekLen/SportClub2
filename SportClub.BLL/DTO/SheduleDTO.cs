using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class SheduleDTO
    {
        public int Id { get; set; }
        public List<int> trainingId { get; set; }
    }
}
