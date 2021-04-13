using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace MechanismUIApplication
{
    public partial class Form1 : Form
    {
        private GraphPane graphPane,graphPane2;
        private GroundLinkClass _groundLink;
        
        private InputLinkClass _inputLink;
        private MechanismClass mechanism;
        private SessionDataClass sessionDataList;
        private NodeClass pointVal;
        private NodeClass node1,node2;
        private List<GraphObj> cloneList;
        LineItem lin;
        LinkClass iptLink;
        LinkClass revLink1, revLink2, prisLink1, prisLink2;
        int[,] adjMat = new int[50, 50];
        int[,] nodelist = new int[10, 2];
        private int numberOfVertices;
        private int Nodecount = -1;
        private double gridSize;
        private int SegmentCount = -1;
        private int subCount = -1;
        private double x1;
        private double y1;
        private int TriadCount = -1;
        private int pCount = -1;
        private double x2;
        private double y2;
        private int nodeNumber=0;
        private int linkSelect = -1;
        private int lSubcount;
        private int baseCount;
        private int prisSelect;
        private int prisCount;
        private int hoverPath = -1;
        private double px1, py1, px2, py2;
        double slope;
        double distance;
        double mirrPointX;
        double mirrPointY;
        private int inputCount;
        private NodeClass _inputNode;
        private int measCount;
        private int measSubCount;
        private double length;
        private int measMoveCount;
        private double measX;
        private double measY;
        int PW;
        bool Hided;

        public Form1()
        {

            InitializeComponent();
            PW = 0;
             sPanel.Width = 0;
            Hided = true;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            dataGridView1.RowCount = 100;
            
            graphPane2 = zedGraphControl2.GraphPane;
            graphPane2.Title.Text = "Steps Visualizer";
            graphPane2.XAxis.IsVisible = false;
            graphPane2.YAxis.IsVisible = false;
            graphPane = zedGraphControl1.GraphPane;
            graphPane.Title.IsVisible = false;
            mechanism = new MechanismClass();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private bool zedGraphControl1_MouseDownEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            if (Nodecount == 0)
            {
                double x1;
                double y1;
                graphPane.ReverseTransform(e.Location, out x1, out y1);
                pointSnap(x1, y1, gridSize, out x1, out y1);

         
                drawNode(x1, y1, nodeNumber);
                AddNodeToGroundedLink(x1, y1, nodeNumber);
                nodeNumber++;

            }
            //axial
            if (SegmentCount == 0)
            {


                if (subCount == 0)
                {

                    graphPane.ReverseTransform(e.Location, out x1, out y1);
                    pointSnap(x1, y1, gridSize, out x1, out y1);
                    // MessageBox.Show(Convert.ToString(x1) + "   " + Convert.ToString(y1));
                    subCount++;
                }
                else
                {


                    graphPane.ReverseTransform(e.Location, out x2, out y2);
                    pointSnap(x1, y1, gridSize, out x1, out y1);
                    pointSnap(x2, y2, gridSize, out x2, out y2);
                    drawAxialLink(x1, y1, x2, y2, Color.Blue);
                    AddNodeToAxialLink(x1, y1, x2, y2);
                    subCount = 0;

                }
            }
            //triangular
            if (TriadCount == 0)
            {
                if (pCount == 0)
                {
                    graphPane.ReverseTransform(e.Location, out x1, out y1);
                    pointSnap(x1, y1, gridSize, out x1, out y1);
                    pCount++;
                }
                else if (pCount == 1)
                {


                    graphPane.ReverseTransform(e.Location, out x2, out y2);
                    pointSnap(x2, y2, gridSize, out x2, out y2);
                    pCount++;
                }
                else
                {
                    double x3;
                    double y3;
                    graphPane.ReverseTransform(e.Location, out x3, out y3);
                    pointSnap(x3, y3, gridSize, out x3, out y3);
                    drawTriangularLink(x1, y1, x2, y2, x3, y3, Color.Blue);
                    AddToNodeTriangularLink(x1, y1, x2, y2, x3, y3);
                    pCount = 0;
                }
            }
            //revolute
            if (linkSelect == 0)
            {

                if (lSubcount == 0)
                {
                    graphPane.ReverseTransform(e.Location, out x1, out y1);

                    LinkClass _link;
                    selectSegment(x1, y1, out _link);
                    revLink1 = _link;

                    lSubcount++;
                }
                else if (lSubcount == 1)
                {
                    graphPane.ReverseTransform(e.Location, out x1, out y1);

                    LinkClass _link;
                    selectSegment(x1, y1, out _link);
                    revLink2 = _link;
                    lSubcount++;
                }
                else
                {


                    graphPane.ReverseTransform(e.Location, out x1, out y1);
                    pointSnap(x1, y1, gridSize, out x1, out y1);
                    drawNodeProp(x1, y1, SymbolType.Circle);
                  
                    
                    AddToRevoluteJoint(revLink1, revLink2, x1,y1);
                    RefreshPane();
                    lSubcount = 0;
                }
            }
            //pris
            if (prisSelect == 0)
            {

                if (prisCount == 0)
                {
                    graphPane.ReverseTransform(e.Location, out x1, out y1);

                    LinkClass _link;
                    selectSegment(x1, y1, out _link);
                    prisLink1 = _link;

                    prisCount++;
                }
                else if (prisCount == 1)
                {
                    graphPane.ReverseTransform(e.Location, out x1, out y1);

                    LinkClass _link;
                    selectSegment(x1, y1, out _link);
                    prisLink2 = _link;
                    prisCount++;
                }
                else if (prisCount == 2)
                {

                    graphPane.ReverseTransform(e.Location, out px1, out py1);
                    pointSnap(px1, py1, gridSize, out px1, out py1);
                    drawPrisPath(px1, py1);
                    pointVal = new NodeClass(px1,py1);

                    prisCount = 0;
                    hoverPath++;

                }

            }
            if (hoverPath > 0)
            { hoverPath++; }
            if (hoverPath == 3)
            {

                PointPair _pointPair1 = new PointPair(px2, py2);
                PointPair _pointPair2 = new PointPair(mirrPointX, mirrPointY);
                PointPairList _pointPairList = new PointPairList();
                _pointPairList.Add(_pointPair1);
                _pointPairList.Add(_pointPair1);
                lin = graphPane.AddCurve("", _pointPairList, Color.Green, SymbolType.Default);
                AddToPrismaticJoint(prisLink1, prisLink2, pointVal, slope);
                RefreshPane();
                hoverPath = -1;
            }
            //base
            if (baseCount == 0)
            {
                double x1;
                double y1;
                graphPane.ReverseTransform(e.Location, out x1, out y1);
                pointSnap(x1, y1, gridSize, out x1, out y1);
                drawBaseSymb(x1, y1);
                AddToBaseNode(x1, y1);

            }
            //input
            if (inputCount == 0)
            {

                double x1;
                double y1;
                graphPane.ReverseTransform(e.Location, out x1, out y1);
                pointSnap(x1, y1, gridSize, out x1, out y1);
                drawNodeProp(x1, y1, SymbolType.Circle);
                AddToInputNode(x1, y1);
        

            }
            if(measCount==0)
            {
              
                if(measSubCount ==0)
                {
                    double x1;
                    double y1;
                    graphPane.ReverseTransform(e.Location,out x1,out y1);
                    node1 = new NodeClass(x1, y1);
                    measSubCount++;
                }
               else if (measSubCount == 1)
                {
                    double x1;
                    double y1;
                    graphPane.ReverseTransform(e.Location, out x1, out y1);
                    node2 = new NodeClass(x1, y1);
                    double diffX = node1.X - node2.X;
                    double diffY = node1.Y - node2.Y;
                    length = Math.Sqrt(diffX * diffX + diffY * diffY);
                    tst.Text += Convert.ToString(length)+" "+ Convert.ToString(node1.X)+" "+ Convert.ToString(node2.X);
                    tst.Text += Environment.NewLine;
                    cloneList = new List<GraphObj>(graphPane.GraphObjList);
                    graphPane.GraphObjList.Clear();  
                    measSubCount=-1;
                    measCount = -1;
                    measMoveCount++;
                }
                
              
            }
            if (measMoveCount > 0)
            {
                measMoveCount++;
            }
            if(measMoveCount == 3)
            {
                graphPane.GraphObjList.Clear();
                TextObj text = new TextObj(
                   Convert.ToString(Math.Round(length,2)), measX, measY + 0.04,
                   CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                cloneList.Add(text);
                foreach(GraphObj obj in cloneList)
                {
                    graphPane.GraphObjList.Add(obj);
                    zedGraphControl1.Refresh();
                }
                measMoveCount = 0;
            }
            return false;
        }

        private void drawBaseSymb(double _x, double _y)
        {
            PointPair _pointPair = new PointPair(_x, _y);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair);

           LineItem curve = graphPane.AddCurve("", _pointPairList, Color.Green);
            curve.Symbol = new Symbol(SymbolType.UserDefined, Color.Black);
            curve.Symbol.Size = 50f;
            
        
            curve.Symbol.UserSymbol = new GraphicsPath(
                new[]
                {
                    new PointF(0.2f-.5f,0.3f-.5f),
                    new PointF(0.2f-.5f,0.5f-.5f),
                    new PointF(0.3f-.5f,0.5f-.5f),
                    new PointF(0.3f-.5f,0.7f-.5f),
                    new PointF(0.5f-.5f,0.8f-.5f),
                    new PointF(0.6f-.5f,0.8f-.5f),
                    new PointF(0.8f-.5f,0.7f-.5f),
                    new PointF(0.8f-.5f,0.5f-.5f),
                    new PointF(0.9f-.5f,0.5f-.5f),
                    new PointF(0.9f-.5f,0.3f-.5f),
                    new PointF(0.2f-.5f,0.3f-.5f),
                    new PointF(0.4f-.5f,0.5f-.5f),//10
                    new PointF(0.4f-.5f,0.6f-.5f),//14
                    new PointF(0.5f-.5f,0.7f-.5f),//11
                    new PointF(0.6f-.5f,0.7f-.5f),//12
                    new PointF(0.7f-.5f,0.6f-.5f),//15
                    new PointF(0.7f-.5f,0.5f-.5f),//13
                    new PointF(0.4f-.5f,0.5f-.5f),//10
                },
                new[]
                {
                    (byte)PathPointType.Start,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Start,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                    (byte)PathPointType.Line,
                }

                );
                    
                
            zedGraphControl1.Invalidate();
        }

        private void AddToInputNode(double _x, double _y)
        {
            foreach(NodeClass gnode in _groundLink.NodesList)
            {
                if (_x == gnode.X && _y == gnode.Y)
                {
                    _inputNode = new NodeClass(_x,_y,gnode.Number);
                }
            }
        }

        private void AddToBaseNode(double _x, double _y)
        {
           
            NodeClass _baseNode;
           
           
            foreach (NodeClass _node in _groundLink.NodesList)
            {
                if (_x == _node.X && _y == _node.Y)
                {
                    _baseNode = new NodeClass(_x,_y,_node.Number);
                    _groundLink.BaseNodeList.Add(_baseNode);
                }
              }
           
                   }

        private void AddToPrismaticJoint(LinkClass _prisLink1, LinkClass _prisLink2, NodeClass _Node, double angle)
        {
            PrismaticJointClass _prisJoint = new PrismaticJointClass();
            _prisJoint.Link1 = _prisLink1;
            _prisJoint.Link1 = _prisLink1;
            _prisJoint.PathAngle = angle;
            _prisJoint.Parent = mechanism;
            foreach (NodeClass _node in _groundLink.NodesList)
            {
                if (_node.X == _Node.X && _node.Y == _Node.Y)
                {
                    _prisJoint.Node1 = new NodeClass(_Node.X, _Node.Y, _node.Number);

                    _prisJoint.Node2 = new NodeClass(_Node.X, _Node.Y, _node.Number);
                }
            }
            mechanism.Joints.Add(_prisJoint);
        }

        private void drawPrisPath(double _x, double _y)
        {
            PointPair _pointPair = new PointPair(_x, _y);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair);
            graphPane.AddCurve("", _pointPairList, Color.Green, SymbolType.Square);

            lin = graphPane.AddCurve("", _pointPairList, Color.Red, SymbolType.Square);

            hoverPath = 0;
            zedGraphControl1.Invalidate();
        }

        private void AddToRevoluteJoint(LinkClass revLink1, LinkClass revLink2,double _x,double _y)
        {
            RevoluteJointClass _revJoint = new RevoluteJointClass();
            _revJoint.Link1 = revLink1;
            _revJoint.Link2 = revLink2;
            _revJoint.Parent = mechanism;
            foreach (NodeClass _node in _groundLink.NodesList)
            {
                if (_node.X == _x && _node.Y == _y)
                {
                    //MessageBox.Show("in");
                    _revJoint.Node1 = new NodeClass(_x, _y, _node.Number);

                    _revJoint.Node2 = new NodeClass(_x, _y, _node.Number);
                }
            }


            mechanism.Joints.Add(_revJoint);
        }

        private void drawNodeProp(double _x, double _y, SymbolType symBOL)
        {
            PointPair _pointPair = new PointPair(_x, _y);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair);

            graphPane.AddCurve("", _pointPairList, Color.Green, symBOL);
            zedGraphControl1.Invalidate();
        }

        private void selectSegment(double _x, double _y, out LinkClass outLink)
        {
            outLink = _groundLink;
            try
            {
                graphPane.CurveList.Clear();


                foreach (LinkClass _link in mechanism.Links)
                {

                    if (_link is AxialLinkClass)
                    {

                        AxialLinkClass _axialLink = (AxialLinkClass)_link;

                        if ((_axialLink.Node1.X == _axialLink.Node2.X || _axialLink.Node1.Y == _axialLink.Node2.Y) && ((_axialLink.Node1.X >= _x && _axialLink.Node2.X <= _x || _axialLink.Node1.X <= _x && _axialLink.Node2.X >= _x) || (_axialLink.Node1.Y >= _y && _axialLink.Node2.Y <= _y || _axialLink.Node1.Y <= _y && _axialLink.Node2.Y >= _y)))
                        {

                            outLink = _axialLink;
                            drawAxialLink(_axialLink.Node1.X, _axialLink.Node1.Y, _axialLink.Node2.X, _axialLink.Node2.Y, Color.Red);

                        }
                        else if ((_axialLink.Node1.X >= _x && _axialLink.Node2.X <= _x || _axialLink.Node1.X <= _x && _axialLink.Node2.X >= _x) && (_axialLink.Node1.Y >= _y && _axialLink.Node2.Y <= _y || _axialLink.Node1.Y <= _y && _axialLink.Node2.Y >= _y))
                        {

                            outLink = _axialLink;


                            drawAxialLink(_axialLink.Node1.X, _axialLink.Node1.Y, _axialLink.Node2.X, _axialLink.Node2.Y, Color.Red);


                        }
                        else
                        {



                            drawAxialLink(_axialLink.Node1.X, _axialLink.Node1.Y, _axialLink.Node2.X, _axialLink.Node2.Y, Color.Blue);
                        }


                    }
                    else if (_link is TriangularLinkClass)
                    {
                        TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;
                        if (((_triangularLink.Node1.X >= _x && _triangularLink.Node2.X <= _x || _triangularLink.Node1.X <= _x && _triangularLink.Node2.X >= _x) || (_triangularLink.Node2.X >= _x && _triangularLink.Node3.X <= _x || _triangularLink.Node2.X <= _x && _triangularLink.Node3.X >= _x) || (_triangularLink.Node1.X >= _x && _triangularLink.Node3.X <= _x || _triangularLink.Node1.X <= _x && _triangularLink.Node3.X >= _x)) &&( (_triangularLink.Node1.Y >= _y && _triangularLink.Node2.Y <= _y || _triangularLink.Node1.Y <= _y && _triangularLink.Node2.Y >= _y) || (_triangularLink.Node2.Y >= _y && _triangularLink.Node3.Y <= _y || _triangularLink.Node2.Y <= _y && _triangularLink.Node3.Y >= _y) || (_triangularLink.Node1.Y >= _y && _triangularLink.Node3.Y <= _y || _triangularLink.Node1.Y <= _y && _triangularLink.Node3.Y >= _y)))
                        {
                            drawTriangularLink(_triangularLink.Node1.X, _triangularLink.Node1.Y, _triangularLink.Node2.X, _triangularLink.Node2.Y, _triangularLink.Node3.X, _triangularLink.Node3.Y, Color.Red);
                            outLink = _triangularLink;
                        }
                        else
                        {
                            drawTriangularLink(_triangularLink.Node1.X, _triangularLink.Node1.Y, _triangularLink.Node2.X, _triangularLink.Node2.Y, _triangularLink.Node3.X, _triangularLink.Node3.Y, Color.Blue);
                        }
                    }

                }

            }
            catch
            { }
        }
        private double findDistance(double x, double y, double _x, double _y)
        {
            return 0;
        }

        private void AddToNodeTriangularLink(double _x1, double _y1, double _x2, double _y2, double _x3, double _y3)
        {
            TriangularLinkClass _triangularLink = new TriangularLinkClass();
            foreach (NodeClass _node in _groundLink.NodesList)
            {
                if (_x1 == _node.X && _y1 == _node.Y)
                {
                    _triangularLink.Node1 = new NodeClass(_node.X, _node.Y, _node.Number,_triangularLink);
                }
                if (_x2 == _node.X && _y2 == _node.Y)
                {
                    _triangularLink.Node2 = new NodeClass(_node.X, _node.Y, _node.Number, _triangularLink);
                }
                if (_x3 == _node.X && _y3 == _node.Y)
                {
                    _triangularLink.Node3 = new NodeClass(_node.X, _node.Y, _node.Number, _triangularLink);
                }

                
            }
            _triangularLink.Parent = mechanism;
            mechanism.Links.Add(_triangularLink);
        }

        private void drawTriangularLink(double _x1, double _y1, double _x2, double _y2, double _x3, double _y3, Color color)
        {
            PointPair _pointPair1 = new PointPair(_x1, _y1);
            PointPair _pointPair2 = new PointPair(_x2, _y2);
            PointPair _pointPair3 = new PointPair(_x3, _y3);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair1);
            _pointPairList.Add(_pointPair2);
            _pointPairList.Add(_pointPair3);
            _pointPairList.Add(_pointPair1);
            LineItem curveItem = graphPane.AddCurve("", _pointPairList, color, SymbolType.None);
            zedGraphControl1.Invalidate();

        }

        private void drawAxialLink(double _x1, double _y1, double _x2, double _y2, Color color)
        {

            //zedGraphControl1.IsAntiAlias = true;
            PointPair _pointPair1 = new PointPair(_x1, _y1);
            PointPair _pointPair2 = new PointPair(_x2, _y2);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair1);
            _pointPairList.Add(_pointPair2);
            graphPane.AddCurve("", _pointPairList, color, SymbolType.None);
            zedGraphControl1.Invalidate();

        }

        private void AddNodeToAxialLink(double _x1, double _y1, double _x2, double _y2)
        {
            AxialLinkClass _axialLink = new AxialLinkClass();
            foreach (NodeClass _node in _groundLink.NodesList)
            {
                if (_x1 == _node.X && _y1 == _node.Y)
                {

                    _axialLink.Node1 = new NodeClass(_node.X, _node.Y, _node.Number);

                }
                if (_x2 == _node.X && _y2 == _node.Y)
                {
                    _axialLink.Node2 = new NodeClass(_node.X, _node.Y, _node.Number);
                }



            }
            _axialLink.Parent = mechanism;
            mechanism.Links.Add(_axialLink);


        }
        private void AddNodeToGroundedLink(double _x, double _y, int nodeno)
        {
            //GroundLinkClass _groundLink;
            if (mechanism.Links.Count == 0)
            {
                _groundLink = new GroundLinkClass();
                mechanism.Links.Add(_groundLink);
            }
            else
            {
                _groundLink = (GroundLinkClass)mechanism.Links[0]; //0 based index
            }

            NodeClass _nodeGround = new NodeClass(_x, _y, nodeno, _groundLink);
            _groundLink.NodesList.Add(_nodeGround);
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
            
            Nodecount = 0;
            
            //axial var
            SegmentCount = -1;
            subCount = -1;
            //triad var
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;
            prisCount = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;

        }
        public void drawNode(double x, double y, int count)
        {
            PointPair _pointPair = new PointPair(x, y);
            PointPairList _pointPairList = new PointPairList();
            _pointPairList.Add(_pointPair);
            TextObj text = new TextObj(
        Convert.ToString(count), x, y + 0.04,
        CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            graphPane.AddCurve("", _pointPairList, Color.Red, SymbolType.Triangle);
            text.FontSpec.Size = 10f;
            
          //  dataGridView1.RowCount = val;
            dataGridView1.Rows[count].Cells[0].Value = Convert.ToString(count);
            dataGridView1.Rows[count].Cells[1].Value = Convert.ToString(Math.Round(x, 2));
            dataGridView1.Rows[count].Cells[2].Value = Convert.ToString(Math.Round(y, 2));
            graphPane.GraphObjList.Add(text);
            zedGraphControl1.Invalidate();


        }
        public void pointSnap(double x1, double y1, double gridSpacing, out double pointSnapX, out double pointSnapY)
        {
            x1 = Math.Round(x1, 1);
            y1 = Math.Round(y1, 1);

            double modX01 = x1 % gridSpacing;
            double modY01 = y1 % gridSpacing;
            if (modX01 <= (gridSpacing) / 2)
            {
                pointSnapX = x1 - modX01;
            }
            else if (modX01 == (gridSpacing) / 2)
            {
                pointSnapX = x1 - 0;

            }
            else
            { pointSnapX = x1 + (gridSpacing - modX01); }
            if (modY01 < (gridSpacing) / 2)
            {
                pointSnapY = y1 - modY01;
            }
            else if (modY01 == (gridSpacing) / 2)
            {

                pointSnapY = y1 - 0;
            }

            else
            { pointSnapY = y1 + (gridSpacing - modY01); }
        }

        private void button1_Click(object sender, EventArgs e)
        {  //axialVar
            SegmentCount = 0;
            subCount = 0;
            //nodeVar
            Nodecount = -1;
            //triad var
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            gridSize = Convert.ToDouble(gridSizeTxt.Text);

            zedGraphControl1.GraphPane.XAxis.MajorGrid.DashOff = 1;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.Color = Color.Black;
            zedGraphControl1.GraphPane.XAxis.Scale.MajorStep = gridSize;

            zedGraphControl1.GraphPane.YAxis.MajorGrid.DashOff = 1;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.Color = Color.Black;
            zedGraphControl1.GraphPane.YAxis.Scale.MajorStep = gridSize;
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.Refresh();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {//axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = 0;
            pCount = 0;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
           int i = 0;
            graphPane.CurveList.Clear();
            graphPane.GraphObjList.Clear();
            //_groundLink.NodesList.Clear();
            //for (int v = 0; v < numberOfVertices; v++)
            //{
            //    NodeClass _node = new NodeClass(Convert.ToDouble(dataGridView1.Rows[v].Cells[1].Value), Convert.ToDouble(dataGridView1.Rows[v].Cells[2].Value), Convert.ToInt32(dataGridView1.Rows[v].Cells[0].Value));
            //    _groundLink.NodesList.Add(_node);
            //}

            foreach (NodeClass _node in _groundLink.NodesList)
            {

                _node.X = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                _node.Y = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                TextObj text = new TextObj(
       Convert.ToString(_node.Number), _node.X, _node.Y + 0.04,
       CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                text.FontSpec.Size = 10f;
                graphPane.GraphObjList.Add(text);
                drawNodeProp(_node.X, _node.Y, SymbolType.Triangle);
                i++;
            }
            // AxialLinkClass _axialLink;
            foreach (LinkClass _link in mechanism.Links)
            {



                if (_link is AxialLinkClass)
                {
                    //object casting
                    AxialLinkClass _axialLink = (AxialLinkClass)_link;
                    _axialLink.Node1.X = Convert.ToDouble(dataGridView1.Rows[_axialLink.Node1.Number].Cells[1].Value);
                    _axialLink.Node1.Y = Convert.ToDouble(dataGridView1.Rows[_axialLink.Node1.Number].Cells[2].Value);
                    _axialLink.Node2.X = Convert.ToDouble(dataGridView1.Rows[_axialLink.Node2.Number].Cells[1].Value);
                    _axialLink.Node2.Y = Convert.ToDouble(dataGridView1.Rows[_axialLink.Node2.Number].Cells[2].Value);

                    drawAxialLink(_axialLink.Node1.X, _axialLink.Node1.Y, _axialLink.Node2.X, _axialLink.Node2.Y, Color.Blue);
                }
                else if (_link is TriangularLinkClass)
                {
                    //object casting
                    TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;
                    _triangularLink.Node1.X = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node1.Number].Cells[1].Value);
                    _triangularLink.Node1.Y = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node1.Number].Cells[2].Value);
                    _triangularLink.Node2.X = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node2.Number].Cells[1].Value);
                    _triangularLink.Node2.Y = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node2.Number].Cells[2].Value);
                    _triangularLink.Node3.X = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node3.Number].Cells[1].Value);
                    _triangularLink.Node3.Y = Convert.ToDouble(dataGridView1.Rows[_triangularLink.Node3.Number].Cells[2].Value);
                    drawTriangularLink(_triangularLink.Node1.X, _triangularLink.Node1.Y, _triangularLink.Node2.X, _triangularLink.Node2.Y, _triangularLink.Node3.X, _triangularLink.Node3.Y, Color.Blue);
                }

            }


        }
        private void RefreshPane()
        {
            graphPane.GraphObjList.Clear();
            graphPane.CurveList.Clear();
           foreach(NodeClass _node in _groundLink.BaseNodeList)
            {
                drawBaseSymb(_node.X, _node.Y);
            }
            foreach (LinkClass _link in mechanism.Links)
            {
                if (_link is GroundLinkClass)
                    {  // GroundLinkClass _groundLink = (GroundLinkClass)_link;


                    GroundLinkClass _groundLink = (GroundLinkClass)_link;
                    foreach (NodeClass _node in _groundLink.NodesList)
                    {
                        drawNodeProp(_node.X, _node.Y, SymbolType.None);
                        TextObj text = new TextObj(
       Convert.ToString(_node.Number), _node.X, _node.Y + 0.04,
       CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                        text.FontSpec.Size = 10f;
                        graphPane.GraphObjList.Add(text);
                    }
                }
                else if (_link is AxialLinkClass)
                {
                    AxialLinkClass _axialLink = (AxialLinkClass)_link;
                    drawAxialLink(_axialLink.Node1.X, _axialLink.Node1.Y, _axialLink.Node2.X, _axialLink.Node2.Y, Color.Blue);
                    
                }
                else if (_link is TriangularLinkClass)
                {
                    //object casting
                    TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;

                    drawTriangularLink(_triangularLink.Node1.X, _triangularLink.Node1.Y, _triangularLink.Node2.X, _triangularLink.Node2.Y, _triangularLink.Node3.X, _triangularLink.Node3.Y, Color.Blue);
                    
                }
            }
            foreach (JointClass _joint in mechanism.Joints)
            {
                if (_joint is RevoluteJointClass)
                {
                    RevoluteJointClass _revJoint = (RevoluteJointClass)_joint;
                   
                    drawNodeProp(_revJoint.Node1.X, _revJoint.Node1.Y, SymbolType.Circle);

                }
                else if (_joint is PrismaticJointClass)
                {
                    PrismaticJointClass _revJoint = (PrismaticJointClass)_joint;
                    drawNodeProp(_revJoint.Node1.X, _revJoint.Node1.Y, SymbolType.Square);
                    //draw pris path
                    PointPair _pointPair1 = new PointPair(px2, py2);
                    PointPair _pointPair2 = new PointPair(mirrPointX, mirrPointY);
                    PointPairList _pointPairList = new PointPairList();
                    _pointPairList.Add(_pointPair1);
                    _pointPairList.Add(_pointPair2);
                    lin = graphPane.AddCurve("", _pointPairList, Color.Green, SymbolType.Default);
                    zedGraphControl1.Invalidate();

                }
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            graphPane.CurveList.Clear();
            numberOfVertices = nodeNumber;
            RefreshPane();
                //creating adjacency matrix
                //step 1 create node list mat
                int i = -1;
                foreach (LinkClass link in mechanism.Links)
                {

                    if(link is AxialLinkClass)
                    {
                        nodelist[i, 0] = link.Node1.Number;
                        nodelist[i, 1] = link.Node2.Number;
             
                    }
                    else if (link is TriangularLinkClass)
                    {
                        TriangularLinkClass _triangle = (TriangularLinkClass)link;

                        nodelist[i, 0] = _triangle.Node1.Number;
                        nodelist[i, 1] = _triangle.Node2.Number;
                       
                        i++;
                        nodelist[i, 0] = _triangle.Node1.Number;
                        nodelist[i, 1] = _triangle.Node3.Number;
                        i++;
                        nodelist[i, 0] = _triangle.Node2.Number;
                        nodelist[i, 1] = _triangle.Node3.Number;
                  
                }
                if (link is InputLinkClass)
                {
                   
                }
                else
                {
                    i++;
                }
                }
            for (int j = 0; j < nodeNumber ;j++)
            {

                for (int b = 0; b < nodeNumber; b++)
                {

                    adjMat[j, b] = 0;

                }
            }
            for (int j = 0; j < i; j++)
            {

                
                adjMat[nodelist[j, 0], nodelist[j, 1]] = 1;
                adjMat[nodelist[j, 1], nodelist[j, 0]] = 1;
            }
            int columnCount = 1;
            adjGrid.RowCount = numberOfVertices;
            adjGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(adjGrid_RowPostPaint);
            for (int b = 0; b < numberOfVertices; b++)
            {   adjGrid.ColumnCount = columnCount;
                adjGrid.Columns[b].HeaderCell.Value = Convert.ToString(b);
                columnCount++;

            }
            for (int j = 0; j < nodeNumber; j++)
            {

            

                for (int b = 0; b < nodeNumber; b++)
                {

                    adjGrid.Rows[j].Cells[b].Value = adjMat[j, b] + " ";

                }

                
            }
        }
        

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {//axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = 0;
            lSubcount = 0;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            //stepSequence.Items.Add("Node" + " " + Convert.ToString(1) + " " + "Solved");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sessionDataList = new SessionDataClass();
            sequenceGenerator();
        }

        private void stepSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            SessionDataClass sessionDisp = sessionDataList.SessionData[stepSequence.SelectedIndex];
            DrawDiagram(sessionDisp);
            MsgDisp.Text = sessionDataList.SessionData[stepSequence.SelectedIndex].MechStatus;
            DefineQueues(sessionDisp);

        }

        private void DefineQueues(SessionDataClass sessionDisp)
        {
            if (sessionDisp.SolvedQueue.Count != 0)
            {
                int u = 0;
                solvedGrid.RowCount = 1;
               
                solvedGrid.Rows[0].HeaderCell.Value = "Solved Nodes";
                solvedGrid.Rows[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                solvedGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                

                foreach (int item in sessionDisp.SolvedQueue)
                {
                    solvedGrid.ColumnCount = u + 1;
                    solvedGrid.Rows[0].Cells[u].Value = item;
                    u++;

                }
            }
        else
            {
                solvedGrid.RowCount = 1;
                solvedGrid.ColumnCount = 1;
                solvedGrid.Rows[0].Cells[0].Value = "Queue is empty";
                 
            }
            if (sessionDisp.UnsolvedQueue.Count != 0)
            {
                int v = 0;
                unsolvedGrid.RowCount = 1;
            
                unsolvedGrid.Rows[0].HeaderCell.Value = "Unsolved Nodes";
                unsolvedGrid.Rows[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                unsolvedGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                foreach (int item in sessionDisp.UnsolvedQueue)
                {
                    unsolvedGrid.ColumnCount = v + 1;
                    unsolvedGrid.Rows[0].Cells[v].Value = item;
                    v++;

                }
            }
            else
            {
                unsolvedGrid.RowCount = 1;
                unsolvedGrid.ColumnCount = 1;
                unsolvedGrid.Rows[0].Cells[0].Value = "Queue is empty";

            }
        }

        private void zedGraphControl2_Load(object sender, EventArgs e)
        {

        }

        private void DrawDiagram(SessionDataClass sessionDisp)
        {
           // int count = 0;
            graphPane2.CurveList.Clear();
            graphPane2.GraphObjList.Clear();
            foreach (NodeClass _nodes in _groundLink.NodesList)
            {
                PointPair _pointPair = new PointPair(_nodes.X, _nodes.Y);
                PointPairList _pointPairList = new PointPairList();
                _pointPairList.Add(_pointPair);

                TextObj text = new TextObj(
                Convert.ToString(_nodes.Number), _nodes.X, _nodes.Y + 0.04,
                CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                LineItem _myCurve = graphPane2.AddCurve("", _pointPairList, sessionDisp.NodeColorList[_nodes.Number], SymbolType.Circle);
                _myCurve.Symbol.Fill = new Fill(sessionDisp.NodeColorList[_nodes.Number]);
                _myCurve.Symbol.Size = 10F;
                _myCurve.Symbol.Fill.Type = FillType.Solid;
               
                graphPane2.GraphObjList.Add(text);
                zedGraphControl2.Invalidate();
            }
            foreach (LinkClass _link in mechanism.Links)
            {
               
                if (_link is AxialLinkClass)
                {
                    AxialLinkClass _axialLink = (AxialLinkClass)_link;
                    PointPair _pointPair1 = new PointPair(_axialLink.Node1.X, _axialLink.Node1.Y);
                    PointPair _pointPair2 = new PointPair(_axialLink.Node2.X, _axialLink.Node2.Y);
                    PointPairList _pointPairList = new PointPairList();
                   _pointPairList.Add(_pointPair1);
                    _pointPairList.Add(_pointPair2);
                    LineItem _myLine = graphPane2.AddCurve("", _pointPairList, Color.Blue, SymbolType.None);
                
                    zedGraphControl2.Invalidate();
                }
                else if (_link is TriangularLinkClass)
                {
                    //object casting
                    TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;
                    PointPair _pointPair1 = new PointPair(_triangularLink.Node1.X, _triangularLink.Node1.Y);
                    PointPair _pointPair2 = new PointPair(_triangularLink.Node2.X, _triangularLink.Node2.Y);
                    PointPair _pointPair3 = new PointPair(_triangularLink.Node3.X, _triangularLink.Node3.Y);
                    PointPairList _pointPairList = new PointPairList();
                    _pointPairList.Add(_pointPair1);
                    _pointPairList.Add(_pointPair2);
                    _pointPairList.Add(_pointPair3);
                    _pointPairList.Add(_pointPair1);
                    LineItem mycurve = graphPane2.AddCurve("", _pointPairList, Color.Blue, SymbolType.None);
                    zedGraphControl2.Invalidate();
                }
            }
            zedGraphControl2.AxisChange();


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            //axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //input var 
            inputCount = 0;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        { //axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = 0;
 
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = 0;

            prisCount = 0;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = -1;
            measSubCount = -1;
            measMoveCount = -1;
        }








        private bool zedGraphControl1_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)

        {


            if (hoverPath > 0)
            {
                prisSelect = -1;
                lin.Clear();

                graphPane.ReverseTransform(e.Location, out px2, out py2);
                pointSnap(px2, py2, gridSize, out px2, out py2);
                // MessageBox.Show(Convert.ToString(px1) + "  " + Convert.ToString(px2));
                slope = Math.Atan2((py2 - py1), (px2 - px1));
                distance = Math.Sqrt(((px2 - px1) * (px2 - px1)) + ((py2 - py1) * (py2 - py1)));
                mirrPointX = px1 + distance * Math.Cos(slope + Math.PI);
                mirrPointY = py1 + distance * Math.Sin(slope + Math.PI);
                PointPair _pointPair1 = new PointPair(px2, py2);
                PointPair _pointPair2 = new PointPair(mirrPointX, mirrPointY);







                PointPairList _pointPairList = new PointPairList();
                _pointPairList.Add(_pointPair1);
                _pointPairList.Add(_pointPair2);
                lin = graphPane.AddCurve("", _pointPairList, Color.Green, SymbolType.Default);

                zedGraphControl1.Refresh();
                return false;
            }
            if(measMoveCount > 0 )
            {
                
              
             if(graphPane.GraphObjList.Count>0)
                {
                    graphPane.GraphObjList.Clear();
                }
                graphPane.ReverseTransform(e.Location, out measX, out  measY);
                TextObj text = new TextObj("L = "+
     Convert.ToString(Math.Round(length,3)), measX, measY + 0.04,
     CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                text.FontSpec.Size = 10f;
                graphPane.GraphObjList.Add(text);
                zedGraphControl1.Refresh();
            }
            return false;
        }

        private void splitContainer11_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sequenceGenerator()
        {
            stepSequence.ClearSelected();
            codeBox.Clear();
            SessionDataClass sessionVar = new SessionDataClass();
            SessionDataClass _intialVar = new SessionDataClass();
            sessionVar.StepCount = 1;
            codeTemplate();
            for (int i = 0; i < numberOfVertices; i++)
            {
                //intial color of nodes is black
                sessionVar.NodeColorList.Add(Color.Black);
                _intialVar.NodeColorList.Add(Color.Black);
                sessionVar.ParentNode.Add(-1);

            }
           
            foreach (NodeClass _node in _groundLink.BaseNodeList)
            {
              
                sessionVar.UnsolvedQueue.Enqueue(_node.Number);
                _intialVar.UnsolvedQueue.Enqueue(_node.Number);
                sessionVar.NodeColorList[_node.Number] = Color.Gold;
                _intialVar.NodeColorList[_node.Number] = Color.Gold;
            }

            stepSequence.Items.Add("Step "+Convert.ToString(sessionVar.StepCount)+" : "+"Intial Step" );
            _intialVar.MechStatus = "Intially the fixed nodes are added to the queue";
            sessionDataList.SessionData.Add(_intialVar);
            sessionVar.StepCount++;
            // checking for nodes
            int iter_val = 0;
          while(sessionVar.UnsolvedQueue.Count!=0)
            {
                
           
                bool found = false;int u, w;
                int item = sessionVar.UnsolvedQueue.Dequeue();
                //checking for base node
                foreach (NodeClass _node in _groundLink.BaseNodeList)
                    {
                       
                        if (item == _node.Number)
                        {
                            found = true;
                            //display code text
                            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                            sessionVar.MechStatus = "Node " + Convert.ToString(item) + " " + "is a base node, hence it's position is solved";
                            baseSolverCodeText(item);

                        }


                    }
                //for revolute and prismatic
                foreach (JointClass _joint in mechanism.Joints)
                {
                    if (_joint is RevoluteJointClass)
                    {
                  
                        //rev joint can be a 
                        RevoluteJointClass _revoluteJoint = (RevoluteJointClass)_joint;
                        if (item == _revoluteJoint.Node1.Number)
                        {
                                                     
                            if (sessionVar.ParentNode[item] == _inputNode.Number)
                            {
                                
                                found = true;
                                stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                                sessionVar.MechStatus = "Node " + Convert.ToString(item) + " " + " is adjacent to input node " + Convert.ToString(sessionVar.ParentNode[item]) + "present on the input link";
                                inputSolverCodeText(item);  
                            }
                            else if (_revoluteJoint.rpDyadFinder(item, numberOfVertices, adjMat ,sessionVar, out u, out w))
                            {
                                found = true;
                                stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                                sessionVar.MechStatus = "Node " + Convert.ToString(item) + " forms RR-dyad with " + Convert.ToString(u) + " and " + Convert.ToString(w);
                                rrDyadSolverCodeText(item);
                            }
                        } 
                    }
                    else if (_joint is PrismaticJointClass)
                    {
                        PrismaticJointClass _prismaticJoint = (PrismaticJointClass)_joint;
                        if (item == _prismaticJoint.Node1.Number)
                        {
                            found = true;

                            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                            sessionVar.MechStatus = "Node " + Convert.ToString(item) + "is a prismatic joint";
                            rpDyadSolverCodeText(item);
                        }
                    }
                    //add prop if any other joint needs to be added in future
                }
                    //coupler node
                    foreach (LinkClass _link in mechanism.Links)
                        {
                            if (_link is TriangularLinkClass)
                            {
                                TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;
                                if (_triangularLink.couplerFinder(item,numberOfVertices,adjMat,sessionVar ,out u, out w))
                                {
                                    found = true;
                            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                            sessionVar.MechStatus = "Node " + Convert.ToString(item) + " forms Coupler with " + Convert.ToString(u) + " and " + Convert.ToString(w);
                            couplerSolverCodeText(item);
                        }

                            }
                        }
                    


             
                if (found == true)
                {
                    //solved nodes are green
                    sessionVar.NodeColorList[item] = Color.Green;
                    sessionVar.SolvedQueue.Enqueue(item);
                    int i = item;
                    for (int j = 0; j < numberOfVertices; j++)
                    {

                        if (adjMat[i, j] == 1)
                        {
                            if (sessionVar.NodeColorList[j] == Color.Black)
                            {
                                sessionVar.ParentNode[j] = item; 
                                sessionVar.NodeColorList[j] = Color.Gold;
                                sessionVar.UnsolvedQueue.Enqueue(j);
                                
                                
                            }
                        }

                    }
              
                }
                else
                {
                    //MessageBox.Show(Convert.ToString(sessionVar.UnsolvedQueue.Count));
                    if(iter_val < numberOfVertices)
                    {//psuhing back in the queue 
                        //sessionVar.UnsolvedQueue.Dequeue();
                        sessionVar.UnsolvedQueue.Enqueue(item);
                        stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " +"Node "+ Convert.ToString(item)+" is pushed for next iteration");
                        sessionVar.MechStatus = "Could not find any pattern to solve the node";
                      
                    }
                    else
                    {
                        stepSequence.Items.Add("Unable to solve");
                        sessionVar.MechStatus = "Unable to solve";
                        break;
                    }

                    iter_val++;
                }
               
                SessionDataClass sessionClone = sessionVar.Clone();
                sessionDataList.SessionData.Add(sessionClone);
                sessionVar.StepCount++;
            }
               
            }

        private void splitContainer6_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void adjGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(adjGrid.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void sButton_Click(object sender, EventArgs e)
        {
            if(Hided)
            { sButton.Text = "HIDE"; }
            else
            {
                sButton.Text = "DISPLAY";
            }
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Hided)
            {
                sPanel.Width = sPanel.Width + 1000;
                if (sPanel.Width >= PW)
                {
                    timer1.Stop();
                    Hided = false;
                    this.Refresh();
                }
            }
            else
            {

                sPanel.Width = sPanel.Width - 1000;
                if (sPanel.Width <= 0)
                {
                    timer1.Stop();
                    Hided = true;
                    this.Refresh();
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            //axialVar
            SegmentCount = -1;
            subCount = -1;
            //nodeVar
            Nodecount = -1;
            //triad count
            TriadCount = -1;
            pCount = -1;
            //rev var
            linkSelect = -1;
            lSubcount = -1;
            //pris var
            prisSelect = -1;

            prisCount = -1;
            hoverPath = -1;
            //bse var
            baseCount = -1;
            //inpt
            inputCount = -1;
            //meaure var
            measCount = 0;
            measSubCount = 0;
            measMoveCount = 0;
        }

        public void rpDyadSolverCodeText(int u)
        {
            codeBox.Text += "//solving for node" + "  " + Convert.ToString(u) + Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public void Solver_" + Convert.ToString(u) + "(double x1, double y1, double x2, double y2, double l1, double alpha,out double t10,out double t11,out double t20,out double t21,out double d1,out double d2)";
           
            
            codeBox.Text += "{" + Environment.NewLine;
           
            codeBox.Text += "rp_dyad(x1,y1,x2,y2,l1,alpha,out t10,out t11,out t20,out t21,out d1,out d2);" + Environment.NewLine;
            
            codeBox.Text += "}";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
        }
        public void baseSolverCodeText(int u)
        {
            codeBox.Text += "//solving for node" + "  " + Convert.ToString(u) + Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public void Solver_" + Convert.ToString(u) + "(double b_x,double b_y,out double x1,out double y1)";
         
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{" + Environment.NewLine;
      
            codeBox.Text += "BaseSolver(b_x,b_y,out x1,out y1);" + Environment.NewLine;
          
            codeBox.Text += "}";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;


        }
        public void inputSolverCodeText(int u)
        {
            codeBox.Text += "//solving for node" + "  " + Convert.ToString(u) + Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public void Solver_" + Convert.ToString(u) + "(int u, int u1, double[,] points, double[] linkLengths, double theta1, int[,] nodelist, int numberOfVertices,out double x1,out double y1)";
           
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{" + Environment.NewLine;
         
            codeBox.Text += "inputSolver(u,u1,points,linkLengths,theta1,nodelist,numberOfVertices,out x1,out y1);" + Environment.NewLine;
           
            codeBox.Text += "}";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
        }
        public void rrDyadSolverCodeText(int u)
        {
            codeBox.Text += "//solving for node" + "  " + Convert.ToString(u) + Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public void Solver_" + Convert.ToString(u) + "(double x1, double y1, double x2, double y2, double l1, double l2,out double theta1,out double theta2,out double theta3,out double theta4)";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{" + Environment.NewLine;
            codeBox.Text += "rr_dyad(x1,y1,x2, y2,l1,l2,out theta1,out theta2,out theta3,out theta4);" + Environment.NewLine;
          
            codeBox.Text += "}";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;


        }
        public void couplerSolverCodeText(int u)
        {

            codeBox.Text += "//solving for node" + "  " + Convert.ToString(u) + Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public void Solver_" + Convert.ToString(u) + "(double x1, double y1, double x2, double y2, double l1, double l2,out double theta1,out double theta2,out double theta3,out double theta4)";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{" + Environment.NewLine;
            codeBox.Text += "couplerSolver(x1,y1,x2, y2,l1,l2,out theta1,out theta2,out theta3,out theta4);" + Environment.NewLine;
            codeBox.Text += "}";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;

        }

        public void codeTemplate()
        {
            codeBox.Text += "using System;";
            codeBox.Text += Environment.NewLine;
          
            codeBox.Text += "using System.Runtime.InteropServices;";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "using System.Text;";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "namespace DLL";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public class Class1";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "{";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public static extern  void inputSolver(int u, int u1, double[,] points, double[] linkLengths, double theta1, int[,] nodelist, int numberOfVertices,out double x1,out double y1);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public static extern int rr_dyad(double x1, double y1, double x2, double y2, double l1, double l2,out double theta1,out double theta2,out double theta3,out double theta4);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public static extern void BaseSolver(double x1,double y1,out double b_x,out double b_y);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public static extern void couplerSolver(double x1, double y1, double x2, double y2, double l1, double l2,out double theta1,out double theta2,out double theta3,out double theta4);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "public static extern void rp_dyad(double x1, double y1, double x2, double y2, double l1, double alpha,out double t10,out double t11,out double t20,out double t21,out double d1,out double d2);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;
            codeBox.Text += "[DllImport(\"Solver_Library.dll\", CallingConvention = CallingConvention.Cdecl)]";
            codeBox.Text += "public static extern void stephenson_solver(double xa,double ya,double xe,double ye,double l1,double l2,double l3,double l4,double l5,double l6,double l7,double theta,double alph1,double alph2,out double phi1,out double phi2,out double phi3,out double phi4);";
            codeBox.Text += Environment.NewLine;
            codeBox.Text += Environment.NewLine;

        }
        private void MechanismSolver()
        {
            stepSequence.ClearSelected();
            codeBox.Clear();
            SessionDataClass sessionVar = new SessionDataClass();
            SessionDataClass _intialVar = new SessionDataClass();
            sessionVar.StepCount = 1;
            codeTemplate();
            for (int i = 0; i < numberOfVertices; i++)
            {
                //intial color of nodes is black
                sessionVar.NodeColorList.Add(Color.Black);
                _intialVar.NodeColorList.Add(Color.Black);
                sessionVar.ParentNode.Add(-1);

            }

            foreach (NodeClass _node in _groundLink.BaseNodeList)
            {

                sessionVar.UnsolvedQueue.Enqueue(_node.Number);
                _intialVar.UnsolvedQueue.Enqueue(_node.Number);
                sessionVar.NodeColorList[_node.Number] = Color.Gold;
                _intialVar.NodeColorList[_node.Number] = Color.Gold;
            }

            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Intial Step");
            _intialVar.MechStatus = "Intially the fixed nodes are added to the queue";
            sessionDataList.SessionData.Add(_intialVar);
            sessionVar.StepCount++;
            // checking for nodes
            int iter_val = 0;
            while (sessionVar.UnsolvedQueue.Count != 0)
            {


                bool found = false; int u, w;
                int item = sessionVar.UnsolvedQueue.Dequeue();
                //checking for base node
                foreach (NodeClass _node in _groundLink.BaseNodeList)
                {

                    if (item == _node.Number)
                    {
                        found = true;
                        //display code text
                        stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                        sessionVar.MechStatus = "Node " + Convert.ToString(item) + " " + "is a base node, hence it's position is solved";
                        baseSolverCodeText(item);

                    }


                }
                //for revolute and prismatic
                foreach (JointClass _joint in mechanism.Joints)
                {
                    if (_joint is RevoluteJointClass)
                    {

                        //rev joint can be a 
                        RevoluteJointClass _revoluteJoint = (RevoluteJointClass)_joint;
                        if (item == _revoluteJoint.Node1.Number)
                        {

                            if (sessionVar.ParentNode[item] == _inputNode.Number)
                            {

                                found = true;
                                stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                                sessionVar.MechStatus = "Node " + Convert.ToString(item) + " " + "is adjacent to input node " + Convert.ToString(sessionVar.ParentNode[item]) + "present on the input link";
                                inputSolverCodeText(item);
                            }
                            else if (_revoluteJoint.rpDyadFinder(item, numberOfVertices, adjMat, sessionVar, out u, out w))
                            {
                                found = true;
                                stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                                sessionVar.MechStatus = "Node " + Convert.ToString(item) + "forms RP-dyad with " + Convert.ToString(u) + " and " + Convert.ToString(w);
                                rrDyadSolverCodeText(item);
                            }
                        }
                    }
                    else if (_joint is PrismaticJointClass)
                    {
                        PrismaticJointClass _prismaticJoint = (PrismaticJointClass)_joint;
                        if (item == _prismaticJoint.Node1.Number)
                        {
                            found = true;

                            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                            sessionVar.MechStatus = "Node " + Convert.ToString(item) + "is a prismatic joint";
                            rpDyadSolverCodeText(item);
                        }
                    }
                    //add prop if any other joint needs to be added in future
                }
                //coupler node
                foreach (LinkClass _link in mechanism.Links)
                {
                    if (_link is TriangularLinkClass)
                    {
                        TriangularLinkClass _triangularLink = (TriangularLinkClass)_link;
                        if (_triangularLink.couplerFinder(item, numberOfVertices, adjMat, sessionVar, out u, out w))
                        {
                            found = true;
                            stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node" + " " + Convert.ToString(item) + " " + "Solved");
                            sessionVar.MechStatus = "Node " + Convert.ToString(item) + "forms Coupler with " + Convert.ToString(u) + " and " + Convert.ToString(w);
                            couplerSolverCodeText(item);
                        }

                    }
                }




                if (found == true)
                {
                    //solved nodes are green
                    sessionVar.NodeColorList[item] = Color.Green;
                    sessionVar.SolvedQueue.Enqueue(item);
                    int i = item;
                    for (int j = 0; j < numberOfVertices; j++)
                    {

                        if (adjMat[i, j] == 1)
                        {
                            if (sessionVar.NodeColorList[j] == Color.Black)
                            {
                                sessionVar.ParentNode[j] = item;
                                sessionVar.NodeColorList[j] = Color.Gold;
                                sessionVar.UnsolvedQueue.Enqueue(j);


                            }
                        }

                    }

                }
                else
                {
                    //MessageBox.Show(Convert.ToString(sessionVar.UnsolvedQueue.Count));
                    if (iter_val < numberOfVertices)
                    {//psuhing back in the queue 
                        //sessionVar.UnsolvedQueue.Dequeue();
                        sessionVar.UnsolvedQueue.Enqueue(item);
                        stepSequence.Items.Add("Step " + Convert.ToString(sessionVar.StepCount) + " : " + "Node " + Convert.ToString(item) + " is pushed for next iteration");
                        sessionVar.MechStatus = "Could not find any pattern to solve the node";

                    }
                    else
                    {
                        stepSequence.Items.Add("Unable to solve");
                        sessionVar.MechStatus = "Unable to solve";
                        break;
                    }

                    iter_val++;
                }

                SessionDataClass sessionClone = sessionVar.Clone();
                sessionDataList.SessionData.Add(sessionClone);
                sessionVar.StepCount++;
            }

        }
    }

    
    
}                            
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        

















































































































































































                                    















                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    

                                   
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    

                                    





































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































