using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;

///INFO
///->Usage of ClickLogicHandler script: 
///ENDINFO

public class ClickLogicHandler : MonoBehaviour
{
    #region Publics
    public bool IsMouseOverUI { get { return (EventSystem.current.IsPointerOverGameObject()); } }
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public OnMouseAction OnClickWorld = new OnMouseAction();
    public OnMouseAction OnReleaseWorld = new OnMouseAction();
    public OnMouseAction OnClickUI = new OnMouseAction();
    public OnMouseAction OnReleaseUI = new OnMouseAction();
    #endregion

    #region Monobehaviours
    public void Update()
    {
        ClickControl();
    }
    #endregion

    #region Functions
    public void ClickControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickWorld(Input.mousePosition, MouseClickType.Left);
            ClickUI(Input.mousePosition, MouseClickType.Left);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseWorld(Input.mousePosition, MouseClickType.Left);
            ReleaseUI(Input.mousePosition, MouseClickType.Left);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClickWorld(Input.mousePosition, MouseClickType.Right);
            ClickUI(Input.mousePosition, MouseClickType.Right);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ReleaseWorld(Input.mousePosition, MouseClickType.Right);
            ReleaseUI(Input.mousePosition, MouseClickType.Right);
        }
    }

    #region Clicks
    public void ClickWorld(Vector2 pointerPos, MouseClickType mouseClickType)
    {
        if(IsMouseOverUI == false)
        {
            OnClickWorld.Invoke(Input.mousePosition, mouseClickType);
        }
    }

    public void ReleaseWorld(Vector2 pointerPos, MouseClickType mouseClickType)
    {
        if (IsMouseOverUI == false)
        {
            OnReleaseWorld.Invoke(Input.mousePosition, mouseClickType);
        }
    }

    public void ClickUI(Vector2 pointerPos, MouseClickType mouseClickType)
    {
        if (IsMouseOverUI == true)
        {
            OnClickUI.Invoke(Input.mousePosition, mouseClickType);
        }
    }

    public void ReleaseUI(Vector2 pointerPos, MouseClickType mouseClickType)
    {
        if (IsMouseOverUI == true)
        {
            OnReleaseUI.Invoke(Input.mousePosition, mouseClickType);
        }
    }
    #endregion
    #endregion
}
public enum MouseClickType
{
    Left,
    Right
}

public class OnMouseAction : UnityEvent<Vector2, MouseClickType> { }