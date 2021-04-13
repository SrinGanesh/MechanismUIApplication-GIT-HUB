using System.Collections.Generic;
using System.Drawing;

namespace MechanismUIApplication
{
  public class SessionDataClass
    {
        private Queue<int> unsolvedQueue;
        private Queue<int> solvedQueue;
        private List<Color> nodeColorList;
        private List<int> parentnode;
        private string mechStatus;
        private string cSharpSolverCode;
        private List<SessionDataClass> sessionData;
        private int stepCount;
        private List<NodeClass> solutionList;
         public Queue<int> UnsolvedQueue
        {
            set { unsolvedQueue = value; }
            get { return unsolvedQueue; }
        }
        public Queue<int> SolvedQueue
        {
            set { solvedQueue=value; }
            get { return solvedQueue; }
        }
        public List<Color> NodeColorList
        {
            set { nodeColorList = value; }
            get { return nodeColorList; }
        }
        public List<NodeClass> SolutionList
        {
            set { solutionList = value; }
            get { return solutionList; }
        }

        public List<int> ParentNode
        {
            set {parentnode = value; }
            get { return parentnode; }
        }
        public string CSharpSolverCode
        {
            set { cSharpSolverCode = value; }
            get {return cSharpSolverCode; }
        }
        public string MechStatus
        {
            set {   mechStatus = value; }
            get { return mechStatus; }
        }
        public List<SessionDataClass> SessionData
        {
           set {  sessionData= value ; }
            get { return sessionData; }
        }
        public int  StepCount
        {
            set { stepCount = value; }
            get { return stepCount; }
        }
        public SessionDataClass()
        {
            parentnode = new List<int>();
            solvedQueue = new Queue<int>();
            unsolvedQueue = new Queue<int>();
            nodeColorList = new List<Color>();
            sessionData = new List<SessionDataClass>();
            solutionList = new List<NodeClass>();
           // mechStatus = "";
        
        }
        public SessionDataClass Clone()
        {
            SessionDataClass session = new SessionDataClass();
            session.ParentNode = new List<int>(this.ParentNode);
            session.MechStatus = this.MechStatus;
            session.CSharpSolverCode = this.CSharpSolverCode;
            session.StepCount = this.StepCount;
            session.SolvedQueue = new Queue<int>(this.SolvedQueue);
            session.UnsolvedQueue = new Queue<int>(this.UnsolvedQueue);
            session.NodeColorList = new List<Color>(this.NodeColorList);//creating a clone of list
            return session;
        }
       

    }
}
