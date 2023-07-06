using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace IntelligentScissors
{
    

    class DijkstraTracking
    {
        
            public static double INFinite = 0x8AC7230489E80000;
            
            public static List<Point> BackDrawing(List<int> list, int Node, int mw)
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                Stack<int> PerviosStak = new Stack<int>();
                 List<Point> shortestPoints = new List<Point>();
                
                PerviosStak.Push(Node);
                int edgeIndex = list[Node];

                for(; edgeIndex != -1;)
                {
                    PerviosStak.Push(edgeIndex);
                    edgeIndex = list[edgeIndex];
                }

                for(; PerviosStak.Count != 0;)
                {

                    int X = PerviosStak.Pop();
                    int Y = mw;
                    Vector2D p = new Vector2D((int)(X % Y), (int)(X / Y));
                    Point point = new Point((int)p.X, (int)p.Y);
                    shortestPoints.Add(point);

                }
                //string filename = @"C:\Users\user\Desktop\test1.txt";
                //using (TextWriter tw = new StreamWriter(filename))
                //{
                //    foreach (var item in shortestPoints)
                //    {
                //        tw.WriteLine("Path for Clicked node" + item + " : ");



                //        tw.WriteLine(string.Format("PositionX: {0} ---> PositionY: {1} >> ", item.X, item.Y));


                //    }
                //}
                




                return shortestPoints;
              


             }

            

            
            public static List<int> Dijjjjjkstra(int S, int D, RGBPixel[,] ImageMatrix, bool flag)
            {

                int Width = ImageMatrix.GetLength(1);
                int Height = ImageMatrix.GetLength(0);
                int Totalnumber = Width * Height;
                List<double> CostDistance = Enumerable.Repeat(INFinite, Totalnumber).ToList();
                


                List<int> pervPoints = Enumerable.Repeat(-1, Totalnumber).ToList();



                PQ pq = new PQ();
                pq.Push((new Junction(-1, S, 0)));
                for (;pq.Size() != 0;)
                {

                    Junction currentNode = pq.Top();
                    pq.Pop();

                    if (currentNode.Cost >= CostDistance[currentNode.ToNode])
                    {
                    continue;
                    }
            
                    pervPoints[currentNode.ToNode] = currentNode.FromNode;
                    CostDistance[currentNode.ToNode] = currentNode.Cost;
                    if (currentNode.ToNode == D && flag == true) break;

                    List<Junction> neibours = operationsOnGraph.searchForNeighbours(ImageMatrix, currentNode.ToNode);
                    for (int i = 0; i < neibours.Count; i++)
                    {
                        Junction HeldEdge = neibours[i];

                        if (CostDistance[HeldEdge.ToNode] > CostDistance[HeldEdge.FromNode] + HeldEdge.Cost)
                        {

                            HeldEdge.Cost = CostDistance[HeldEdge.FromNode] + HeldEdge.Cost;
                            pq.Push(HeldEdge);
                        }
                    }
                }
            
                return pervPoints;
            }

        
    }
}
