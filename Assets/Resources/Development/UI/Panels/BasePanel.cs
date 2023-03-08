using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of BasePanel script: Base class for all UI Panels for usage of Inheritance and Polymorphism.
///ENDINFO

public abstract class BasePanel : MonoBehaviour
{
    #region Publics
    public GameObject Elements;
    public PanelState CurrentPanelState;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action OnPanelActivated;
    public Action OnPanelDeactivated;
    #endregion

    #region Monobehaviours
    private void Awake()
    {
        UIManager.Instance.AddPanel(this);
    }

    private void OnDestroy()
    {
        UIManager.Instance.RemovePanel(this);
    }

    protected virtual void OnEnable()
    {
        UIManager.Instance.AddPanel(this);
    }

    protected virtual void OnDisable()
    {
        UIManager.Instance.RemovePanel(this);
    }
    #endregion

    #region Functions
    public void ActivatePanel()
    {
        Elements.SetActive(true);
        CurrentPanelState = PanelState.Active;
        OnPanelActivated?.Invoke();
    }

    public void DeactivatePanel()
    {
        Elements.SetActive(false);
        CurrentPanelState = PanelState.Deactive;
        OnPanelDeactivated?.Invoke();
    }
    #endregion
}
public enum PanelState
{
    Active,
    Deactive
}