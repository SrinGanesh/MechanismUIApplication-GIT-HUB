using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MechanismUIApplication
{
    public class RevoluteJointClass : JointClass
    {

        public bool rpDyadFinder(int u,int numberOfVertices,int[,] adjMat,SessionDataClass _session,out int u1, out int w)
        {
            u1 = -1; w = -1;
            int count = 0;
            int count1 = 0;
            for (int j = 0; j < numberOfVertices; j++)
            {
                if (adjMat[u, j] == 1)
                {
                    if (_session.NodeColorList[j] == Color.Green)
                    {
                        count++;
                        if (count == 1)
                            u1 = j;
                        if (count == 2)
                            w = j;
                    }
                }
            }
            if (count == 2)
            {
                for (int j = 0; j < numberOfVertices; j++)
                {
                    if (adjMat[u1, j] == 1)
                    {
                        if (j == w)
                        {
                            count1++;
                        }
                    }
                }
            }
            else
                count1 = -1;
            if (count1 == 1)
                return false;
            else if (count1 == 0)
                return true;
            else
                return false;
        }
        //specific code to revoluteJoint goes here
    }
}
