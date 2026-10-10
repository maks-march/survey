using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.Services
{
    public class SurveyState
    {
        public string? CatOwnership { get; set; }
        public int? CatsCount { get; set; }
        public string? CatName { get; set; }
        public string? CatBreed { get; set; }
        public int CatMass { get; internal set; }
        public List<string> CatTraits { get; internal set; } = new();
        public bool CatHealthy { get; internal set; }
        public DateTime CatDate { get; internal set; }
        public string CatInfo { get; internal set; }
    }
}
