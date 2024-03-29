using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    public float threshold;
    void FixedUpdate()
    {
        if(transform.position.y < threshold)
        {
            transform.position = new Vector3(726, 557.84f, 55);
        }
        
    }
}
