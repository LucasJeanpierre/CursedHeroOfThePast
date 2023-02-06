using System.Collections;
using System.Collections.Generic;
using Platformer.UI;
using UnityEngine;
using Platformer.Mechanics;
using UnityEngine.SceneManagement;

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
    private MenuSoundEffect menuSoundEffect;
    private bool MenuSoundEffectIsInstantiated = false;


    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(true);
        
    }
    void Start()
    {
        menuSoundEffect = MenuSoundEffect.Instance;
        MenuSoundEffectIsInstantiated = menuSoundEffect != null;
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
        foreach (MenuPanel panel in panelsDict.Values){
            panel.ChangeState(false);
        } 

        panelsDict[panelType].ChangeState(true);


    }
    public void OpenPanel(PanelType panelType){
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundInstance();
        }
        OpenOnePanel(panelType);
    }

    public void GoBackInPanel(PanelType panelType){
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundButton();
        }
        
        OpenOnePanel(panelType);
    }

    public void SoundSendFeedback(){
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundButton();
        }
    }

    public void Resume(){
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundButton();
        }
        metaGameController.ToggleMainMenu(false);
    }
    public void ChangeScene(string sceneName)
    {
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundInstance();
        }
        gameController.ChangeScene(sceneName);
    }
    public void QuitGame()
    {
        gameController.QuitGame();
    }
        public void Restart()
    {
        if (MenuSoundEffectIsInstantiated) { 
            menuSoundEffect.PlaySoundInstance();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
