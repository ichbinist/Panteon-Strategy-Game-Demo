using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of WorldObjectMouseFollow script: 
///ENDINFO

public class WorldObjectMouseFollow : MonoBehaviour
{
    #region Publics

    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward;
    }
    #endregion

    #region Functions

    #endregion

}
