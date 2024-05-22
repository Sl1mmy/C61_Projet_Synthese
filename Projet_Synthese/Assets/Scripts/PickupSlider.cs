using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la fonctionnalité de "drag" pour un curseur UI Slider, permettant de déplacer les objets dans l'espace 4D.
/// Auteur(s): Maxime
/// </summary>
public class PickupSlider : MonoBehaviour
{
    private shape4d object4d;
    private float previousValue;
    public float sensitivity = 0.5f;

    public bool isEnabled = true;

    public UnityEngine.UI.Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        object4d = GetComponent<shape4d>();

        previousValue = slider.value;

        if (isEnabled)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void Update()
    {
        if (isEnabled)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float newValue)
    {
        float delta = newValue - previousValue;

        float adjustedDelta = delta * sensitivity;

        slider.onValueChanged.RemoveListener(OnSliderValueChanged);

        slider.value = previousValue + adjustedDelta;

        object4d.positionW = slider.value;

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        previousValue = slider.value;
    }
}
