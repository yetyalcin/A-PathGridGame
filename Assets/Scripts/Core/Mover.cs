using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Grid;
using UnityEngine.Events;

namespace Game.Core
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        public UnityEvent MovementComplete;
        public UnityEvent NextGrid;

        private bool _isStopped;

        #region UnityBuildinFunctions
        
        #endregion

        #region CustomMethods
        public IEnumerator CorMoveTo(List<Node> Path, float timeToMove, GlobalVariables.UnitType type)
        {
            Vector3 initialScale = transform.localScale;
            float t = 0;

             for (int i = 0; i < Path.Count; i++)
             {
                 Vector3 targetPos = Path[i].transform.position;
                 while (t < 1)
                 {
                     transform.position = Vector3.Lerp(transform.position, targetPos, _curve.Evaluate(t));
                     t = t + Time.deltaTime / timeToMove;
                     ScaleUpDownTween(initialScale, t);

                     yield return new WaitForEndOfFrame();
                 }
                 transform.position = targetPos;

                 if (i > 0)
                     Path[i - 1].FillEvent(Path[i - 1].GridX, Path[i - 1].GridY, false, GlobalVariables.UnitType.Empty, null);

                 NextGrid?.Invoke();
                 t = 0;
             }

             MovementComplete?.Invoke();
        }

        private void ScaleUpDownTween(Vector3 initialScale, float t)
        {
            if (t <= 0.25f)
                transform.localScale = Vector3.Lerp(initialScale, initialScale * 1.5f, _curve.Evaluate(t));
            else
                transform.localScale = Vector3.Lerp(initialScale, initialScale * 1.5f, _curve.Evaluate(Mathf.Abs(1 - t)));
        }

        #endregion
    }

}