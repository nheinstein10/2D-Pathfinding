using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pathfinding.Map {
    [CustomEditor(typeof(MapNodeManager))]
    public class MapNodeManagerEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            MapNodeManager myScript = (MapNodeManager)target;
            if(GUILayout.Button("Add New Node")) {
                myScript.CreateNewNode();
            }
        }
    }
}
