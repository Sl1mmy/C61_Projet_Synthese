using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vaisseaufinish : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            print("Next Level");
        }
    }
}
