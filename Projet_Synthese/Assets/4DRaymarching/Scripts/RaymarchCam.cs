using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe gérant le rendu par raymarching dans la vue de scène Unity, 
/// en utilisant les objets shape4d pour générer la scène.
/// </summary>
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RaymarchCam : SceneViewFilter
{
    [SerializeField]
    private Shader _shader;
    List<ComputeBuffer> buffersToDispose;



    /// <summary>
    /// Propriété pour accéder ou créer un matériau de raymarch avec un shader spécifié.
    /// </summary>
    public Material _raymarchMaterial
    {
        get 
        {
            if (!_raymarchMat && _shader)
            {
                _raymarchMat = new Material(_shader);
                _raymarchMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _raymarchMat;
        }
    }

    private Material _raymarchMat;


    /// <summary>
    /// Propriété pour obtenir la caméra attachée à cet objet, la créant si nécessaire.
    /// </summary>
    public Camera _camera
    {
        get
        {
            if (!_cam)
            {
                _cam = GetComponent<Camera>();
            }
            return _cam;
        }
    }

    private Camera _cam;

    public Transform _directionalLight;

    public float _maxDistance;
    public float _maxIterations;

    [Header("4D transform")]
    public float _wPosition;
    public Vector3 _wRotation;


    [HideInInspector]
    public List<shape4d> orderedShapes = new List<shape4d>();


    /// <summary>
    /// Applique le rendu d'image en utilisant le raymarching, en configurant les paramètres nécessaires comme la direction de la lumière, les distances maximales et les itérations maximales, et en plaçant correctement les textures sur les écrans de destination.
    /// </summary>
    /// <param name="source">La texture source à utiliser pour le rendu.</param>
    /// <param name="destination">La texture de destination sur laquelle le rendu final sera appliqué.</param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        buffersToDispose = new List<ComputeBuffer>();
        CreateScene();

        if (!_raymarchMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }

        _raymarchMaterial.SetVector("_LightDir", _directionalLight ? _directionalLight.forward : Vector3.down);
        _raymarchMaterial.SetMatrix("_CamFrustum", CamFrustum(_camera));
        _raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);
        _raymarchMaterial.SetFloat("_maxDistance", _maxDistance);
        _raymarchMaterial.SetFloat("_maxIterations", _maxIterations);

        _raymarchMaterial.SetVector("_wRotation", _wRotation * Mathf.Deg2Rad);
        _raymarchMaterial.SetFloat("w", _wPosition);


        RenderTexture.active = destination;
        _raymarchMat.SetTexture("_MainTex", source);
        GL.PushMatrix();
        GL.LoadOrtho();
        _raymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);

        //BOTTOM LEFT
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);

        //BOTTOM RIGHT
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);

        //TOP RIGHT
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);

        //TOP LEFT
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();

        foreach (var buffer in buffersToDispose)
        {
            buffer.Dispose();
        }
    }

    /// <summary>
    /// Calcule et retourne la matrice de projection de la caméra basée sur ses paramètres de champ de vision et de rapport d'aspect.
    /// </summary>
    /// <param name="cam">La caméra pour laquelle la matrice de projection est calculée.</param>
    /// <returns>La matrice de projection de la caméra.</returns>
    private Matrix4x4 CamFrustum(Camera cam)
    {
        Matrix4x4 frustum = Matrix4x4.identity;
        float fov = Mathf.Tan((cam.fieldOfView * 0.5f) * Mathf.Deg2Rad);

        Vector3 goUp = Vector3.up * fov;
        Vector3 goRight = Vector3.right * fov * cam.aspect;

        //calculer les 4 coins du frustum (tronc)
        Vector3 TopLeft =   (-Vector3.forward - goRight + goUp);
        Vector3 TopRight =  (-Vector3.forward + goRight + goUp);
        Vector3 BotRight =  (-Vector3.forward + goRight - goUp);
        Vector3 BotLeft =   (-Vector3.forward - goRight - goUp);

        frustum.SetRow(0, TopLeft);
        frustum.SetRow(1, TopRight);
        frustum.SetRow(2, BotRight);
        frustum.SetRow(3, BotLeft);

        return frustum;
    }

    /// <summary>
    /// Crée la scène à partir des objets de forme4d présents dans la scène Unity, 
    /// en ordonnant les formes selon leur opération et en collectant les données nécessaires pour le rendu de raymarching.
    /// </summary>
    void CreateScene()
    {
        List<shape4d> shapes = new List<shape4d>(FindObjectsOfType<shape4d>());
        shapes.Sort((a, b) => a.operation.CompareTo(b.operation));

        orderedShapes = new List<shape4d>();

        foreach (var shape in shapes)
        {
            if (shape.transform.parent == null)
            {
                Transform parentShape = shape.transform;
                orderedShapes.Add(shape);
                shape.numChildren = parentShape.childCount;
                for (int j = 0; j < parentShape.childCount; j++)
                {
                    if(parentShape.GetChild(j).TryGetComponent(out shape4d shape4D))
                    {
                        orderedShapes.Add(shape4D);
                        orderedShapes[orderedShapes.Count - 1].numChildren = 0;
                    }
                }
            }
        }

        ShapeData[] shapeData = new ShapeData[orderedShapes.Count];
        for (int i = 0; i < orderedShapes.Count; i++)
        {
            var s = orderedShapes[i];
            Vector3 color = new Vector3(s.color.r, s.color.g, s.color.b);
            shapeData[i] = new ShapeData
            {
                position = s.Position(),
                scale = s.Scale(),
                rotation = s.Rotation(),
                rotationW = s.RotationW(),
                color = color,
                shapeType = (int)s.shapeType,
                operation = (int)s.operation,
                blendStrength = s.smoothRadius * 3,
                numChildren = s.numChildren
            };
        }

        ComputeBuffer shapeBuffer = new ComputeBuffer(shapeData.Length, ShapeData.GetSize());
        shapeBuffer.SetData(shapeData);
        _raymarchMaterial.SetBuffer("shapes", shapeBuffer);
        _raymarchMaterial.SetInt("numShapes", shapeData.Length);

        buffersToDispose.Add(shapeBuffer);
    }

    /// <summary>
    /// Structure contenant les données d'une forme pour le rendu de raymarching.
    /// </summary>
    struct ShapeData
    {
        public Vector4 position;
        public Vector4 scale;
        public Vector3 rotation;
        public Vector3 rotationW;
        public Vector3 color;
        public int shapeType;
        public int operation;
        public float blendStrength;
        public int numChildren;

        public static int GetSize()
        {
            return 84; //84 pour le size du ShapeData en bytes
        }
    }
}
