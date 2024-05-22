using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Contrôle automatique du déplacement d'un curseur (Slider) de l'interface utilisateur.
/// </summary>
public class AutomaticSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider slider; // Reference to the UI Slider
    public float speed = 0.5f; // Speed of the slider movement

    private RaymarchCam cam;
    private bool movingRight = true;


    private void Start()
    {
        cam = GetComponent<RaymarchCam>();
    }
    void Update()
    {
        // Move the slider value
        if (movingRight)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue - 0.1)
                movingRight = false;
        }
        else
        {
            slider.value -= speed * Time.deltaTime;
            if (slider.value <= slider.minValue + 0.1)
                movingRight = true;
        }
        cam._wPosition = slider.value;
    }
}
