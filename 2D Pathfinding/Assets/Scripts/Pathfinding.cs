using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Map;
using System.Linq;

namespace Pathfinding.Gameplay {
    public class Pathfinding : MonoBehaviour {
        GameObject destinationNode;
        GameObject currentNode;

        List<GameObject> pathNodeQueue;
        List<GameObject> visitedNode;

        List<float> currentWeightValueList;

        Graph g;

        private void Start() {
            SetupGameNodeGraph();
            destinationNode = MapNodeManager.instance.nearestNode;
            currentNode = MapNodeManager.instance.sourceNode;

            g.BFS(2);
        }

        void SetupGameNodeGraph() {
            g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(1, 2);
            g.AddEdge(2, 0);
            g.AddEdge(2, 3);
            g.AddEdge(3, 3);
        }
    }

    public class Graph {
        private int V; // no. of vertices
        private List<int>[] adjList;

        public Graph(int v) {
            this.V = v;
            adjList = new List<int>[V];
            for(int i = 0; i < adjList.Length; i++) {
                adjList[i] = new List<int>();
            }
        }

        public void AddEdge(int v, int w) {
            adjList[v].Add(w);
        }

        public void BFS(int s) {
            // mark all the vertices as not visited
            bool[] visited = new bool[V];
            for(int i = 0; i < visited.Length; i++) {
                visited[i] = false;
            }

            // create queue for BFS
            LinkedList<int> queue = new LinkedList<int>();

            // mark the current node as visited and enqueue it
            visited[s] = true;
            queue.AddLast(s);

            while(queue.Count != 0) {
                // dequeue a vertex from queue and print it
                s = queue.First();
                Debug.Log(s + " ");
                queue.RemoveFirst();

                // get all adjacent vertices of the dequeued vertex s. If an adjacent has not beent visited, then mark visited and enqueue it
                IEnumerator<int> e = adjList[s].GetEnumerator();
                while (e.MoveNext()) {
                    int n = e.Current;
                    if(!visited[n]) {
                        visited[n] = true;
                        queue.AddLast(n);
                    }
                }
            }
        }
    }
}
