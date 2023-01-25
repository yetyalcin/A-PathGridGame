using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
using Game.Core;
using Game.Grid;
using Game.Unit;

namespace Game.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        [SerializeField] private GlobalVariables.UnitType _type;
        [SerializeField] private float _moveTime;

        [Space(10)]

        public List<Node> Path;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Node _currentNode;
        [SerializeField] private Node _targetNode;

        private bool _isMoving;
        private int _currentNodeIndex;

        #region UnityBuildinFunctions
        private void Start()
        {
            _mover = GetComponent<Mover>();

            _currentNode = FindObjectOfType<GridSystem>().NodeFromWorldPoint(this.transform.position);
        }

        private void Update()
        {
            InteractWithMovement();
        }
        #endregion

        #region CustomMethods
        private void InteractWithMovement()
        {
            if (!_isMoving && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                bool rayCast = Physics.Raycast(GetMouseRay(), out hit, groundLayerMask);
                if (rayCast)
                {
                    _isMoving = true;
                    Path.Clear();
                    _currentNodeIndex = 0;

                    _targetNode = hit.transform.GetComponent<Node>();

                    PathFinding.Instance.FindPath(this.transform.position, _targetNode.transform.position, Path);

                    StartCoroutine(_mover.CorMoveTo(Path, _moveTime, _type));
                }
            }
        }
        
        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        public void OnMovementComplete()
        {
            _isMoving = false;
            CheckCollectableOnNode();
        }
        public void OnNextGrid()
        {
            if(_currentNodeIndex < Path.Count - 1)
                _currentNodeIndex++;

            _currentNode = Path[_currentNodeIndex];
            CheckCollectableOnNode();
            Debug.Log("doldurdu");
            Path[_currentNodeIndex].FillEvent(Path[_currentNodeIndex].GridX, Path[_currentNodeIndex].GridY, true, this._type, this.gameObject);
        }
        private void CheckCollectableOnNode()
        {
            if (_currentNode.IsFilled)
                return;

            if (_currentNode.FillType.Equals(GlobalVariables.UnitType.Coin))
            {
                Debug.Log("Coin Collected");
                _currentNode.FillObj.GetComponent<Coin>().CollectThis();
                SceneManager.Instance.CheckExpectedCoins();
            }
        }

        #endregion
    }

}
