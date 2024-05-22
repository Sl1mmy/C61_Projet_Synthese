using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;


/// <summary>
/// Calcule les diff�rentes fonctions.
/// Auteur(s): No�
/// </summary>
// shapes adapt� des fonctions 3D provenant de https://iquilezles.org/articles/distfunctions/
namespace Unity.Mathematics
{
    public class DistanceFunctions : MonoBehaviour
    {

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � une sph�re dans l'espace 3D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance � la sph�re.</param>
        /// <param name="s">Le rayon de la sph�re.</param>
        /// <returns>La distance sign�e du point � la sph�re.</returns>
        public float sdSphere(float3 p, float s)
        {
            return length(p) - s;
        }

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � une bo�te (ou cube) dans l'espace 3D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance � la bo�te.</param>
        /// <param name="s">Le vecteur repr�sentant les demi-dimensions de la bo�te (la moiti� de la largeur, hauteur et profondeur).</param>
        /// <returns>La distance sign�e du point � la bo�te.</returns>
        public float sdBox(float3 p, float3 s)
        {
            float3 q = abs(p) - s;
            return length(max(q, 0.0f)) + min(max(q.x, max(q.y, q.z)), 0.0f);
        }

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � une hypersph�re dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance � l'hypersph�re.</param>
        /// <param name="s">Le rayon de l'hypersph�re.</param>
        /// <returns>La distance sign�e du point � l'hypersph�re.</returns>
        public float sdHyperSphere(float4 p, float s)
        {
            return length(p) - s;
        }

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � un hypercube dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance � l'hypercube.</param>
        /// <param name="s">Le vecteur repr�sentant les demi-dimensions de l'hypercube (la moiti� de la largeur, hauteur, profondeur et dimension W).</param>
        /// <returns>La distance sign�e du point � l'hypercube.</returns>
        public float sdHyperCube(float4 p, float4 s)
        {
            float4 q = abs(p) - s;
            return min(max(q.x, max(q.y, max(q.z, q.w))), 0.0f) + length(max(q, 0.0f));
        }

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � un duo-cylindre dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance au duo-cylindre.</param>
        /// <param name="h">La hauteur du duo-cylindre (dans l'espace XZ).</param>
        /// <param name="r">Le rayon du duo-cylindre (dans l'espace YW).</param>
        /// <returns>La distance sign�e du point au duo-cylindre.</returns>
        public float sdDuoCylinder(float4 p, float h, float r)
        {
            
            float2 d = abs(float2(length(p.xz), length(p.yw))) - float2(h, r);
            return min(max(d.x, d.y), 0.0f) + length(max(d, 0.0f));
        }

        /// <summary>
        /// Calcule et retourne la distance sign�e d'un point � un hypercone dans l'espace 4D.
        /// </summary>
        /// <param name="p">Le point dont on veut calculer la distance � l'hypercone.</param>
        /// <param name="h">Le vecteur contenant les param�tres de l'hypercone : h.x est la hauteur, h.y est le rayon � la base.</param>
        /// <returns>La distance sign�e du point � l'hypercone.</returns>
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
        /// Applique une fonction de m�lange entre deux valeurs avec une transition contr�l�e par le param�tre k.
        /// </summary>
        /// <param name="a">La premi�re Distance.</param>
        /// <param name="b">La deuxi�me Distance.</param>
        /// <param name="k">La force du blending.</param>
        /// <returns>La valeur r�sultante apr�s m�lange.</returns>
        public float Blend(float a, float b, float k)
        {
            float h = clamp(0.5f + 0.5f * (b - a) / k, 0.0f, 1.0f);
            return lerp(b, a, h) - (k * h * (1.0f - h));
        }


        /// <summary>
        /// Combine deux distances de forme selon une op�ration sp�cifi�e et une force de m�lange.
        /// </summary>
        /// <param name="distanceA">La premi�re distance de forme.</param>
        /// <param name="distanceB">La deuxi�me distance de forme.</param>
        /// <param name="operation">L'op�ration de combinaison � appliquer (Union, Blend, Substract, Intersect).</param>
        /// <param name="blendStrength">La force de m�lange � appliquer en cas d'op�ration de type Blend.</param>
        /// <returns>La distance combin�e selon l'op�ration sp�cifi�e.</returns>
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