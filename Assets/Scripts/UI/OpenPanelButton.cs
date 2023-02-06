using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType panelType;

    private MainMenuController mainMenuController;
    // public GameMusicPlayer gameMusicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public void OnClick(){
        mainMenuController.OpenPanel(panelType);
    }

    public void OnClickBack(){
        mainMenuController.GoBackInPanel(panelType);
    }
}
