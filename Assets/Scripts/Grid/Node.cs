using UnityEngine;

namespace Game.Grid
{
    public class Node : MonoBehaviour
    {
        [HideInInspector] public Node Parent;
        public bool IsFilled;
        [HideInInspector] public Vector3 WorldPosition;

        public int GridX;
        public int GridY;

        [HideInInspector] public int gCost;
        [HideInInspector] public int hCost;
        [HideInInspector] public int fCost { get { return gCost + hCost; } }

        private GlobalVariables.UnitType _fillType;
        private GameObject _fillObj;


        private void Start()
        {
            FindObjectOfType<GridSystem>().IsFilled += FillEvent;
        }

        public void FillEvent(int x, int y,bool isFilled, GlobalVariables.UnitType type, GameObject obj)
        {
            Debug.Log(IsFilled + this.transform.name);
            IsFilled = isFilled;
            _fillType = type;
            _fillObj = obj;
        }
    }
}