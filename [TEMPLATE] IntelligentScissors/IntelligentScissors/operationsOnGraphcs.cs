using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.IO;
namespace IntelligentScissors
{

    public class Limits
    {
        public int x, y, z, w;
    }

    public class Junction
    {
        // edge class that describe where the edge going and the cost to another edge 
        public double Cost;
        public int FromNode, ToNode;
        public Junction(int _From, int _To, double _Cost)
        {
            FromNode = _From;
            ToNode = _To;
            Cost = _Cost;
        }
    }

    class operationsOnGraph
    {
        // CONVERT 1D TO 2D EQUITION >>>>> p.x = index / 3;
        //                           >>>>>.p.y = index % 3;
        //
        // CONVERT 2D TO 1D EQUITION >>>>> NewIndex= (row * length_of_row) + column;

        // struct weight for x , y pixels 
        struct weight
        {
            int X;
            int Y;

        }
        weight weightt;
        // enum to get image types of images 
        enum Image_Type
        {
            GrayScale,
            RGP
        }
        public static bool InImage(int To, Limits Lim, int w)
        {
            Vector2D Target2d = new Vector2D((int)To % w, (int)To / w);
            bool xF = false;
            bool yF = false;
            if (Target2d.X >= Lim.x && Target2d.X < Lim.y && Target2d.Y >= Lim.z && Target2d.Y < Lim.w)
            {
                xF = true;
                yF = true;

            }


            return xF && yF;
        }

        public static bool is_Valid(int j, int k, RGBPixel[,] ImageMatrix)
        {
            bool Valid_j = false, Valid_k = false;
            if (j >= 0 && j < ImageOperations.GetWidth(ImageMatrix) && k >= 0 && k < ImageOperations.GetHeight(ImageMatrix))
            {
                Valid_j = true;
                Valid_k = true;
            }

            return Valid_j && Valid_k;
        }

        // building graph from image using image matrix and list of pervious edges 
        public static Dictionary<int, List<Junction>> buildGraph(RGBPixel[,] ImageMatrix)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int i = 0, j ;
            int lenght = ImageMatrix.GetLength(0);
            int wid = ImageMatrix.GetLength(1);
            Dictionary<int, List<Junction>> neighboursList;
            List<List<Junction>> FinalList;
            FinalList = new List<List<Junction>>();
            neighboursList = new Dictionary<int, List<Junction>>();
            while (i < lenght)
            {
                for (j = 0; j < wid; j++)
                {

                    int nnnIndex = (j) + (i * wid);
                    //-------------------------------------
                    // get List of neighbours
                    List<Junction> Searching = searchForNeighbours(ImageMatrix, nnnIndex);
                    neighboursList[nnnIndex] = Searching;


                }
                //while (j < Width)
                //{
                //    // convert 2D index to 1D
                //    int nnnIndex = (j) + (i * Width);
                //    //-------------------------------------
                //    // get List of neighbours
                //    List<Edge> Searching = searchForNeighbours(ImageMatrix, nnnIndex);
                //    neighboursList.Add(Searching);
                //    ++j;
                //}

                i++;

            }

            // printing neighboursList to see if the adjecentlist that we made work succesfully or not :'
            //string filename = @"C:\Users\user\Desktop\test1.txt";
            //using (TextWriter tw = new StreamWriter(filename))
            //{
            //    foreach (var item in neighboursList)
            //    {
            //        tw.WriteLine("Node " + item.Key + " : ");
            //        foreach (var obj in item.Value)
            //        {

            //            tw.WriteLine(string.Format("FromNode: {0} ---> ToNode: {1} >> Weights:{2} ", obj.FromNode, obj.ToNode, obj.Cost));

            //        }

            //    }
            //}
            watch.Stop();
            string filename = @"C:\Users\user\Desktop\test1.txt";
            using (TextWriter tw = new StreamWriter(filename))


                tw.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            return neighboursList;
        }
        // Here We are building adjecentlist from the graph of image using function that calculate pixel energies 
        public static List<Junction> searchForNeighbours(RGBPixel[,] ImageMatrix, int nI)
        {
            int H = 0, W = 0;
            W = ImageMatrix.GetLength(1);
            Vector2D index2D = new Vector2D();

            Vector2D Weightt;

            H = ImageMatrix.GetLength(0);

            // convert 1D index to vector 2D

            index2D.X = (int)nI % W;
            index2D.Y = (int)nI / W;

            //-------------------------------------------

            int X2D = (int)index2D.X, Y2D = (int)index2D.Y;
            bool rightNeighbour = (X2D < W - 1);
            bool leftNeighbour = (X2D > 0);
            bool bottomNeighbour = (Y2D < H - 1);
            bool topNeighbour = (Y2D > 0);
            List<Junction> neighbours = new List<Junction>();
            //------------------------------------------------
            int totalSize = H * W;
            long oo = 0x2386F26FC10000;

            // calculate the weight with neighbours
            Weightt = ImageOperations.CalculatePixelEnergies(X2D, Y2D, ImageMatrix);
            // For right neighbours
            if (rightNeighbour && is_Valid(X2D, Y2D, ImageMatrix))
            {
                // convert 2D to 1D; 
                int newIndex = (X2D + 1) + (Y2D * W);

                if (Weightt.X == 0)
                    neighbours.Add(new Junction(nI, newIndex, oo));
                else
                    neighbours.Add(new Junction(nI, newIndex, 1 / (Weightt.X)));

            }
            // For Left neighbour 
            if (leftNeighbour && is_Valid(X2D, Y2D, ImageMatrix))
            {
                // calculate the weight with left neighbour
                Weightt = ImageOperations.CalculatePixelEnergies(X2D - 1, Y2D, ImageMatrix);
                // convert 2D to 1D; 
                int newIndex = (X2D - 1) + ((Y2D) * W);
                if (Weightt.X == 0)
                    neighbours.Add(new Junction(nI, newIndex, oo));

                else
                    neighbours.Add(new Junction(nI, newIndex, 1 / (Weightt.X)));


            }
            // For top neighbours
            if (topNeighbour && is_Valid(X2D, Y2D, ImageMatrix))
            {
                // convert 2D to 1D; 
                int newIndex = X2D + ((Y2D - 1) * W);
                // calculate the weight with top neighbour
                Weightt = ImageOperations.CalculatePixelEnergies(X2D, Y2D - 1, ImageMatrix);
                if (Weightt.Y == 0)
                    neighbours.Add(new Junction(nI, newIndex, oo));
                else
                    neighbours.Add(new Junction(nI, newIndex, 1 / (Weightt.Y)));

            }
            // For bottom neighbours
            if (bottomNeighbour && is_Valid(X2D, Y2D, ImageMatrix))
            {
                // convert 2D to 1D; 
                int newIndex = X2D + ((Y2D + 1) * W);
                if (Weightt.Y == 0)
                    neighbours.Add(new Junction(nI, newIndex, oo));

                else

                    neighbours.Add(new Junction(nI, newIndex, 1 / (Weightt.Y)));
            }

            foreach (var test in neighbours)
            {
                Debug.Print(test.Cost.ToString());
            }
            return neighbours;
        }

       
    }
}


