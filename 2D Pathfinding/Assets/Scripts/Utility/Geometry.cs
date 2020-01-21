using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Utility {
    public static class Geometry {
        public static GameObject GetNearestNodeToMouseClickPos(Vector3 mousePos, List<GameObject> nodes) {
            GameObject nearestNode = nodes[0];
            foreach(var node in nodes) {
                if((node.transform.position - mousePos).magnitude < (nearestNode.transform.position - mousePos).magnitude) {
                    nearestNode = node;
                }
            }
            return nearestNode;
        }

        public static void DrawLinePathAllNodes(List<GameObject> nodes) {
            // something
        }
    }
}
