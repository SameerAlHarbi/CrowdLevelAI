using System;

namespace HajjHkApi.Models
{
    public class CrowdLevel
    {
        public int Id { get; set; }

        public DateTime CrowdLevelDate { get; set; }

        public decimal Percentage { get; set; }

        public string LocationName { get; set; }
    }
}
