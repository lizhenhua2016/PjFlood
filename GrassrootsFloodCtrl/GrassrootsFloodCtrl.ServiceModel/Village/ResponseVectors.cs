

using System.Collections.Generic;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
    public class ResponseVectors
    {
        public int Type { get; set; }
        public List<Vectors> Vectorses { get; set; }
    }

    public class Vectors
    {
        public int Id { get; set; }
        public string Adcd { get; set; }
        public string Adcdname { get; set; }
        public string Shape { get; set; }
        
        public string Dispointname { get; set; }
    }
}