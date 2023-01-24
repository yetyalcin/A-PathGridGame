using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class InputManager : MonoBehaviour
{
    public bool HasInput;

    public List<KeyCode> MoveKeys;

    #region UnityBuildinFunctions
    private void Start()
    {
	
    }
    private void Update()
    {
        foreach (var item in MoveKeys)
        {
            if (Input.GetKeyDown(item))
                HasInput = true;
            else if (Input.GetKeyUp(item))
                HasInput = false;
        }
    }
    #endregion

    #region CustomMethods
	//CustomMethods
    #endregion
}
