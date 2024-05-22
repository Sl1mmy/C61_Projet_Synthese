using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrôle la rotation de la caméra autour d'un point cible.
/// </summary>
public class CamRotate : MonoBehaviour
{
    public Transform target; // The point to rotate around
    public float distance = 10.0f; // Distance from the target
    public float speed = 5.0f; // Speed of rotation

    private float angle = 0.0f;

    void Update()
    {
        // Update the angle
        angle += speed * Time.deltaTime;

        // Calculate the new position
        float x = target.position.x + distance * Mathf.Cos(angle);
        float z = target.position.z + distance * Mathf.Sin(angle);

        // Set the camera's position
        transform.position = new Vector3(x, transform.position.y, z);

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
