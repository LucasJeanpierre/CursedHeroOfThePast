using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeLevel : MonoBehaviour
{
   
    public void ChangeLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
