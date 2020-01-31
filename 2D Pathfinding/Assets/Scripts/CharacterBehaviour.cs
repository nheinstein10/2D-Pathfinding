using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Map;
using Pathfinding.Algorithms;

namespace Pathfinding.Gameplay {
    public class CharacterBehaviour : MonoBehaviour {
        private void Start() {
            transform.position = MapNodeManager.instance.sourceNode.transform.position;
        }
    }
}
