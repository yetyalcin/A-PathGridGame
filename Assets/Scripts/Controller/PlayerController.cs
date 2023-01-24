using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Grid;

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

                _targetNode = hit.transform.GetComponent<Node>();
                
                PathFinding.Instance.FindPath(this.transform.position, _targetNode.transform.position, Path);

                _currentNode.FillEvent(_currentNode.GridX,_currentNode.GridY,false,GlobalVariables.UnitType.Empty,null);

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
    }
    #endregion
}
