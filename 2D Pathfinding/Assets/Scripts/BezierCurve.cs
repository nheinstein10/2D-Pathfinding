using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Bezier {
    public class BezierCurve : MonoBehaviour {
        public static BezierCurve instance;

        #region Fields
        public Transform[] bezierPoints;

        public GameObject pathPointsParent;
        public int numberOfPathPoints;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake() {
            instance = this;
        }

        private void Start() {
            
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            for(float t = 0f; t <= 1f; t += 1f / numberOfPathPoints) {
                Vector2 currentBezierPoint = (1 - t * t * t) * bezierPoints[0].position + 3 * (1 - t) * (1 - t) * t * bezierPoints[1].position + 3 * (1 - t) * t * t * bezierPoints[2].position + t * t * t * bezierPoints[3].position;
                Gizmos.DrawSphere(currentBezierPoint, 0.2f);
            }
        }
        #endregion
    }
}
