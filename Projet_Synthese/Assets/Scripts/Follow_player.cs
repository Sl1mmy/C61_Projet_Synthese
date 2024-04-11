using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_player : MonoBehaviour
{

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
        transform.position = player.transform.position + new Vector3(14.6f, 227.46f, -166.6f);
        transform.rotation = initialRotation;
    }
}
