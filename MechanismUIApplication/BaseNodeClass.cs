using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
    public class BaseNodeClass : NodeClass
    {
        private List<BaseNodeClass> nodesList;
        public List<BaseNodeClass> NodesList
        {
            set { nodesList = value; }
            get { return nodesList; }
        }
        public BaseNodeClass()
        {
            nodesList = new List<BaseNodeClass>();
        }
    }
}
