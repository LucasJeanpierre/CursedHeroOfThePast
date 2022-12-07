using System.Collections;
using System.Collections.Generic;
using Platformer.UI;
using UnityEngine;
using Platformer.Mechanics;

public enum PanelType{
    None,
    Main,
    Options,
    Credits,
    Feedback
}

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private List<MenuPanel> panelsList = new List<MenuPanel>();

    private Dictionary<PanelType, MenuPanel> panelsDict = new Dictionary<PanelType, MenuPanel>();

    [SerializeField] private MetaGameController metaGameController;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;

        foreach (MenuPanel panel in panelsList)
        {
            if(panel){
                panelsDict.Add(panel.GetPanelType(), panel);
            }
        }

        OpenOnePanel(PanelType.Main);
    }

    private void OpenOnePanel(PanelType panelType){

        foreach (MenuPanel panel in panelsDict.Values) panel.ChangeState(false);
        
        if(panelType != PanelType.None){
            panelsDict[panelType].ChangeState(true);
        }

    }
    public void OpenPanel(PanelType panelType){
        OpenOnePanel(panelType);
    }

    public void Resume(){
        metaGameController.ToggleMainMenu(false);
    }
    public void ChangeScene(string sceneName)
    {
        gameController.ChangeScene(sceneName);
    }
    public void QuitGame()
    {
        gameController.QuitGame();
    }

}
