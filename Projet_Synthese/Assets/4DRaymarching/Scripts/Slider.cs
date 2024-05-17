using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    private RaymarchCam cam;
    private float previousValue;
    public float sensitivity = 0.5f;

    public UnityEngine.UI.Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<RaymarchCam>();

        previousValue = slider.value;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        // Calculate the change in value
        float delta = newValue - previousValue;

        // Adjust the value change speed
        float adjustedDelta = delta * sensitivity;

        // Remove the listener to prevent recursion
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);

        // Update the slider value
        slider.value = previousValue + adjustedDelta;

        // Set the cam _wPosition
        cam._wPosition = slider.value;

        // Re-add the listener
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Update the previous value for the next iteration
        previousValue = slider.value;
    }
}
