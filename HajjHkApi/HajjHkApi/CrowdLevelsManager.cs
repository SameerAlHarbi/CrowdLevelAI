using HajjHkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HajjHkApi
{
    public class CrowdLevelsManager
    {
        public List<CrowdLevel> CrowdLevels { get; set; }

        public CrowdLevelsManager()
        {
            this.CrowdLevels = new List<CrowdLevel>();
        }
    }
}
