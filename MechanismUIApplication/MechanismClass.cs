using System.Collections.Generic;

namespace MechanismUIApplication
{
    public class MechanismClass
    {

        //variables
        private List<LinkClass> links;
        private List<JointClass> joints;
       
      
        public List<LinkClass> Links
        {
            set { links = value; }
            get { return links; }
        }
       
        public List<JointClass> Joints
        {
            set { joints = value; }
            get { return joints; }
        }

        public MechanismClass()
        {
            links = new List<LinkClass>();
            joints = new List<JointClass>();
           
        }

        public void PopulateMechanism()
        {
            links = new List<LinkClass>();
            joints = new List<JointClass>();
            //mouse down event 
          

        }

    }
}
