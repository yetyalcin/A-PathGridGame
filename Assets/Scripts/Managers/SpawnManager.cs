using System.Collections.Generic;
using UnityEngine;
using Game.Grid;

namespace Game.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private GridSystem _gridSystem;
        public List<Transform> SpawnedTransformList;

        public Transform spawnObj;

        #region UnityBuildinFunctions
        private void Start()
        {
            _gridSystem = FindObjectOfType<GridSystem>();

            List<Transform> nodeToTransform = new List<Transform>();
            foreach (var item in _gridSystem.GridList)
                nodeToTransform.Add(item.transform);

            SpawnUnit(spawnObj, 5, nodeToTransform, GlobalVariables.UnitType.Box);
        }
        #endregion

        #region CustomMethods
        private void SpawnUnit(Transform obj, int spawnAmount, List<Transform> spawnPoints, GlobalVariables.UnitType objType)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                bool[] activePositions;
                int gettedPositionIndex = GetPositionIndex(spawnPoints, out gettedPositionIndex, out activePositions);

                activePositions[gettedPositionIndex] = true;

                Node node = spawnPoints[gettedPositionIndex].GetComponent<Node>();

                Transform spawnedObj = Instantiate(obj, spawnPoints[gettedPositionIndex].position, Quaternion.identity);

                node.FillEvent(node.GridX,node.GridY,true,GlobalVariables.UnitType.Coin,spawnedObj.gameObject);

                SpawnedTransformList.Add(spawnedObj);

            }
        }

        private int GetPositionIndex(List<Transform> spawnPoints, out int gettedPositionIndex, out bool[] activePositions)
        {
            gettedPositionIndex = Random.Range(0, spawnPoints.Count);
            activePositions = new bool[100000];
            while (activePositions[gettedPositionIndex])
            {
                gettedPositionIndex = Random.Range(0, spawnPoints.Count);
            }

            return gettedPositionIndex;
        }
        #endregion
    }
}

