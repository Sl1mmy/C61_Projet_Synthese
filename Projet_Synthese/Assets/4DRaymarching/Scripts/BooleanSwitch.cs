using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanSwitch : MonoBehaviour
{
    public shape4d.Operation newOperation;
    public GameObject targetShape;

    private void OnCollisionEnter(Collision collision)
    {
        print("picked");
        if (collision.gameObject.CompareTag("Player"))
        {
            shape4d shapeScript = targetShape.GetComponent<shape4d>();
            shapeScript.operation = newOperation;
            Destroy(gameObject);
        }
    }
}
