using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{

    private BoxCollider2D _boxCollider2D;
    // Start is called before the first frame update

    public string nextLevel;
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToNextLevel() {
        SceneManager.LoadScene("Scenes/Levels/"+nextLevel);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("End Level");
            GoToNextLevel();
        }
    }

}
