using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le déplacement d'un curseur à l'aide d'un slider et ajuste la position d'une caméra de raymarching en fonction de la valeur du curseur.
/// Auteur(s): Maxime
/// </summary>
public class Slider : MonoBehaviour
{
    private RaymarchCam cam;
    private float previousValue;
    public float sensitivity = 0.5f;

    public UnityEngine.UI.Slider slider;


    void Start()
    {
        cam = GetComponent<RaymarchCam>();

        previousValue = slider.value;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        
        float delta = newValue - previousValue;
        float adjustedDelta = delta * sensitivity;

        slider.onValueChanged.RemoveListener(OnSliderValueChanged);

        slider.value = previousValue + adjustedDelta;

        cam._wPosition = slider.value;

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        previousValue = slider.value;
    }
}
