using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = "Level " + SceneManager.GetActiveScene().name.Substring(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
