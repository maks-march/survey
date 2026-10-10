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
        public int CatAge { get; internal set; }
    }
}
