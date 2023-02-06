using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundEffect : MonoBehaviour
{
    private static MenuSoundEffect instance = null;

    public static MenuSoundEffect Instance
    {
        get { return instance; }
    }

    private AudioSource audioSource;
    public AudioClip instanceClick;
    public AudioClip buttonClick;

    void Awake()
    {
        Debug.Log("Awake SoundMenu Instant");
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
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundInstance()
    {
        audioSource.PlayOneShot(instanceClick);
        Debug.Log("PlaySoundInstance");
    }

    public void PlaySoundButton()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
