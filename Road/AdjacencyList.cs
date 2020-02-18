using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    public class AdjacencyList
    {
        LinkedList<Tuple<int, int>>[] adjacency_list;
        public AdjacencyList(int vertices)
        {
            adjacency_list = new LinkedList<Tuple<int, int>>[vertices];

            for (int i = 0; i < adjacency_list.Length; i++)
            {
                adjacency_list[i] = new LinkedList<Tuple<int, int>>();
            }
        }

        public void AppendEdge(int start_vertex, int end_vertex, int weight)
        {
            adjacency_list[start_vertex].AddLast(new Tuple<int, int>(end_vertex, weight));
        }
        public void PrependEdge(int start_vertex, int end_vertex, int weight)
        {
            adjacency_list[start_vertex].AddFirst(new Tuple<int, int>(end_vertex, weight));
        }
        public int Length()
        {
            return adjacency_list.Length;
        }

        // Returns a copy of the Linked List of outward edges from a vertex
        public LinkedList<Tuple<int, int>> this[int index]
        {
            get
            {
                LinkedList<Tuple<int, int>> edgeList
                               = new LinkedList<Tuple<int, int>>(adjacency_list[index]);

                return edgeList;
            }
        }
        public bool RemoveEdge(int startVertex, int endVertex, int weight)
        {
            Tuple<int, int> edge = new Tuple<int, int>(endVertex, weight);

            return adjacency_list[startVertex].Remove(edge);
        }

        public void Print()
        {
            int i = 0;

            foreach (LinkedList<Tuple<int, int>> list in adjacency_list)
            {
                Console.Write("adjacency_list[" + i + "] -> ");

                foreach (Tuple<int, int> edge in list)
                {
                    Console.Write(edge.Item1 + "(" + edge.Item2 + ") ");
                }

                ++i;
                Console.WriteLine();
            }
        }
    }
}
