using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    public class Tile
    {
        Tuple<int, int> _position;
        char _image;

        public Tuple<int, int> position { get { return _position; } set { _position = value; } }
        public char image { get { return _image; } set { _image = value; } }

        public Tile(Tuple<int, int> position, char image)
        {
            this.position = position;
            this.image = image;
        }
    }
}
