using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MechanismUIApplication
{
    public abstract class LinkClass
    {
        //variables        
        protected Color color;
        protected NodeClass node1;
        protected NodeClass node2;
        protected MechanismClass parent;
        int abcd;

        //properties
        public Color Color
        {
            set { color = value; }
            get { return color; }
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

        public MechanismClass Parent
        {
            set { parent = value; }
            get { return parent; }
        }

        //constructor
        public LinkClass()
        {

        }

       

    
}
}
