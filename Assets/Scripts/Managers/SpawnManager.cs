using System.Collections.Generic;
using UnityEngine;
using Game.Grid;
using Game.Unit;

namespace Game.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private GridSystem _gridSystem;
        public List<Transform> SpawnedTransformList;
        public bool ReSpawn;

        [SerializeField] private Transform[] _spawnObj;
        [SerializeField] private GlobalVariables.UnitType _spawnUnitType;
        private int _spawnIndex;

        public static SpawnManager Instance;

        #region UnityBuildinFunctions
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        private void Start()
        {
            _gridSystem = FindObjectOfType<GridSystem>();
            List<Transform> nodeToTransform = CreateNodeList();

            SpawnUnit(_spawnObj, 5, nodeToTransform);
        }


        #endregion

        #region CustomMethods
        private List<Transform> CreateNodeList()
        {
            List<Transform> nodeToTransform = new List<Transform>();
            foreach (var item in _gridSystem.GridList)
                nodeToTransform.Add(item.transform);
            return nodeToTransform;
        }
        private void SpawnUnit(Transform[] obj, int spawnAmount, List<Transform> spawnPoints)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                bool[] activePositions;
                int gettedPositionIndex = GetPositionIndex(spawnPoints, out gettedPositionIndex, out activePositions);

                activePositions[gettedPositionIndex] = true;

                Node node = spawnPoints[gettedPositionIndex].GetComponent<Node>();

                Transform spawnedObj = Instantiate(obj[_spawnIndex], spawnPoints[gettedPositionIndex].position, Quaternion.identity);

                spawnedObj.GetComponent<UnitBase>().CurrentNode = node;

                SpawnedTransformList.Add(spawnedObj);

                _spawnIndex++;
            }
        }
        public void ReSpawnUnit()
        {
            foreach (var item in SpawnedTransformList)
            {
                Node currentNode = item.GetComponent<UnitBase>().CurrentNode;
                currentNode.FillEvent(currentNode.GridX, currentNode.GridY, false, GlobalVariables.UnitType.Empty, null);
                Destroy(item.gameObject);
            }

            SpawnedTransformList.Clear();
            _spawnIndex = 0;
            List<Transform> nodeToTransform = CreateNodeList();
            SpawnUnit(_spawnObj, 5, nodeToTransform);

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

