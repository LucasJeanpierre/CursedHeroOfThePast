using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicPlayer : MonoBehaviour
{

    private static GameMusicPlayer instance = null;
    public static GameMusicPlayer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


}
