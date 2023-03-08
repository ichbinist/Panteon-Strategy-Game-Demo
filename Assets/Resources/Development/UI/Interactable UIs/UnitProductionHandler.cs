using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of UnitProductionHandler script: 
///ENDINFO

public class UnitProductionHandler : MonoBehaviour
{
    #region Publics
    public GameObject Elements;
    public InteractableState CurrentInteractableState;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action OnInteractableActivated;
    public Action OnInteractableDeactivated;
    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public void ActivatePanel()
    {
        Elements.SetActive(true);
        CurrentInteractableState = InteractableState.Active;
        OnInteractableActivated?.Invoke();
    }

    public void DeactivatePanel()
    {
        Elements.SetActive(false);
        CurrentInteractableState = InteractableState.Deactive;
        OnInteractableDeactivated?.Invoke();
    }
    #endregion

}
public enum InteractableState
{
    Active,
    Deactive
}