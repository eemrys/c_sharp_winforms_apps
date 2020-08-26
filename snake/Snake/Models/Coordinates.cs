using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Models
{
    [Serializable]
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
