using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    class Program
    {

        static void Main(string[] args)
        {
            Map m = new Map();

            m.CreateEmpty(48, 24);

            m.CreateTowns(8, 8, 1);

            m.LinkTowns(0, 1);
            m.LinkTowns(1, 2);
            m.LinkTowns(2, 3);
            m.LinkTowns(3, 6);
            m.LinkTowns(0, 4);
            m.LinkTowns(4, 5);
            m.LinkTowns(5, 6);
            m.LinkTowns(6, 7);

            m.LinkRoads();

            //m.links.Print();

            Console.Write(m.AsString());
            Console.Read();
        }
    }
}
