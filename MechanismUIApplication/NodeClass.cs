using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MechanismUIApplication
{
   public class NodeClass
    {
        //variables
        private double x;
        private double y;
        private int number;
        private NoteType noteType;
        private LinkClass parent;

        //properties
        public double X
        {
            set { x = value; }
            get { return x; }
        }

        public double Y
        {
            set { y = value; }
            get { return y; }
        }
        public int Number
        {
            set { number = value;}
            get { return number;}
        }

        public NoteType NoteType
        {
            set { noteType = value; }
            get { return noteType; }
        }

        public LinkClass Parent
        {
            set { parent = value; }
            get { return parent; }
        }

        //constructor
        public NodeClass()
        { }

        public NodeClass(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public NodeClass(double _x, double _y,int _number ,LinkClass _parentLink)
        {
            x = _x;
            y = _y;
            parent = _parentLink;
            number = _number;
            noteType = NoteType.Unsolved;
        }
        public NodeClass(double _x, double _y, int _number)
        {
            x = _x;
            y = _y;
           
            number = _number;
           
        }
        public Color GetColor()
        {
            Color _returnColor = Color.Red;
            switch (noteType)
            {
                case NoteType.Unsolved:
                    _returnColor = Color.Red;
                    break;
                case NoteType.InQueue:
                    _returnColor = Color.Orange;
                    break;
                case NoteType.Solved:
                    _returnColor = Color.DarkGreen;
                    break;
            }

            return _returnColor;
        }
    }
    public enum NoteType
    {
        Unsolved,
        InQueue,
        Solved
    }
}
