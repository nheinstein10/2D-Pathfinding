using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Utility;

namespace Pathfinding.Map {
    [ExecuteInEditMode]
    public class MapNodeManager : MonoBehaviour {
        public static MapNodeManager instance;

        public List<GameObject> nodes;

        public int nodesCount;

        public GameObject sourceNode;
        [HideInInspector] public GameObject nearestNode;
        public int theNearestNodeIndex = default;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            sourceNode = nodes[0];
            //DrawLine();
        }

        private void DrawLine() {
            var line = new GameObject();
            line.AddComponent<LineRenderer>();
            line.name = "Line-0";
            Vector3[] positions = new Vector3[2];
            positions[0] = nodes[0].transform.position;
            positions[1] = nodes[1].transform.position;
            line.GetComponent<LineRenderer>().SetPositions(positions);
            line.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
        }

        private void Update() {
            nodesCount = nodes.Count;

            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                var nearestNode = Geometry.GetNearestNodeToMouseClickPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), nodes);
                Debug.Log(nearestNode.transform.position);
                Debug.Log(nearestNode.name);
            }
        }

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
