using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelSelector : MonoBehaviour
{
    public GameObject world1Grid;
    public GameObject world2Grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void selectWorld(int world)
    {

        if (world == 1)
        {
            world1Grid.SetActive(true);
            world2Grid.SetActive(false);
        }
        else if (world == 2)
        {
            world1Grid.SetActive(false);
            world2Grid.SetActive(true);
        }
    }

}
