using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    private RaymarchCam cam;
    public UnityEngine.UI.Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<RaymarchCam>();
    }

    // Update is called once per frame
    void Update()
    {
        cam._wPosition = slider.value;
    }
}
