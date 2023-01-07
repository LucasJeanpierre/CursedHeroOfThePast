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
    public bool isWorldSelector = false;

    void Awake(){
        if(isWorldSelector){
            showWorldsSelector();
        }
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
        SceneManager.LoadScene(sceneName);
    }

    public void selectWorld(int world)
    {
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
