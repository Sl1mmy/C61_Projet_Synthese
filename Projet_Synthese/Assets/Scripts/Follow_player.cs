using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_player : MonoBehaviour
{
    [SerializeField]
    Vector3 camPosition = new Vector3(-0.38f, 6.48f, -4);

    public Transform player;
    private Quaternion initialRotation;

    // Update is called once per frame
    void Start()
    {
        // Stocker la rotation initiale de la caméra
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.position = player.transform.position + camPosition;
        transform.rotation = initialRotation;
    }
}
