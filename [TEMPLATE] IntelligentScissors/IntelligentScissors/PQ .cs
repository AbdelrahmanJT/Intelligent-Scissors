using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public class PQ

    {
        private List<Junction> MinimumList = new List<Junction>();
        public bool Empty_check()
        {
            if (MinimumList.Count != 0)
            {
                return false;
            }
            return true;
        }
        public Junction Top()
        {
            Junction e = MinimumList[0];
            return e;
        }
        public int Size()
        {
            int e = MinimumList.Count;
            return e;
        }
        // Parent function used to return the parent of a given node
        private int grand_node(int x)
        {
            int e = (x - 1) / 2;
            return e;
        }
        // ToLeft function used to return the left node next to the given node
        private int to_left(int x)
        {
            int e = x * 2 + 1;
            return e;
        }
        // ToRight function used to return the right node next to the given node
        private int to_right(int x)
        {
            int e = x * 2 + 2;
            return e;
        }
        // Push function used to add elements to the periority Queue
        public void Push(Junction x)
        {
            MinimumList.Add(x);
            //Call function Adjust to re-arrange the elements in the List
            Adjust(MinimumList.Count - 1);
        }
        // Pop function used to remove elements to the periority Queue
        public Junction Pop()
        {
            Junction e = MinimumList[0];
            MinimumList[0] = MinimumList[MinimumList.Count - 1];
            MinimumList.RemoveAt(MinimumList.Count - 1);
            //Call function Adjust to re-arrange the elements in the List
            Adjust_after(0);
            return e;
        }
        /*
         * Adjust function is used to adjust all elements in the heap list after adding new element to it
         * It works recursively until all elements place in right places!
         */
        private void Adjust(int x)
        {
            //Basecase
            if (MinimumList[x].Cost >= MinimumList[grand_node(x)].Cost || x == 0)
            {
                return;
            }

            Junction e = MinimumList[grand_node(x)];
            MinimumList[grand_node(x)] = MinimumList[x];
            MinimumList[x] = e;
            Adjust(grand_node(x));
        }
        /*
         * AdjustAfter function is used to adjust all elements in the heap list after deleting an element from it
         * It works recursively until all elements place in right places!
         */
        private void Adjust_after(int x)
        {
            //Basecase
            if ((to_left(x) < MinimumList.Count && to_right(x) < MinimumList.Count && MinimumList[to_left(x)].Cost >= MinimumList[x].Cost &&
               MinimumList[to_right(x)].Cost >= MinimumList[x].Cost) || to_left(x) >= MinimumList.Count ||
               (to_left(x) < MinimumList.Count && to_right(x) >= MinimumList.Count && MinimumList[to_left(x)].Cost >= MinimumList[x].Cost))
            {
                return;
            }
            if (to_right(x) < MinimumList.Count && MinimumList[to_right(x)].Cost <= MinimumList[to_left(x)].Cost)
            {
                Junction e = MinimumList[to_right(x)];
                MinimumList[to_right(x)] = MinimumList[x];
                MinimumList[x] = e;
                Adjust_after(to_right(x));
            }
            else
            {
                Junction e = MinimumList[to_left(x)];
                MinimumList[to_left(x)] = MinimumList[x];
                MinimumList[x] = e;
                Adjust_after(to_left(x));
            }
        }

    }
}