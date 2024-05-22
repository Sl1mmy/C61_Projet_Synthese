using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant une forme dans un espace 4D, utilisée pour le rendu par raymarching.
/// Auteur(s): Noé
/// </summary>
public class shape4d : MonoBehaviour
{
    public enum ShapeType { HyperSphere, HyperCube, DuoCylinder, HyperCone, HyperPlane };
    public enum Operation { Union, Blend, Substract, Intersect };

    [Header("Shape")]
    public ShapeType shapeType;
    public Operation operation;

    [Header("4D Transform")]
    public float positionW;
    public Vector3 rotationW;
    public float scaleW = 1.0f;

    [Header("Render")]
    public Color color;
    [Range(0.0f, 1.0f)]
    public float smoothRadius;

    [HideInInspector]
    public int numChildren;
    Vector4 parentScale = Vector4.one;

    /// <summary>
    /// Calcule et retourne la position de la forme dans l'espace 4D.
    /// </summary>
    /// <returns>La position de la forme dans l'espace 4D.</returns>
    public Vector4 Position()
    {
        Vector3 position3D = transform.position;
        return new Vector4(position3D.x, position3D.y, position3D.z, positionW);
    }

    /// <summary>
    /// Calcule et retourne la rotation de la forme dans l'espace 3D, convertie en radians.
    /// </summary>
    /// <returns>La rotation de la forme dans l'espace 3D en radians.</returns>
    public Vector3 Rotation()
    {
        return transform.eulerAngles * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Calcule et retourne la rotation de la forme autour de l'axe W dans l'espace 4D, convertie en radians.
    /// </summary>
    /// <returns>La rotation de la forme autour de l'axe W dans l'espace 4D en radians.</returns>
    public Vector3 RotationW()
    {
        return rotationW * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Calcule et retourne l'échelle de la forme dans l'espace 4D, en tenant compte de l'échelle parente le cas échéant.
    /// </summary>
    /// <returns>L'échelle de la forme dans l'espace 4D.</returns>
    public Vector4 Scale()
    {
        if (transform.parent != null && transform.parent.TryGetComponent(out shape4d shape))
        {
             parentScale = shape.Scale();
        }
        else parentScale = Vector4.one;

        Vector3 localScale3D = transform.localScale;
        return Vector4.Scale(new Vector4(localScale3D.x, localScale3D.y, localScale3D.z, scaleW), parentScale);
    }
}
