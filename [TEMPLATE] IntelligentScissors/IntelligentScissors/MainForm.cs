using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        int SourcePoint = -1;
        List<Point> selcetionPoints;
        List<int> listOfParents;
        List<Point> Points;
        float j = .08f;
        float i = 0.6f;
        Point[] Pathhh;
        RGBPixel[,] TwoD; 
        Limits B; 
        public MainForm()
        {
            InitializeComponent();
            Points = new List<Point>();
            selcetionPoints = new List<Point>();
            var g = pictureBox1.CreateGraphics();
        }
        RGBPixel[,] ImageMatrix;
        public static Limits SQR(int S, int w, int h,int x)
        {
            
            Vector2D vect = new Vector2D((int)S%(w+1), (int)S / (w + 1)); 
            Limits lim = new Limits();
            int max_dist = 250;
            bool tes1 = vect.X > max_dist;
            bool tes2 = w - vect.X > max_dist;
            bool tes3 = vect.Y > max_dist;
            bool tes4 = h - vect.Y > max_dist;

            if (tes4)
            {
                lim.w = (int)vect.Y + max_dist;
            }
            else
            {
                lim.w = h;
            }
            if (tes3)
            {
                lim.z = (int)vect.Y - max_dist;
            }
            else
            {
                lim.z = 0;
            }
            if (tes1)
            {
                lim.x = (int)vect.X - max_dist;
            }  
            else
            {
                lim.x = 0;
            }

            if (tes2)
            {
                lim.y = (int)vect.X + max_dist;
            }
            else
            { 
              lim.y = w; 
            }
            
            
            Limits lx = new Limits();
            lx = lim;
            return lx;
        }
        
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            operationsOnGraph.buildGraph(ImageMatrix);
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

       
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            List<Point> tmp;
            int M_S = -1;
            bool notEmpty = pictureBox1.Image != null;
            if (notEmpty)
            {
                var clicked_node = (e.X+ (e.Y* ImageOperations.GetWidth(ImageMatrix)));
                if (SourcePoint != clicked_node)
                {
                    if (SourcePoint == -1)
                    {
                        M_S = clicked_node;
                    }// in the first click save frist clicked anchor   
                    else
                    {
                        if (selcetionPoints == null || Pathhh == null)
                        {
                            throw new ArgumentNullException();
                        }
                        tmp= selcetionPoints;
                        for (int i = 0; i < Pathhh.Length; i++)
                        {
                            tmp.Add(Pathhh[i]);
                        }
                        

                    }
                        
                    SourcePoint = clicked_node;
                    Points.Add(e.Location);
                    B = new Limits();
                    B = SQR(SourcePoint,
                        ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1,0);
                   
                    TwoD = getintoNewImage(B,ImageMatrix);
                    
                    Vector2D newd = new Vector2D((int)SourcePoint % ImageOperations.GetWidth(ImageMatrix), (int)SourcePoint / ImageOperations.GetWidth(ImageMatrix));
                    newd.X = newd.X - B.x;
                    newd.Y = newd.Y - B.z;
                    int yo = (int)newd.X + ((int)newd.Y * ImageOperations.GetWidth(TwoD));
                    int newsrc = yo;
                    listOfParents = DijkstraTracking.Dijjjjjkstra(newsrc, 0, TwoD, false);
                    
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            var Graphh = e.Graphics;
            Point penNode = new Point(4, 4);
            float[] DrawingPatternn = { (float)1, (float)0.000000000001 };
            Pen selcted = new Pen(Brushes.Red, 1);
            Pen cell = new Pen(Brushes.Blue, 1);
            if (ImageMatrix != null)
            {
                
                while (i < Points.Count)
                {
                    Graphh.FillEllipse(Brushes.Yellow, new Rectangle(
                        new Point(Points[i].X - penNode.X / 2, Points[i].Y - penNode.Y / 2),
                        new Size(penNode)));
                    i++;
                }
                if (Pathhh != null&& Pathhh.Length > 10)
                {
                    
                    cell.DashPattern = DrawingPatternn;
                    Graphh.DrawCurve(cell, Pathhh);
                    
                }        
                if (selcetionPoints.Count > 5 && selcetionPoints != null)
                {
                    selcted.DashPattern = DrawingPatternn;
                    e.Graphics.DrawCurve(selcted, selcetionPoints.ToArray());
                    

                }
                    
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label9.Text = e.X.ToString(); 
            label10.Text = e.Y.ToString();
            int prevNode = 0;

            if (i > j * 2 && ImageMatrix != null)
            {
                    int ww = ImageOperations.GetWidth(ImageMatrix);
                    var mouseNode = e.X + (e.Y * ww );
                  
                if (SourcePoint != -1 && prevNode != mouseNode)
                {
                    prevNode = mouseNode;
                    if (operationsOnGraph.InImage(mouseNode, B, ImageOperations.GetWidth(ImageMatrix)))
                    {
                       
                        Vector2D nextNode = new Vector2D((int)mouseNode % ImageOperations.GetWidth(ImageMatrix), (int)mouseNode / ImageOperations.GetWidth(ImageMatrix));
                        nextNode.X = nextNode.X-B.x;
                        nextNode.Y = nextNode.Y-B.z;
                        int yz = (int)nextNode.X + ((int)nextNode.Y * ImageOperations.GetWidth(TwoD));
                        int Segment_mouse = yz;
                        List<Point> pathOfNode = new List<Point>();
                        pathOfNode = DijkstraTracking.BackDrawing(listOfParents, Segment_mouse, ImageOperations.GetWidth(TwoD));
                        List<Point> Curpath = makeRoad(B, pathOfNode);
                        Pathhh = Curpath.ToArray();

                    }
                    else
                    {

                        List<int> ne = DijkstraTracking.Dijjjjjkstra(SourcePoint, mouseNode, ImageMatrix, true);
                        List<Point> p = DijkstraTracking.BackDrawing(ne, mouseNode, ImageOperations.GetWidth(ImageMatrix));
                        Pathhh = p.ToArray();
                    }
                }
                i = 0.0f;
            }
                
            
            if (i > j)
            {
                pictureBox1.Refresh();
                
            }
            i = i + .019f;
            
            
        }
  
       
        public static List<Point> makeRoad( Limits v, List<Point> Way)
        {
            int K = 0;
            while (K < Way.Count)
            { 
                Way[K] = sumRoad(v, Way[K]);
                K++;
            }
            List<Point> list = new List<Point>(Way);
            return list;
        }
        public static Point sumRoad( Limits T, Point O)
        {
            O.X = O.X + T.x;
            O.Y = O.Y + T.z;
            return O;
        }
        public static RGBPixel[,] getintoNewImage(Limits a, RGBPixel[,] ImageMatrix)
        {
            int newHeight = a.w - a.z;

            int newWidth = a.y - a.x;
            int i = 0;
            bool flag = false;

            RGBPixel[,] newImage = new RGBPixel[newHeight + 1, newWidth + 1];
            while (i <= newHeight&&flag==false)
            { 
                for (int j = 0; j <= newWidth; j++)
                {
                        newImage[i, j] = ImageMatrix[a.z + i, a.x + j];
                }
                i++;
             }

            return newImage;
        }
       

    }

}