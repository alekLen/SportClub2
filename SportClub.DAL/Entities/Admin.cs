﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Admin : Person
    {
        public int Id { get; set; }
        //  public string Login { get; set; } = string.Empty;
        // public string Password { get; set; } = string.Empty;
        public int? Level { get; set; }
    }
}
