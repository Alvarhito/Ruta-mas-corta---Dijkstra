using System;
using System.Collections.Generic;

namespace Dijkstras{
    class Graph
    {
        Dictionary<int, Dictionary<int, int>> vertices = new Dictionary<int, Dictionary<int, int>>();

        public void add_vertex(int name, Dictionary<int, int> edges)
        {
            vertices[name] = edges;
        }

        public List<int> shortest_path(int start, int finish)
        {
            var previous = new Dictionary<int, int>();
            var distances = new Dictionary<int, int>();
            var nodes = new List<int>();

            List<int> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<int>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }
	/*
    class MainClass
    {
        public static void Main(string[] args)
        {
            Graph g = new Graph();
            g.add_vertex(0, new Dictionary<int, int>() {{1, 7}, {2, 8}});
            g.add_vertex(1, new Dictionary<int, int>() {{0, 7}, {5, 2}});
            g.add_vertex(2, new Dictionary<int, int>() {{0, 8}, {5, 6}, {6, 4}});
            g.add_vertex(3, new Dictionary<int, int>() {{5, 8}});
            g.add_vertex(4, new Dictionary<int, int>() {{7, 1}});
            g.add_vertex(5, new Dictionary<int, int>() {{1, 2}, {2, 6}, {3, 8}, {6, 9}, {7, 3}});
            g.add_vertex(6, new Dictionary<int, int>() {{2, 4}, {5, 9}});
            g.add_vertex(7, new Dictionary<int, int>() {{4, 1}, {5, 3}});

            g.shortest_path(0, 7).ForEach( x => Console.WriteLine(x) );
        }
    }*/
}