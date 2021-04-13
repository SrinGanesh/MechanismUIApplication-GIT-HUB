using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
  public class PrismaticJointClass : JointClass
    {
        private double pathAngle;
        
        public double PathAngle
        {
            
            set { pathAngle = value; }
            get { return pathAngle; }
        } 
    }
}
