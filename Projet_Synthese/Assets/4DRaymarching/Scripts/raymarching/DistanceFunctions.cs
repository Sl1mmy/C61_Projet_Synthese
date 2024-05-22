using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;


/// <summary>
/// Calcule les différentes fonctions.
/// Auteur(s): Noé
/// </summary>
// shapes adapté des fonctions 3D provenant de https://iquilezles.org/articles/distfunctions/
namespace Unity.Mathematics
{
    public class DistanceFunctions : MonoBehaviour
    {

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à une sphère dans l'espace 3D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance à la sphère.</param>
        /// <param name="s">Le rayon de la sphère.</param>
        /// <returns>La distance signée du point à la sphère.</returns>
        public float sdSphere(float3 p, float s)
        {
            return length(p) - s;
        }

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à une boîte (ou cube) dans l'espace 3D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance à la boîte.</param>
        /// <param name="s">Le vecteur représentant les demi-dimensions de la boîte (la moitié de la largeur, hauteur et profondeur).</param>
        /// <returns>La distance signée du point à la boîte.</returns>
        public float sdBox(float3 p, float3 s)
        {
            float3 q = abs(p) - s;
            return length(max(q, 0.0f)) + min(max(q.x, max(q.y, q.z)), 0.0f);
        }

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à une hypersphère dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance à l'hypersphère.</param>
        /// <param name="s">Le rayon de l'hypersphère.</param>
        /// <returns>La distance signée du point à l'hypersphère.</returns>
        public float sdHyperSphere(float4 p, float s)
        {
            return length(p) - s;
        }

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à un hypercube dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance à l'hypercube.</param>
        /// <param name="s">Le vecteur représentant les demi-dimensions de l'hypercube (la moitié de la largeur, hauteur, profondeur et dimension W).</param>
        /// <returns>La distance signée du point à l'hypercube.</returns>
        public float sdHyperCube(float4 p, float4 s)
        {
            float4 q = abs(p) - s;
            return min(max(q.x, max(q.y, max(q.z, q.w))), 0.0f) + length(max(q, 0.0f));
        }

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à un duo-cylindre dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance au duo-cylindre.</param>
        /// <param name="h">La hauteur du duo-cylindre (dans l'espace XZ).</param>
        /// <param name="r">Le rayon du duo-cylindre (dans l'espace YW).</param>
        /// <returns>La distance signée du point au duo-cylindre.</returns>
        public float sdDuoCylinder(float4 p, float h, float r)
        {
            
            float2 d = abs(float2(length(p.xz), length(p.yw))) - float2(h, r);
            return min(max(d.x, d.y), 0.0f) + length(max(d, 0.0f));
        }

        /// <summary>
        /// Calcule et retourne la distance signée d'un point à un hypercone dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance à l'hypercone.</param>
        /// <param name="h">Le vecteur contenant les paramètres de l'hypercone : h.x est la hauteur, h.y est le rayon à la base.</param>
        /// <returns>La distance signée du point à l'hypercone.</returns>
        public float sdHyperCone(float4 p, float4 h) //TODO fix
        {
            return max(length(p.xzw) - h.x, abs(p.y) - h.y) -(h.x * p.y);
        }

        public float sdHyperPlane(float4 p, float4 s)
        {

            float plane = dot(p, normalize(float4(0, 1, 0, 0))) - (sin(p.x * s.x + p.w) + sin(p.z * s.z) + sin((p.x * 0.34f + p.z * 0.21f) * s.w)) / s.y;
            return plane;
        }


        //------ OPERATION BOOLEENS -------------------------------------------//


        /// <summary>
        /// Applique une fonction de mélange entre deux valeurs avec une transition contrôlée par le paramètre k.
        /// </summary>
        /// <param name="a">La première Distance.</param>
        /// <param name="b">La deuxième Distance.</param>
        /// <param name="k">La force du blending.</param>
        /// <returns>La valeur résultante après mélange.</returns>
        public float Blend(float a, float b, float k)
        {
            float h = clamp(0.5f + 0.5f * (b - a) / k, 0.0f, 1.0f);
            return lerp(b, a, h) - (k * h * (1.0f - h));
        }


        /// <summary>
        /// Combine deux distances de forme selon une opération spécifiée et une force de mélange.
        /// </summary>
        /// <param name="distanceA">La première distance de forme.</param>
        /// <param name="distanceB">La deuxième distance de forme.</param>
        /// <param name="operation">L'opération de combinaison à appliquer (Union, Blend, Substract, Intersect).</param>
        /// <param name="blendStrength">La force de mélange à appliquer en cas d'opération de type Blend.</param>
        /// <returns>La distance combinée selon l'opération spécifiée.</returns>
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