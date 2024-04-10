using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //4D position
    public Vector4 Position()
    {
        Vector3 position3D = transform.position;
        return new Vector4(position3D.x, position3D.y, position3D.z, positionW);
    }

    //3D rotation
    public Vector3 Rotation()
    {
        return transform.eulerAngles * Mathf.Deg2Rad;
    }

    //Rotation axe W
    public Vector3 RotationW()
    {
        return rotationW * Mathf.Deg2Rad;
    }

    //4D scale
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
