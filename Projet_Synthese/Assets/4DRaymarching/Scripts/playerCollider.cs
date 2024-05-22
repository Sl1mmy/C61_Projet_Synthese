using System.Collections;
using System.Collections.Generic;
using static Unity.Mathematics.math;
using UnityEngine;

namespace Unity.Mathematics
{

    /// <summary>
    /// Gère la détection de collision d'un joueur dans un environnement en utilisant un champ de distance et la marche de rayons.
    /// </summary>
    public class playerCollider : MonoBehaviour
    {

        public float colliderOffset = 1.2f;
        public float maxDownMovement = 1f;

        public Transform[] rayMarchTransforms;

        private DistanceFunctions Df;
        private RaymarchCam camScript;


        void Start()
        {
            camScript = Camera.main.GetComponent<RaymarchCam>();
            Df = GetComponent<DistanceFunctions>();
        }

        void Update()
        {
            MoveToGround();
            RayMarch(rayMarchTransforms);
        }

        /// <summary>
        /// Calcule la distance entre un point dans un espace 4D et une forme spécifiée.
        /// </summary>
        /// <param name="shape">La forme 4D pour laquelle mesurer la distance.</param>
        /// <param name="p4D">Le point dans l'espace 4D à partir duquel la distance est mesurée.</param>
        /// <returns>La distance entre le point et la forme.</returns>
        public float GetShapeDistance(shape4d shape, float4 p4D)
        {
            p4D -= (float4)shape.Position();

            Vector3 shapeRotation = shape.Rotation();
            p4D.xz = mul(p4D.xz, math.float2x2(cos(shapeRotation.y), sin(shapeRotation.y), -sin(shapeRotation.y), cos(shapeRotation.y)));
            p4D.yz = mul(p4D.yz, math.float2x2(cos(shapeRotation.x), -sin(shapeRotation.x), sin(shapeRotation.x), cos(shapeRotation.x)));
            p4D.xy = mul(p4D.xy, math.float2x2(cos(shapeRotation.z), -sin(shapeRotation.z), sin(shapeRotation.z), cos(shapeRotation.z)));

            Vector3 shapeRotationW = shape.RotationW();
            p4D.xw = mul(p4D.xw, math.float2x2(cos(shapeRotationW.x), sin(shapeRotationW.x), -sin(shapeRotationW.x), cos(shapeRotationW.x)));
            p4D.zw = mul(p4D.zw, math.float2x2(cos(shapeRotationW.z), -sin(shapeRotationW.z), sin(shapeRotationW.z), cos(shapeRotationW.z)));
            p4D.yw = mul(p4D.yw, math.float2x2(cos(shapeRotationW.y), -sin(shapeRotationW.y), sin(shapeRotationW.y), cos(shapeRotationW.y)));


            switch (shape.shapeType)
            {
                case shape4d.ShapeType.HyperCube:
                    return Df.sdHyperCube(p4D, shape.Scale());

                case shape4d.ShapeType.HyperSphere:
                    return Df.sdHyperSphere(p4D, shape.Scale().x);

                case shape4d.ShapeType.DuoCylinder:
                    return Df.sdDuoCylinder(p4D, ((float4)shape.Scale()).x, ((float4)shape.Scale()).y);
                case shape4d.ShapeType.HyperCone:
                    return Df.sdHyperCone(p4D, shape.Scale());

            }

            return Camera.main.farClipPlane;
        }

        /// <summary>
        /// Calcule la distance d'un point à partir du champ de distance.
        /// </summary>
        /// <param name="position">La position du point dans l'espace 3D.</param>
        /// <returns>La distance du point au champ de distance.</returns>
        public float DistanceField(float3 position)
        {
            float4 p4D = float4(position, camScript._wPosition);
            Vector3 wRot = camScript._wRotation * Mathf.Deg2Rad;

            if ((wRot).magnitude != 0)
            {
                p4D.xw = mul(p4D.xw, float2x2(cos(wRot.x), -sin(wRot.x), sin(wRot.x), cos(wRot.x)));
                p4D.yw = mul(p4D.yw, float2x2(cos(wRot.y), -sin(wRot.y), sin(wRot.y), cos(wRot.y)));
                p4D.zw = mul(p4D.zw, float2x2(cos(wRot.z), -sin(wRot.z), sin(wRot.z), cos(wRot.z)));

            }

            float globalDst = Camera.main.farClipPlane;

            for (int i = 0; i < camScript.orderedShapes.Count; i++)
            {
                shape4d shape = camScript.orderedShapes[i];
                int numChildren = shape.numChildren;

                float localDst = GetShapeDistance(shape, p4D);


                for (int j = 0; j < numChildren; j++)
                {
                    shape4d childShape = camScript.orderedShapes[i + j + 1];
                    float childDst = GetShapeDistance(childShape, p4D);

                    localDst = Df.Combine(localDst, childDst, childShape.operation, childShape.smoothRadius);

                }
                i += numChildren; // skip les enfants dans la loop exterieure

                globalDst = Df.Combine(globalDst, localDst, shape.operation, shape.smoothRadius);
            }

            return globalDst;
        }

        /// <summary>
        /// Effectue une marche de rayon pour détecter les collisions avec les objets spécifiés.
        /// </summary>
        /// <param name="ro">Un tableau de transformations représentant les positions à vérifier pour les collisions.</param>
        void RayMarch(Transform[] ro)
        {

            int nrHits = 0;

            for (int i = 0; i < ro.Length; i++)
            {
                Vector3 p = ro[i].position;
                //check collision
                float d = DistanceField(p);

                if (d < 0) //collision
                {
                    nrHits++;
                    //collision
                    print(ro[i].name);
                    transform.Translate(ro[i].forward * d * 1.5f, Space.World);
                }
            }
        }

        /// <summary>
        /// Déplace l'objet vers le sol en détectant les collisions.
        /// </summary>
        void MoveToGround()
        {
            Vector3 p = transform.position;
            //check collision

            float d = DistanceField(p);
            d = Mathf.Min(d, maxDownMovement);
            Debug.Log(d);
            transform.Translate(Vector3.down * d, Space.World);
        }
    }
}