using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    [SerializeField] private float Time = 0.0f;

    [SerializeField] private List<GameObject> _ListOfRewindObjects;
    public Text TimeText;

    // Update is called once per frame
    void Update()
    {
        //Display the current time on the UI
        TimeText.text = "Time : " + Time.ToString();

        //Change the time manually
        #if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time -= 1.0f;
        }
        #endif
    }

    public void RewindAllAffectedObjects(float time)
    {
        foreach (var rewindObject in _ListOfRewindObjects)
        {
            rewindObject.GetComponent<RewindSaveInfo>().RewindTo(Time);
        }
    }

    /**
     * This is the function that allow all the other objects of the game the get the current time
     * The time variable is private to prevent other object from modifying it
     * */
    public float GetTime()
    {
        return Time;
    }
}
