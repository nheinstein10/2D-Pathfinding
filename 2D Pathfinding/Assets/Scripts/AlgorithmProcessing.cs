using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Algorithms {
    public static class AlgorithmProcessing {
        public static int[,] ExtractWeightedAdjacencyMatrix(List<GameObject> nodes) {
            int[,] adjacencyMatrix = new int[nodes.Count, nodes.Count];

            for(int i = 0; i < nodes.Count; i++) {
                for(int j = 0; j < nodes.Count; j++) {
                    if (Pathfinding.instance.g.IsEdge(i, j)) {
                        adjacencyMatrix[i, j] = Mathf.Abs((int)(nodes[i].transform.position - nodes[j].transform.position).magnitude);
                    } else {
                        adjacencyMatrix[i, j] = 0;
                    }
                }
            }

            return adjacencyMatrix;
        }
    }
}
