using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System.Linq;
using Game.Grid;

namespace Game.Core
{
    public class PathFinding : MonoBehaviour
    {
        private GridSystem _grid;

        [SerializeField] private bool CanMoveDiagonal;

        public static PathFinding Instance;

        #region UnityBuildinFunctions
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        private void Start()
        {
            _grid = GetComponent<GridSystem>();
        }
        #endregion

        #region CustomMethods
        public void FindPath(Vector3 startPos, Vector3 targetPos, List<Node> movePath)
        {
            Node startNode = _grid.NodeFromWorldPoint(startPos);
            Node targetNode = _grid.NodeFromWorldPoint(targetPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode, movePath);
                    return;
                }

                if (!CanMoveDiagonal)
                {
                    LoopNeighbors(_grid.GetNeighboorsStraith(currentNode), targetNode, openSet, closedSet, currentNode);
                }
                else
                {
                    LoopNeighbors(_grid.GetNeighborsDiagonal(currentNode), targetNode, openSet, closedSet, currentNode);
                }

            }
        }

        private void LoopNeighbors(List<Node> neighborDirectionList, Node targetNode, List<Node> openSet, HashSet<Node> closedSet, Node currentNode)
        {
            foreach (Node neighbor in neighborDirectionList)
            {
                if (neighbor.IsFilled || closedSet.Contains(neighbor)) 
                    continue;

                int newMoveCostToNeightbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMoveCostToNeightbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMoveCostToNeightbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int distanceY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);

            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

        private void RetracePath(Node startNode, Node endNode, List<Node> movePath)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                movePath.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            movePath.Reverse();
        }
        #endregion
    }

}