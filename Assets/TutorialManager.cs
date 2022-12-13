using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public GameObject[] popUps;
    public GameObject endButton;
    private int popUpIndex = 0;
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        endButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < popUps.Length; i++)
        {
            if(i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

        if(SceneManager.GetActiveScene().name == "LVL1.0"){
            if(popUpIndex == 0){
                if(!isWaiting && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))){
                    StartCoroutine(WaitForSeconds(2.0f));
                }
            } else if(popUpIndex == 1){
                if(!isWaiting && Input.GetKeyDown(KeyCode.Space)){
                    StartCoroutine(WaitForSeconds(2.0f));
                }
            } else if(popUpIndex == 2){
                if(!isWaiting && Input.GetKeyDown(KeyCode.LeftShift)){
                    StartCoroutine(WaitForSeconds(2.0f));
                }
            } else if(popUpIndex == 3){
                if(!isWaiting && Input.GetKeyDown(KeyCode.C)){
                    StartCoroutine(WaitForSeconds(2.0f));
                }
            } else if(popUpIndex == 4){
                if(!isWaiting && Input.GetKeyDown(KeyCode.C)){
                    StartCoroutine(WaitForSeconds(2.0f));
                    endButton.SetActive(true);
                }
            }     
        } else if(SceneManager.GetActiveScene().name == "LVL1.1"){
            if(popUpIndex == 0){
                if(!isWaiting && Input.GetKeyDown(KeyCode.C)){
                    StartCoroutine(WaitForSeconds(5.0f));
                }
            } else if(popUpIndex == 1){
                if(!isWaiting && Input.GetKeyDown(KeyCode.S)){
                    StartCoroutine(WaitForSeconds(1.0f));
                }
            } else if(popUpIndex == 2){
                if(!isWaiting && Input.GetKeyDown(KeyCode.S)){
                    StartCoroutine(WaitForSeconds(1.0f));
                }
            } else if(popUpIndex == 3){
                if(!isWaiting && Input.GetKeyDown(KeyCode.C)){
                    StartCoroutine(WaitForSeconds(2.0f));
                }
            } else if(popUpIndex == 4){
                if(!isWaiting && Input.GetKeyDown(KeyCode.S)){
                    StartCoroutine(WaitForSeconds(1.0f));
                }
            } else if(popUpIndex == 5){
                if(!isWaiting && Input.GetKeyDown(KeyCode.C)){
                    StartCoroutine(WaitForSeconds(0.2f));
                }
            }
        }
    }

    IEnumerator WaitForSeconds(float time){
        isWaiting = true;
        TMP_Text popUpText = popUps[popUpIndex].GetComponent<TMP_Text>();
        popUpText.color = new Color(0f, 0.73f, 0.17f);
        yield return new WaitForSeconds(time);
        popUpIndex++;
        isWaiting = false;
    }
}
