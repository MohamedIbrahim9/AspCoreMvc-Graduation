using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.ViewModels
{
    public class Polygon
    {
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }
        public Point BottomRight { get; set; }
        public Point BottomLeft { get; set; }
        public int Precision { get; set; }
    }
}
