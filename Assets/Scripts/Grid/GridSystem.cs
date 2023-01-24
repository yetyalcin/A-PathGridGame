using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System;

namespace Game.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private Transform _gridTransform;
        [SerializeField] private Transform _gridParent;

        public Transform Player;

        public Vector2 GridWorldSize;
        public float NodeRadius;

        public List<Node> GridList = new List<Node>();

        private Node[,] _grid;

        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;

        public event Action<int, int, bool, GlobalVariables.UnitType, GameObject> IsFilled;

        #region UnityBuildinFunctions
        private void Awake()
        {
            _nodeDiameter = NodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
            _grid = new Node[_gridSizeX, _gridSizeY];

            CreateGrid();
        }
        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireCube(transform.position, new Vector2(GridWorldSize.x, GridWorldSize.y));
            //
            //Node playerNode = NodeFromWorldPoint(Player.position);
            //
            //foreach (var item in _grid)
            //{
            //    Gizmos.DrawCube(item.WorldPosition, Vector3.one * (_nodeDiameter - .1f));
            //
            //    if (playerNode == item)
            //        Gizmos.color = Color.green;
            //    else
            //        Gizmos.color = Color.white;
            //}
        }
        #endregion

        #region CustomMethods
        private void CreateGrid()
        {
            Vector3 worldBotLeft = this.transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBotLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.up * (y * _nodeDiameter + NodeRadius);

                    Node spawnedNode = SpawnTile(x, y, worldPoint);
                    _grid[x, y] = spawnedNode;
                }
            }
        }

        private Node SpawnTile(int x, int y, Vector3 pos)
        {
            GameObject obj = Instantiate(_gridTransform.gameObject);
            obj.name = "[" + x + "]" + "[" + y + "]";
            Node node = obj.AddComponent<Node>();
            obj.transform.position = pos;
            node.GridX = x;
            node.GridY = y;

            obj.transform.parent = _gridParent;
            GridList.Add(node);
            return node;
        }

        public Node NodeFromWorldPoint(Vector3 worldPos)
        {
            float percentX = (worldPos.x + GridWorldSize.x / 2) / GridWorldSize.x;
            float percentY = (worldPos.y + GridWorldSize.y / 2) / GridWorldSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

            return _grid[x, y];
        }

        public List<Node> GetNeighborsDiagonal(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int xCheck = node.GridX + x;
                    int yCheck = node.GridY + y;

                    if (xCheck >= 0 && xCheck < _gridSizeX && yCheck >= 0 && yCheck < _gridSizeY)
                        neighbors.Add(_grid[xCheck, yCheck]);
                }
            }

            return neighbors;
        }

        public List<Node> GetNeighboorsStraith(Node node)
        {
            List<Node> neighbors = new List<Node>();
            int xCheck;
            int yCheck;

            //right face
            xCheck = node.GridX + 1;
            yCheck = node.GridY;
            SetNewMembersToList(neighbors, xCheck, yCheck);

            //left face
            xCheck = node.GridX - 1;
            yCheck = node.GridY;
            SetNewMembersToList(neighbors, xCheck, yCheck);

            //top face
            xCheck = node.GridX;
            yCheck = node.GridY + 1;
            SetNewMembersToList(neighbors, xCheck, yCheck);

            //bot face
            xCheck = node.GridX;
            yCheck = node.GridY - 1;
            SetNewMembersToList(neighbors, xCheck, yCheck);

            return neighbors;
        }

        private void SetNewMembersToList(List<Node> neighbors, int xCheck, int yCheck)
        {
            if (xCheck >= 0 && xCheck < _gridSizeX)
            {
                if (yCheck >= 0 && yCheck < _gridSizeY)
                {
                    neighbors.Add(_grid[xCheck, yCheck]);
                }
            }
        }
        #endregion
    }

}