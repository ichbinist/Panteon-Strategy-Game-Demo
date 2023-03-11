using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of BaseInteractable script: Base script for Interactable UIs' (BuildingProductionHandler and UnitProductionHandler). Contains Interactable UIs' General Functions like Activation and Deactivation.
///ENDINFO

public class BaseInteractable : MonoBehaviour
{
    #region Publics
    [FoldoutGroup("Interactable References")]
    public GameObject Elements;
    [FoldoutGroup("Interactable Settings")]
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