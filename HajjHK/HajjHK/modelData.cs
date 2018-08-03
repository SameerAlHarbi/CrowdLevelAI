using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HajjHK
{
    public class ModelData
    {
        public string Id { get; set; }

        public List<Prediction> Predictions { get; set; }

        public ModelData()
        {
            this.Predictions = new List<Prediction>();
        }
    }

    public class Prediction
    {
        public decimal Probability { get; set; }

        public string TagId { get; set; }

        public string TagName { get; set; }
    }
}
