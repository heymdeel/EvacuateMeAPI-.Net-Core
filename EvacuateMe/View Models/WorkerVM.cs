﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.ViewModels
{
    public class WorkerVM
    {
        public string Name { get; set; }

        public string Phone { get; set; }
        
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfHire { get; set; }

        public string CarNumber { get; set; }
    }
}
