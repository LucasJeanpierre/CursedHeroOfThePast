using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Canvas))]


public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType panelType;
    private Canvas canvas;
    private bool state = false;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    private void UpdateState(){
        canvas.enabled = state;
    }

    public void ChangeState(){
        state = !state;
        UpdateState();
    }

    public void ChangeState(bool newState){
        Debug.Log("ChangeState: " + newState+ " " + panelType);
        state = newState;
        UpdateState();
    }

    #region Getter
    public PanelType GetPanelType(){
        return panelType;
    }
    #endregion
}
