﻿using RDFSurveyForm.Model;

namespace RDFSurveyForm.Dto.Unit_SubUnitDto
{
    public class UpdateSubunitDto
    {
        public int Id { get; set; }
        public string SubunitName { get; set; }
        public int? UnitId { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedAt { get; set; }  = DateTime.Now;                       
    }
}
