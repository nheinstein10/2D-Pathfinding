using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Map;
using System.Linq;
using System;

namespace Pathfinding.Algorithms {
    public class Pathfinding : MonoBehaviour {
        public static Pathfinding instance;

        GameObject destinationNode;
        GameObject currentNode;

        List<GameObject> pathNodeQueue;
        List<GameObject> visitedNode;

        List<float> currentWeightValueList;

        public Graph g;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            SetupGameNodeGraph();
            destinationNode = MapNodeManager.instance.nearestNode;
            currentNode = MapNodeManager.instance.sourceNode;

            //g.BFS(1);
            Debug.Log(g.IsEdge(3, 4));

            int[,] graph =  {
                         { 0, 6, 0, 0, 0, 0, 0, 9, 0 },
                         { 6, 0, 9, 0, 0, 0, 0, 11, 0 },
                         { 0, 9, 0, 5, 0, 6, 0, 0, 2 },
                         { 0, 0, 5, 0, 9, 16, 0, 0, 0 },
                         { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
                         { 0, 0, 6, 0, 10, 0, 2, 0, 0 },
                         { 0, 0, 0, 16, 0, 2, 0, 1, 6 },
                         { 9, 11, 0, 0, 0, 0, 1, 0, 5 },
                         { 0, 0, 2, 0, 0, 0, 6, 5, 0 }
                            };

            int[,] gameGraph = new int[MapNodeManager.instance.GetNodeList().Count, MapNodeManager.instance.GetNodeList().Count];
            gameGraph = AlgorithmProcessing.ExtractAdjacencyMatrix(0, MapNodeManager.instance.GetNodeList());
            Debug.Log(MapNodeManager.instance.GetNodeList().Count);
            //foreach(var el in MapNodeManager.instance.GetNodeList()) {
            //    Debug.Log(el.name);
            //}

            for(int i = 0; i < MapNodeManager.instance.nodesCount; i++) {
                Debug.Log(((Func<string>)(() => {
                    string result = "";
                    for(int j = 0; j < MapNodeManager.instance.nodesCount; j++) {
                        result += gameGraph[i, j] + " ";
                    }
                    return result;
                }))());
            }

            Dijkstra.DijkstraAlgo(gameGraph, 0, MapNodeManager.instance.GetNodeList().Count);
        }

        void SetupGameNodeGraph() {
            g = new Graph(5);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(1, 2);
            g.AddEdge(2, 0);
            g.AddEdge(2, 3);
            //g.AddEdge(1, 3);
            g.AddEdge(3, 0);
            g.AddEdge(3, 4);
            //g.AddEdge(0, 4);
        }
    }

    public class Graph {
        private int V; // no. of vertices
        private List<int>[] adjList;
        public bool[,] booleanAdjMatrix;

        public Graph(int v) {
            this.V = v;
            adjList = new List<int>[V];
            booleanAdjMatrix = new bool[V, V];
            for (int i = 0; i < adjList.Length; i++) {
                adjList[i] = new List<int>();
            }
        }

        public void AddEdge(int v, int w) {
            adjList[v].Add(w);
            booleanAdjMatrix[v, w] = true;
            booleanAdjMatrix[w, v] = true;
        }

        public bool IsEdge(int v, int w) {
            return booleanAdjMatrix[v, w];
        }

        public void BFS(int s) {
            // mark all the vertices as not visited
            bool[] visited = new bool[V];
            for (int i = 0; i < visited.Length; i++) {
                visited[i] = false;
            }

            // create queue for BFS
            LinkedList<int> queue = new LinkedList<int>();

            // mark the current node as visited and enqueue it
            visited[s] = true;
            queue.AddLast(s);

            while (queue.Count != 0) {
                // dequeue a vertex from queue and print it
                s = queue.First();
                Debug.Log(s + " ");
                queue.RemoveFirst();

                // get all adjacent vertices of the dequeued vertex s. If an adjacent has not beent visited, then mark visited and enqueue it
                IEnumerator<int> e = adjList[s].GetEnumerator();
                while (e.MoveNext()) {
                    int n = e.Current;
                    if (!visited[n]) {
                        visited[n] = true;
                        queue.AddLast(n);
                    }
                }
            }
        }

        // A BFS based function to check whether d is reachable from s. 
        public bool isReachable(int s, int d) { // s: source, d: destination
            LinkedList<int> temp;

            // Mark all the vertices as not visited(By default set 
            // as false) 
            bool[] visited = new bool[V];

            // Create a queue for BFS 
            LinkedList<int> queue = new LinkedList<int>();

            // Mark the current node as visited and enqueue it 
            visited[s] = true;
            queue.AddLast(s);

            // 'i' will be used to get all adjacent vertices of a vertex 
            IEnumerator<int> i;
            while (queue.Count != 0) {
                // Dequeue a vertex from queue and print it
                s = queue.First();
                queue.RemoveFirst();

                int n;
                i = adjList[s].GetEnumerator();

                // Get all adjacent vertices of the dequeued vertex s 
                // If a adjacent has not been visited, then mark it 
                // visited and enqueue it 
                while (i.MoveNext()) {
                    n = i.Current;

                    // If this adjacent node is the destination node, 
                    // then return true 
                    if (n == d) {
                        return true;
                    }

                    // Else, continue to do BFS 
                    if (!visited[n]) {
                        visited[n] = true;
                        queue.AddLast(n);
                    }
                }
            }

            // If BFS is complete without visited d 
            return false;
        }
    }

    class Dijkstra {

        private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount) {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v) {
                if (shortestPathTreeSet[v] == false && distance[v] <= min) {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private static void Print(int[] distance, int verticesCount) {
            Debug.Log("Vertex    Distance from source");

            for (int i = 0; i < verticesCount; ++i)
                Debug.LogErrorFormat("{0}\t  {1}", i, distance[i]);
        }

        public static void DijkstraAlgo(int[,] graph, int source, int verticesCount) {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i) {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count) {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                        distance[v] = distance[u] + graph[u, v];
            }

            Print(distance, verticesCount);
        }
    }
}
