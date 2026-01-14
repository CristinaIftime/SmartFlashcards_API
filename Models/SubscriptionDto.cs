using System;

namespace SmartFlashcards_API.Models
{
    public class SubscriptionDto
    {
        public Guid subscription_id { get; set; }
        public Guid user_id { get; set; }
        public Guid plan_id { get; set; }
        public string status { get; set; }
        public DateTime current_period_start { get; set; }
        public DateTime current_period_end { get; set; }

        public PlanDto plan { get; set; } // embedded pentru afișare rapidă
    }

    public class CreateSubscriptionDto
    {
        public Guid user_id { get; set; }
        public Guid plan_id { get; set; }
    }
}
