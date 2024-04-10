using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

// shapes adapté des fonctions 3D provenant de https://iquilezles.org/articles/distfunctions/
namespace Unity.Mathematics
{
    public class DistanceFunctions : MonoBehaviour
    {

        // SPHERE
        // s : radius
        float sdSphere(float3 p, float s)
        {
            return length(p) - s;
        }
        // BOX
        // s : size of box 
        float sdBox(float3 p, float3 s)
        {
            float3 q = abs(p) - s;
            return length(max(q, 0.0f)) + min(max(q.x, max(q.y, q.z)), 0.0f);
        }

        // HYPERSPHERE
        float sdHyperSphere(float4 p, float s)
        {
            return length(p) - s;
        }

        // HYPERCUBE
        float sdHyperCube(float4 p, float4 s)
        {
            float4 q = abs(p) - s;
            return min(max(q.x, max(q.y, max(q.z, q.w))), 0.0f) + length(max(q, 0.0f));
        }

        // DUOCYLINDER
        float sdDuoCylinder(float4 p, float h, float r)
        {
            
            float2 d = abs(float2(length(p.xz), length(p.yw))) - float2(h, r);
            return min(max(d.x, d.y), 0.0f) + length(max(d, 0.0f));
        }

        // HYPERCONE
        float sdHyperCone(float4 p, float4 h)
        {
            return max(length(p.xzw) - h.x, abs(p.y) - h.y) -(h.x * p.y);
        }

        // HYPERPLANE
        //float sdHyperPlane(float4 p, float4 s) 
        //{ 
        //    return 0.0f;
        //}


        //OPERATION BOOLEENS
        public float Blend(float a, float b, float k)
        {
            float h = clamp(0.5f + 0.5f * (b - a) / k, 0.0f, 1.0f);
            return lerp(b, a, h) - (k * h * (1.0f - h));
        }

        public float Combine(float distanceA, float distanceB, shape4d.Operation operation, float blendStrength) 
        {
            float distance = distanceA;
            
            switch(operation)
            {
                case shape4d.Operation.Union:
                    if (distanceB < distanceA)
                    {
                        distance  = distanceB;
                    }
                    break;
                case shape4d.Operation.Blend:
                    distance = Blend(distanceA, distanceB, blendStrength);
                    break;
                case shape4d.Operation.Substract:
                    if (-distanceB > distance)
                    {
                        distance = -distanceB;
                    }
                    break;
                case shape4d.Operation.Intersect:
                    if(distanceB > distance)
                    {
                        distance = distanceB;
                    }
                    break;
            }
            return distance;
        }
    }

}