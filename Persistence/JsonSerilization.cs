using System.Collections.Generic;

namespace Persistence
{
    public class JsonSerilization
    {
    public class Geometry
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }


    }
}