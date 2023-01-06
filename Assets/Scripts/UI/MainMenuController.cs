using System.Collections;
using System.Collections.Generic;
using Platformer.UI;
using UnityEngine;
using Platformer.Mechanics;

public enum PanelType{
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
                Debug.Log("Add Panel: " + panel.GetPanelType());
            }
        }

        OpenOnePanel(PanelType.Main);
    }

    private void OpenOnePanel(PanelType panelType){
        foreach (MenuPanel panel in panelsDict.Values){
            Debug.Log("For Panel to false: " + panel.GetPanelType()+ " False");
            panel.ChangeState(false);
        } 

        Debug.Log("Set choosen Panel to true: " + panelType+ " False");
        panelsDict[panelType].ChangeState(true);


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
