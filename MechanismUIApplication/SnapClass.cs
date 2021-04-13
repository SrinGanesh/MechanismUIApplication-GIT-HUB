using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
    public class SnapClass
    {
        private double x1;
        private double y1;

        //constructor
        public double X 
        {
            set { x1 = value; }
            get { return x1; }
        }
        public double Y
        {
            set { y1 = value; }
            get { return y1; }
        }

        
    }
}
