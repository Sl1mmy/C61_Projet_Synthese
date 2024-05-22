using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la réapparition de l'objet lorsque sa position descend en dessous d'un certain seuil.
/// </summary>
public class Respawn : MonoBehaviour
{
    [SerializeField]
    public float threshold;
    void FixedUpdate()
    {
        if(transform.position.y < threshold)
        {
            transform.position = new Vector3(703.4f, 557.84f, -144);
        }
        
    }
}
