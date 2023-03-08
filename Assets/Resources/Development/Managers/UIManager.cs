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
    private List<BasePanel> panels = new List<BasePanel>();
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public List<BasePanel> GetPanels()
    {
        return panels;
    }

    public void AddPanel(BasePanel panel)
    {
        if(!GetPanels().Contains(panel))
            GetPanels().Add(panel);
    }

    public void RemovePanel(BasePanel panel)
    {
        if (GetPanels().Contains(panel))
            GetPanels().Remove(panel);
    }
    #endregion

}
