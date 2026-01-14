using System;

namespace SmartFlashcards_API.Models
{
    public class UpgradeDto
    {
        public Guid plan_id { get; set; }
    }

    public class SimulateUploadDto
    {
        public int cards_target { get; set; }
    }
}
