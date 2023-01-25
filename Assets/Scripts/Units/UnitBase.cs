using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Game.Grid;

namespace Game.Unit
{
    public abstract class UnitBase : MonoBehaviour
    {
        public GlobalVariables.UnitType Type;
        public Node CurrentNode;

        #region UnityBuildinFunctions
        private void Start()
        {
            
        }
        private void Update()
        {

        }
        #endregion

        #region CustomMethods
        protected void SetNodeFill()
        {
            CurrentNode.GetComponent<Node>().FillEvent(CurrentNode.GridX, CurrentNode.GridY, false, Type, this.gameObject);
        }
        #endregion
    }

}

