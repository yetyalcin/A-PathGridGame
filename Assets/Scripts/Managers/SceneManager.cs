using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace Game.Managers
{
    public class SceneManager : MonoBehaviour
    {
        public List<int> ExpectedCoinOrder;
        public List<int> CollectedCoinByOrder;

        private int _coinOrderCheckIndex;

        public static SceneManager Instance;

        #region UnityBuildinFunctions
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        private void Start()
        {

        }
        private void Update()
        {

        }
        #endregion

        #region CustomMethods
        public void CheckExpectedCoins()
        {
            for (int i = 0; i < CollectedCoinByOrder.Count; i++)
            {
                if (CollectedCoinByOrder[i].Equals(ExpectedCoinOrder[i]))
                    _coinOrderCheckIndex++;
                else
                {
                    CollectedCoinByOrder.Clear();
                    SpawnManager.Instance.ReSpawnUnit();
                }
            }

            if (CollectedCoinByOrder.Count.Equals(ExpectedCoinOrder.Count))
            {
                Debug.Log("Kazandýn");
            }
        }
        #endregion
    }

}