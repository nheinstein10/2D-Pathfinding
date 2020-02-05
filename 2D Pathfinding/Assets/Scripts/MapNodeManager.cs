using Pathfinding.Utility;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Algorithms;

namespace Pathfinding.Map {
    [ExecuteInEditMode]
    public class MapNodeManager : MonoBehaviour {
        public static MapNodeManager instance;

        public List<GameObject> nodes;

        public int nodesCount;

        public GameObject sourceNode;
        [HideInInspector] public GameObject nearestNode;
        public int theNearestNodeIndex = default;

        [SerializeField] Material lineMaterial;

        public int[,] gameGraph = default;
        public List<int> currentSolutionList = default;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            sourceNode = nodes[0];
            gameGraph = AlgorithmProcessing.ExtractWeightedAdjacencyMatrix(nodes);
            //DrawLine();
            DrawLineAllConnectedNodes();
        }

        public void DrawLine() {
            var line = new GameObject();
            line.AddComponent<LineRenderer>();
            line.name = "Line-0";
            Vector3[] positions = new Vector3[2];
            positions[0] = nodes[0].transform.position;
            positions[1] = nodes[1].transform.position;
            line.GetComponent<LineRenderer>().SetPositions(positions);
            line.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
            line.GetComponent<LineRenderer>().material = lineMaterial;
        }

        // overload
        public void DrawLine(Vector3 pos1, int nodeIndex1, Vector3 pos2, int nodeIndex2) {
            var line = new GameObject();
            line.AddComponent<LineRenderer>();
            line.name = nodeIndex1.ToString() + "-" + nodeIndex2.ToString();
            Vector3[] positions = new Vector3[2];
            positions[0] = pos1;
            positions[1] = pos2;
            line.GetComponent<LineRenderer>().SetPositions(positions);
            line.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
            line.GetComponent<LineRenderer>().material = lineMaterial;
        }

        public void DrawLineAllConnectedNodes() {
            for (int i = 0; i < nodesCount; i++) {
                for (int j = 0; j < nodesCount; j++) {
                    if (i == j) {
                        continue;
                    } else {
                        if (Pathfinding.Algorithms.Pathfinding.instance.g.IsEdge(i, j)) {
                            DrawLine(nodes[i].transform.position, i, nodes[j].transform.position, j);
                        }
                    }
                }
            }
        }

        private void Update() {
            nodesCount = nodes.Count;

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                var nearestNode = Geometry.GetNearestNodeToMouseClickPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), nodes);
                Debug.Log(nearestNode.transform.position);
                Debug.Log(nearestNode.name);
                var currentDestinationIndex = int.Parse(nearestNode.name.Split('-')[1]);
                Debug.Log(currentDestinationIndex);
                DijkstrasAlgorithm.dijkstra(gameGraph, 0, currentDestinationIndex, currentSolutionList);
                Debug.Log("Solution: ");
                foreach(var el in currentSolutionList) {
                    Debug.Log(el);
                }
            }
        }

        //private void OnGUI() {
        //    //GUILayout.BeginArea(new Rect(10, 10, 100, 100));
        //    if (GUILayout.Button("Add New Node")) {
        //        CreateNewNode();
        //    }
        //}

        public List<GameObject> GetNodeList() => nodes;

        public void CreateNewNode() {
            var tempObject = new GameObject();
            tempObject.name = "Node-" + nodesCount;
            tempObject.transform.SetParent(gameObject.transform);
            tempObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0f, Camera.main.pixelWidth), Random.Range(0f, Camera.main.pixelHeight), 10f));
            nodes.Add(tempObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            //Gizmos.DrawSphere(sourceNode.transform.position, 0.2f);
            if (nodes.Count > 0) {
                foreach (var el in nodes) {
                    Gizmos.DrawSphere(el.transform.position, 0.2f);
                }
            }
        }
#endif
    }
}
