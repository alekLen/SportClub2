﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class TrainingGroupDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;//название тренировки

        //public int TimeId { get; set; }
        public string? Time { get; set; }

        public int Day { get; set; }
        public string? DayName { get; set; }
         

        public string RoomName { get; set; } = string.Empty;
        public int RoomId { get; set; }//id зала

        public string CoachName { get; set; } = string.Empty;//имя тренера//
        public int CoachId { get; set; }//?id тренера
        public string? CoachPhoto { get; set; }


        public string GroupName { get; set; } = string.Empty;//имя группы//
        public int GroupId { get; set; }//?id группы

        //public string typeflag { get => "TrainingGroup"; }
    }
}
