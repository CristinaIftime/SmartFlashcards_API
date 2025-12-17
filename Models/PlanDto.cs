using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFlashcards_API.Models
{
    public class PlanDto
    {
        public Guid plan_id { get; set; }
        public string type { get; set; }
        public int price_cents { get; set; }
        public string currency { get; set; }
        public string billing_period { get; set; }
        public bool is_active { get; set; }
    }
}