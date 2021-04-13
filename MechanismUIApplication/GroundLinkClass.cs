using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
    public class GroundLinkClass : LinkClass
    {
        private List<NodeClass> nodesList;
        private List<NodeClass> baseNodesList;

        public List<NodeClass> NodesList
        {
            set { nodesList = value; }
            get { return nodesList; }
        }
        public List<NodeClass> BaseNodeList
          {
            set { baseNodesList = value; }
            get { return baseNodesList; }
        }
        public GroundLinkClass()
        {
            nodesList = new List<NodeClass>();
            baseNodesList = new List<NodeClass>();
        }
    }
}
