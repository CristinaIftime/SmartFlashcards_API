using System.Collections.Generic;

namespace SmartFlashcards_API.Models
{
    public class LimitDto
    {
        public string limit_code { get; set; }
        public int value { get; set; }
    }

    public class PlanWithLimitsDto : PlanDto
    {
        public List<LimitDto> limits { get; set; }
    }
}
