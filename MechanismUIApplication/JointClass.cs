using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechanismUIApplication
{
    public abstract class JointClass
    {
        protected LinkClass link1;
        protected LinkClass link2;
        protected NodeClass node1;
        protected NodeClass node2;
        protected double jointValue;
        protected MechanismClass parent;
        public LinkClass Link1
        {
            set { link1 = value; }
            get { return link1; }
        }

        public LinkClass Link2
        {
            set { link2 = value; }
            get { return link2; }
        }

        public NodeClass Node1
        {
            set { node1 = value; }
            get { return node1; }
        }

        public NodeClass Node2
        {
            set { node2 = value; }
            get { return node2; }
        }

        public double JointValue
        {
            set { jointValue = value; }
            get { return jointValue; }
        }
        public MechanismClass Parent
        {
            set { parent = value; }
            get { return parent; }
        }
        public JointClass()
        {

        }
}
}
