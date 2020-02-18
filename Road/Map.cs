using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road
{
    public class Map
    {
        private List<List<Tile>> _tilemap;
        private List<Town> _towns;
        private AdjacencyList _links;
        private List<List<Road>> _roads;
        public List<List<Tile>> tilemap { get { return _tilemap; } set { _tilemap = value; } }
        public List<Town> towns { get { return _towns; } set { _towns = value; } }
        public AdjacencyList links { get { return _links; } set { _links = value; } }
        public List<List<Road>> roads { get { return _roads; } set { _roads = value; } }

        public Map()
        {
            tilemap = new List<List<Tile>>();
            towns = new List<Town>();
            roads = new List<List<Road>>();
        }

        /// <summary>
        /// Creates empty map of size (x_size, y_size).
        /// </summary>
        /// <param name="x_size"></param>
        /// <param name="y_size"></param>
        public void CreateEmpty(int x_size, int y_size)
        {
            for (int y = 0; y < y_size; y++)
            {
                List<Tile> row = new List<Tile>();
                for (int x = 0; x < x_size; x++)
                {
                    row.Add(new Tile(new Tuple<int, int>(x, y), '·'));
                }
                tilemap.Add(row);
            }
        }

        /// <summary>
        /// Returns Map as multi-line string.
        /// </summary>
        /// <returns></returns>
        public string AsString()
        {
            string r = "";
            foreach (List<Tile> row in tilemap)
            {
                foreach (Tile tile in row)
                {
                    r += tile.image;
                }
                r += '\n';
            }
            return r;
        }

        /// <summary>
        /// Creates towns within the map. Chunk size X, Chunk Size Y, Chunk Padding.
        /// </summary>
        /// <param name="cs_x"></param>
        /// <param name="cs_y"></param>
        /// <param name="c_pad"></param>
        public void CreateTowns(int cs_x = 1, int cs_y = 1, int c_pad = 1)
        {
            /* Chunks are 2D tile arrays
             * Each chunk contains one town
             * A chunk is a List<List<Tile> - cx * cy tiles in area
             * chunks_area are stored in a tuple
             * chunk_position is also a tuple (iterated through chunks_area)
             * 
             * Chunks are added back to the tilemap using...
             * ...cp_x * cs_x + rel_x, chunk_position[1] * cs_y + rel_y...
             * ...as the current tiles' actual position.
             */

            //Create a new empty chunk [DEBUGGING - Place a town on the 0,0 position of each chunk (will be randomised later)]
            Random r = new Random();
            for (int cp_y = 0; cp_y < Math.Floor((decimal)(tilemap.Count / cs_y)); cp_y++)
            {
                for (int cp_x = 0; cp_x < Math.Floor((decimal)(tilemap[0].Count / cs_x)); cp_x++)
                {
                    Tile[,] chunk = new Tile[cs_x, cs_y];
                    int town_x = c_pad + r.Next(Math.Max(cs_x - (c_pad * 2), 0));
                    int town_y = c_pad + r.Next(Math.Max(cs_y - (c_pad * 2), 0));
                    for (int rel_y = 0; rel_y < cs_y; rel_y++)
                    {
                        for (int rel_x = 0; rel_x < cs_x; rel_x++)
                        {
                            if (rel_x == town_x && rel_y == town_y)
                            {
                                Town t = new Town(new Tuple<int, int>(cp_y * cs_y + rel_y, cp_x * cs_x + rel_x), '☺', "Town");
                                chunk[rel_x, rel_y] = t;
                                towns.Add(t);
                            }else if (rel_x == 0 && rel_y == 0) {
                                chunk[rel_x, rel_y] = new Tile(new Tuple<int, int>(rel_x, rel_y), '·');
                            }
                            else { chunk[rel_x, rel_y] = new Tile(new Tuple<int, int>(rel_x, rel_y), '·'); }
                            tilemap[cp_y * cs_y + rel_y][cp_x * cs_x + rel_x] = chunk[rel_x, rel_y];
                        }
                    }
                }
            }
            links = new AdjacencyList(towns.Count());
        }

        /// <summary>
        /// Links two towns together. Provide their corresponding positions within Links.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void LinkTowns(int p1, int p2)
        {
            List<Tuple<int, int>> r = GenerateRoad(
                towns[p1].position.Item1, towns[p1].position.Item2,
                towns[p2].position.Item1, towns[p2].position.Item2
                );

            links.AppendEdge(p1, p2, r.Count);
            links.AppendEdge(p2, p1, r.Count);
            List<Road> road = new List<Road>();
            foreach (Tuple<int, int> coordinate in r)
            {
                if (tilemap[coordinate.Item2][coordinate.Item1].GetType() != typeof(Town))
                {
                    Road rtile = new Road(coordinate, '╬');
                    tilemap[coordinate.Item2][coordinate.Item1] = rtile;
                    road.Add(rtile);
                }
            }
            roads.Add(road);
        }

        private List<Tuple<int, int>> GenerateRoad(int y1, int x1, int y2, int x2)
        {
            List<Tuple<int, int>> coords = new List<Tuple<int, int>>();

            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);

            if (x1 < x2) // x1 to left of x2
            {
                for (int x = x1; x < x1 + dx; x++)
                {
                    coords.Add(new Tuple<int, int>(x, y1));
                }
            }
            else // x1 to right of x2
            { 
                for (int x = x1; x > x1 - 1 - dx; x--)
                {
                    coords.Add(new Tuple<int, int>(x, y1));
                }
            }
            if (y1 < y2) // y1 above y2
            {
                for (int y = y1; y < y1 + dy+1; y++)
                {
                    coords.Add(new Tuple<int, int>(x2, y));
                }
            }
            else // y1 below y2
            {
                for (int y = y1; y > y1 - 1 - dy; y--)
                {
                    coords.Add(new Tuple<int, int>(x2, y));
                }
            }
            return coords;
        }

        /// <summary>
        /// Links the roads together and calls Road.DetermineImage()
        /// </summary>
        public void LinkRoads()
        {
            foreach (List<Road> road in roads)
            {
                foreach (Road r in road)
                {                    
                    //North Check
                    if (tilemap[r.position.Item2][Math.Max(0, r.position.Item1 - 1)].GetType() == typeof(Road) ||
                        tilemap[r.position.Item2][Math.Max(0, r.position.Item1 - 1)].GetType() == typeof(Town))
                    {
                        r.links['w'] = true;
                    }
                    //South Check
                    if (tilemap[r.position.Item2][Math.Min(tilemap[0].Count-1, r.position.Item1 + 1)].GetType() == typeof(Road) ||
                        tilemap[r.position.Item2][Math.Min(tilemap[0].Count-1, r.position.Item1 + 1)].GetType() == typeof(Town))
                    {
                        r.links['e'] = true;
                    }
                    //East Check
                    if (tilemap[Math.Max(0, r.position.Item2 - 1)][r.position.Item1].GetType() == typeof(Road)||
                        tilemap[Math.Max(0, r.position.Item2 - 1)][r.position.Item1].GetType() == typeof(Town))
                    {
                        r.links['n'] = true;
                    }
                    //West Check
                    if (tilemap[Math.Min(tilemap[0].Count-1, r.position.Item2 + 1)][r.position.Item1].GetType() == typeof(Road) ||
                        tilemap[Math.Min(tilemap[0].Count-1, r.position.Item2 + 1)][r.position.Item1].GetType() == typeof(Town))
                    {
                        r.links['s'] = true;
                    }
                    r.DetermineImage();
                }
            }
        }
    }
}
