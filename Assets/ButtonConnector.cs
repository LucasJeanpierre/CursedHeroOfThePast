using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConnector : MonoBehaviour
{
    public Material mat_green, mat_red;
    public LineRenderer lineRenderer;
    public Button button;
    public Door door;

    public float buttonStepDown = 1.0f;
    public float doorStepRight = 1.0f;
    public float doorStepUp = 1.0f;

    private bool _colorToBeSet = true;
    private float _alpha1 = 0.4f;
    private float _alpha2 = 0.8f;
    private float _red = 0.4f;
    private float _green = 0.4f;

    void Start()
    {
        lineRenderer.material = mat_green;
        lineRenderer.startColor = new Color(0, _green, 0, _alpha1); // Green
        lineRenderer.endColor = new Color(0, _green, 0, _alpha1);
        lineRenderer.SetPosition(0, new Vector3(button.transform.position.x, button.transform.position.y, 0));
        lineRenderer.SetPosition(1, new Vector3(button.transform.position.x, button.transform.position.y - buttonStepDown, 0));
        lineRenderer.SetPosition(2, new Vector3(door.transform.position.x + doorStepRight, button.transform.position.y - buttonStepDown, 0));
        lineRenderer.SetPosition(3, new Vector3(door.transform.position.x + doorStepRight, button.transform.position.y - buttonStepDown + doorStepUp, 0));
    }

    void Update()
    {
        if (button._isPressed && _colorToBeSet)
        {
            lineRenderer.material = mat_red;
            lineRenderer.startColor = new Color(_red, 0, 0, _alpha2); // Red
            lineRenderer.endColor = new Color(_red, 0, 0, _alpha2);
            _colorToBeSet = false;
        }
        if (!button._isPressed && !_colorToBeSet)
        {
            lineRenderer.material = mat_green;
            lineRenderer.startColor = new Color(0, _green, 0, _alpha1); // Green
            lineRenderer.endColor = new Color(0, _green, 0, _alpha1);
            _colorToBeSet = true;
        }
    }
}
