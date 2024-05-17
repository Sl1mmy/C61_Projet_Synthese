using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
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
        object4d.positionW = slider.value;

        // Re-add the listener
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Update the previous value for the next iteration
        previousValue = slider.value;
    }
}
