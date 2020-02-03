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
            //Debug.Log(g.IsEdge(3, 4));

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

            int[,] adjacencyMatrix = { { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
                                    { 4, 0, 8, 0, 0, 0, 0, 11, 0 },
                                    { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
                                    { 0, 0, 7, 0, 9, 14, 0, 0, 0 },
                                    { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
                                    { 0, 0, 4, 0, 10, 0, 2, 0, 0 },
                                    { 0, 0, 0, 14, 0, 2, 0, 1, 6 },
                                    { 8, 11, 0, 0, 0, 0, 1, 0, 7 },
                                    { 0, 0, 2, 0, 0, 0, 6, 7, 0 } };

            int[,] gameGraph = new int[MapNodeManager.instance.GetNodeList().Count, MapNodeManager.instance.GetNodeList().Count];
            gameGraph = AlgorithmProcessing.ExtractWeightedAdjacencyMatrix(0, MapNodeManager.instance.GetNodeList());
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

            //Dijkstra.DijkstraAlgo(gameGraph, 0, MapNodeManager.instance.GetNodeList().Count);
            DijkstrasAlgorithm.dijkstra(gameGraph, 3);
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
            //g.AddEdge(3, 4);
            //g.AddEdge(0, 4);
            g.AddEdge(2, 4);
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

    public class DijkstrasAlgorithm {

        private static readonly int NO_PARENT = -1;

        // Function that implements Dijkstra's  
        // single source shortest path  
        // algorithm for a graph represented  
        // using adjacency matrix  
        // representation  
        public static void dijkstra(int[,] adjacencyMatrix,
                                            int startVertex) {
            int nVertices = adjacencyMatrix.GetLength(0);

            // shortestDistances[i] will hold the  
            // shortest distance from src to i  
            int[] shortestDistances = new int[nVertices];

            // added[i] will true if vertex i is  
            // included / in shortest path tree  
            // or shortest distance from src to  
            // i is finalized  
            bool[] added = new bool[nVertices];

            // Initialize all distances as  
            // INFINITE and added[] as false  
            for (int vertexIndex = 0; vertexIndex < nVertices;
                                                vertexIndex++) {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }

            // Distance of source vertex from  
            // itself is always 0  
            shortestDistances[startVertex] = 0;

            // Parent array to store shortest  
            // path tree  
            int[] parents = new int[nVertices];

            // The starting vertex does not  
            // have a parent  
            parents[startVertex] = NO_PARENT;

            // Find shortest path for all  
            // vertices  
            for (int i = 1; i < nVertices; i++) {

                // Pick the minimum distance vertex  
                // from the set of vertices not yet  
                // processed. nearestVertex is  
                // always equal to startNode in  
                // first iteration.  
                int nearestVertex = -1;
                int shortestDistance = int.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++) {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance) {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }

                // Mark the picked vertex as  
                // processed  
                added[nearestVertex] = true;

                // Update dist value of the  
                // adjacent vertices of the  
                // picked vertex.  
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++) {
                    int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                    if (edgeDistance > 0
                        && ((shortestDistance + edgeDistance) <
                            shortestDistances[vertexIndex])) {
                        parents[vertexIndex] = nearestVertex;
                        shortestDistances[vertexIndex] = shortestDistance +
                                                        edgeDistance;
                    }
                }
            }

            printSolution(startVertex, shortestDistances, parents);
        }

        // A utility function to print  
        // the constructed distances  
        // array and shortest paths  
        private static void printSolution(int startVertex,
                                        int[] distances,
                                        int[] parents) {
            int nVertices = distances.Length;

            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++) {
                if (vertexIndex != startVertex) {
                    Debug.Log("Vertex: ");
                    Debug.Log("\n" + startVertex + " -> ");
                    Debug.Log(vertexIndex + " \t\t ");

                    Debug.Log("Distance: ");
                    Debug.Log(distances[vertexIndex]);

                    Debug.Log("Path: ");
                    printPath(vertexIndex, parents);
                }
            }
        }

        // Function to print shortest path  
        // from source to currentVertex  
        // using parents array  
        public static void printPath(int currentVertex,
                                    int[] parents) {

            // Base case : Source node has  
            // been processed  
            if (currentVertex == NO_PARENT) {
                return;
            }
            printPath(parents[currentVertex], parents);
            Debug.Log(currentVertex + " ");
        }       
    }
}
