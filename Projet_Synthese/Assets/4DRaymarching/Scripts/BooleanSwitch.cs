using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de changer une opération booléenne d'une forme 4D lorsqu'un joueur entre en collision avec l'objet.
/// Auteur(s): Noé
/// </summary>
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
