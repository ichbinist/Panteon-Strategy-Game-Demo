using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of UIManager script: Like MVC(Model-View-Controller), UIManager handles the communication between Logic and UI.
///ENDINFO

public class UIManager : Singleton<UIManager>
{
    #region Publics
    [ShowInInspector]
    [HideLabel]
    [FoldoutGroup("Cached Panels")]
    public List<BasePanel> GetPanels { get { return (panels); } }
    #endregion

    #region Privates
    private List<BasePanel> panels = new List<BasePanel>();
    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public void AddPanel(BasePanel panel)
    {
        if(!GetPanels.Contains(panel))
            panels.Add(panel);
    }

    public void RemovePanel(BasePanel panel)
    {
        if (GetPanels.Contains(panel))
            panels.Remove(panel);
    }
    #endregion

}
