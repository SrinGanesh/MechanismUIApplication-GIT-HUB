using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
    public class AxialLinkClass : LinkClass
    {
        public double GetLength()
        {
            double diffX = Node1.X - Node2.X;
            double diffY = Node1.Y - Node2.Y;
            double length = Math.Sqrt(diffX * diffX + diffY * diffY);
            return length;
        }
    }
}
