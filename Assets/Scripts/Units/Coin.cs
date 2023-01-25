using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Game.Managers;

namespace Game.Unit
{
    public class Coin : UnitBase
    {
        public int CoinOrderIndex;

        #region UnityBuildinFunctions
        private void Start()
        {
            SetNodeFill();
        }
        #endregion

        #region CustomMethods
        public void CollectThis()
        {
            SceneManager.Instance.CollectedCoinByOrder.Add(CoinOrderIndex);
            SpawnManager.Instance.SpawnedTransformList.Remove(this.transform);
            Destroy(this.gameObject);
        }
        #endregion
    }
}
