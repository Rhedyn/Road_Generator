using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    public class Road : Tile
    {
        private Dictionary<char, bool> _links;

        public Dictionary<char, bool> links { get { return _links; } set { _links = value; } }

        public Road(Tuple<int, int> position, char image) : base(position, image)
        {
            links = new Dictionary<char, bool> { { 'n', false }, { 's', false }, { 'e', false }, { 'w', false } };
        }

        public void DetermineImage()
        {
            if (links['n'] && links['s'] && links['e'] && links['w'])
            {
                image = '╬';
            }
            else if (links['n'] && links['e'] && links['w']) // T stem north
            {
                image = '╩';
            }
            else if (links['n'] && links['s'] && links['e']) // T stem east
            {
                image = '╠';
            }
            else if (links['s'] && links['e'] && links['w']) // T stem south
            {
                image = '╦';
            }
            else if (links['n'] && links['s'] && links['w']) // T stem west
            {
                image = '╣';
            }
            else if (links['n'] && links['e']) // L0
            {
                image = '╚';
            }
            else if (links['s'] && links['e']) // L90
            {
                image = '╔';
            }
            else if (links['s'] && links['w']) // L180
            {
                image = '╗';
            }
            else if (links['n'] && links['w']) // L270
            {
                image = '╝';
            }
            else if (links['n'] && links['s']) // Vertical
            {
                image = '║';
            }
            else if (links['e'] && links['w']) // Horizontal
            {
                image = '═';
            }
        }
    }
}
