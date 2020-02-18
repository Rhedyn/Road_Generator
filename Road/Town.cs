using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    public class Town : Road
    {
        private string _name;
        public string name { get { return _name; } set { _name = value; } }

        public Town(Tuple<int, int> position, char image, string name) : base(position, image)
        {
            this.name = name;
        }
    }
}
