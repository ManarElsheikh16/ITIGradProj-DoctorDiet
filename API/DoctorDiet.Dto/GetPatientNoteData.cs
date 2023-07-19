﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class GetPatientNoteData
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string PatientId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public int DayCustomPlanId { get; set; }

        public bool Seen { get; set; }
    }
}
