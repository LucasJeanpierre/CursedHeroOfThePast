using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelSelector : MonoBehaviour
{
    public GameObject worldSelector;
    public GameObject world1LevelsSelector;
    public GameObject world2LevelsSelector;
    public GameObject world3LevelsSelector;
    // public bool isWorldSelector = false;
    private MenuSoundEffect gameMusicPlayer;

    void Start(){
        gameMusicPlayer = MenuSoundEffect.Instance;
        showWorldsSelector();   
    }

    public void backToWorldsSelector()
    {
        gameMusicPlayer.PlaySoundButton();
        showWorldsSelector();
    }
    public void showWorldsSelector()
    {
        worldSelector.SetActive(true);
        world1LevelsSelector.SetActive(false);
        world2LevelsSelector.SetActive(false);
        world3LevelsSelector.SetActive(false);
    }

    public void selectALevel(string sceneName)
    {
        gameMusicPlayer.PlaySoundButton();
        SceneManager.LoadScene(sceneName);
    }

    public void selectWorld(int world)
    {
        gameMusicPlayer.PlaySoundInstance();
        worldSelector.SetActive(false);
        if (world == 1)
        {
            world1LevelsSelector.SetActive(true);
            world2LevelsSelector.SetActive(false);
            world3LevelsSelector.SetActive(false);
        }
        else if (world == 2)
        {
            world1LevelsSelector.SetActive(false);
            world2LevelsSelector.SetActive(true);
            world3LevelsSelector.SetActive(false);
        } else if (world == 3)
        {
            world1LevelsSelector.SetActive(false);
            world2LevelsSelector.SetActive(false);
            world3LevelsSelector.SetActive(true);
        }
    }

}
