Shader "Raymarch/RaymarchShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            #include "DistanceFunc.cginc" //POUR LES SHAPES

            sampler2D _MainTex;
            uniform sampler2D _CameraDepthTexture;
            uniform float4x4 _CamFrustum, _CamToWorld;

            uniform float3 _wRotation;
            uniform float w;

            uniform float _maxIterations;
            uniform float _maxDistance;
            uniform float4 _sphere1;

            uniform int _useShadows;
            uniform float _shadowSoftness;
            uniform float _maxShadowDistance;
            uniform float _shadowIntensity;
            uniform float _aoIntensity;

            uniform float3 _lightDir;
            uniform fixed4 _skyColor;
            uniform float3 _player;



            //Structure représentant une forme dans l'espace 4D pour le rendu de raymarching.
            struct Shape 
            {
                float4 position;
                float4 scale;
                float3 rotation;
                float3 rotationW;
                float3 color;
                int shapeType;
                int operation;
                float blendStrength;
                int numChildren;
            };

            StructuredBuffer<Shape> shapes;
            int numShapes;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 ray : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                half index = v.vertex.z;
                v.vertex.z = 0;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                o.ray = _CamFrustum[(int)index].xyz;

                o.ray /= abs(o.ray.z); //normalize

                o.ray = mul(_CamToWorld, o.ray);

                return o;
            }


            // Calcule et retourne la distance signée d'un point à une forme géométrique dans l'espace 4D, en tenant compte de sa position et de sa rotation.
            // shape : La forme géométrique à évaluer.
            // position4D : La position du point dans l'espace 4D.
            // RETOURNE: La distance signée du point à la forme géométrique spécifiée.
            float GetShapeDistance(Shape shape, float4 position4D) 
            {
                position4D -= shape.position;

                position4D.xz = mul(position4D.xz, float2x2(cos(shape.rotation.y), sin(shape.rotation.y), -sin(shape.rotation.y), cos(shape.rotation.y)));
                position4D.yz = mul(position4D.yz, float2x2(cos(shape.rotation.x), -sin(shape.rotation.x), sin(shape.rotation.x), cos(shape.rotation.x)));
                position4D.xy = mul(position4D.xy, float2x2(cos(shape.rotation.z), -sin(shape.rotation.z), sin(shape.rotation.z), cos(shape.rotation.z)));

                position4D.xw = mul(position4D.xw, float2x2(cos(shape.rotationW.x), sin(shape.rotationW.x), -sin(shape.rotationW.x), cos(shape.rotationW.x)));
                position4D.zw = mul(position4D.zw, float2x2(cos(shape.rotationW.z), -sin(shape.rotationW.z), sin(shape.rotationW.z), cos(shape.rotationW.z)));
                position4D.yw = mul(position4D.yw, float2x2(cos(shape.rotationW.y), -sin(shape.rotationW.y), sin(shape.rotationW.y), cos(shape.rotationW.y)));

                switch(shape.shapeType) 
                {
                    case 0: //sphere
                        return sdHyperSphere(position4D, shape.scale.x);
                    case 1: //cube
                        return sdHyperCube(position4D, shape.scale);
                    case 2: //cylinder
                        return sdDuoCylinder(position4D, shape.scale.x, shape.scale.y);
                    case 3: //cone
                        return sdHyperCone(position4D, shape.scale);
                    //case 4:
                    //    return 0;
                    //case 5:
                    //    return 0;
                    //case 6:
                    //    return 0;
                }

                return _maxDistance;
            }


            // Calcule la distance signée d'un point à un champ de distance composé de formes géométriques dans l'espace 4D, 
            // en appliquant les transformations nécessaires et en combinant les distances de chaque forme.
            // position : La position du point dans l'espace 3D.
            // RETOURNE : La distance signée du point au champ de distance.
            float4 distanceField(float3 position) 
            {
                float4 position4D = float4 (position, w);
                if(length(_wRotation) != 0) 
                {
                    position4D.xw = mul(position4D.xw, float2x2(cos(_wRotation.x), -sin(_wRotation.x), sin(_wRotation.x), cos(_wRotation.x)));
                    position4D.yw = mul(position4D.yw, float2x2(cos(_wRotation.y), -sin(_wRotation.y), sin(_wRotation.y), cos(_wRotation.y)));
                    position4D.zw = mul(position4D.zw, float2x2(cos(_wRotation.z), -sin(_wRotation.z), sin(_wRotation.z), cos(_wRotation.z)));
                }

                float globalDistance = _maxDistance;
                float3 globalColor = 1;

                for (int i = 0; i < numShapes; i++) 
                {
                    Shape shape = shapes[i];
                    int numChildren = shape.numChildren;

                    float localDistance = GetShapeDistance(shape, position4D);
                    float3 localColor = shape.color;

                    for (int j = 0; j < numChildren; j++) 
                    {
                        Shape childShape = shapes[i + j + 1];
                        float childDistance = GetShapeDistance(childShape, position4D);

                        float4 combined = Combine(localDistance, childDistance, localColor, childShape.color, childShape.operation, childShape.blendStrength);
                        localColor = combined.xyz;
                        localDistance = combined.w;
                    }
                    i+= numChildren;
                    
                    float4 globalCombined = Combine(globalDistance, localDistance, globalColor, localColor, shape.operation, shape.blendStrength);
                    globalColor = globalCombined.xyz;
                    globalDistance = globalCombined.w;
                }

                return float4(globalDistance, globalColor);
            }


            // Calcule et retourne la normale d'un point sur une surface définie par un champ de distance, en utilisant une différence finie pour estimer la pente.
            // position : La position du point sur la surface.
            // RETOURNE : La normale de la surface au point spécifié.
            float3 getNormal(float3 p) 
            {
                float d = distanceField(p).x;
                const float2 e = float2(.01, 0);
                float3 n = d - float3(distanceField(p - e.xyy).x,distanceField(p - e.yxy).x,distanceField(p - e.yyx).x);
                return normalize(n);
            }

            // hardshadow
            float hardShadowCalc( in float3 ro, in float3 rd, float mint, float maxt) 
            {
                float res = 1.0;
                for (float t = mint; t < maxt; ) 
                {
                    float h = min(distanceField(ro + rd*t).x, sdVerticalCapsule(ro + rd*t - _player, 1, 0.4));
                    if (h < 0.001)
                        return 0.0;
                    t += h;
                }
                return res;
            }

            // softshadow
            float softShadowCalc(in float3 ro, in float3 rd, float mint, float maxt, float k) 
            {
                float res = 1.0;
                float ph = 1e20;
                for(float t = mint; t < maxt; ) 
                {
                    float h = distanceField(ro + rd*t).x;
                    if(h < 0.001)
                        return 0.0;
                    float y = h*h/(2.0 * ph);
                    float d = sqrt(h*h-y*y);
                    res = min(res, k*d/max(0.0, t-y));
                    ph = h;
                    t += h;
                }
                return res;
            }

            // Effectue le raymarching le long d'un rayon donné depuis un point d'origine dans une direction donnée jusqu'à une certaine profondeur, en évaluant les distances de chaque itération pour déterminer les collisions et en appliquant le rendu en fonction.
            // rayOrigin : L'origine du rayon.
            // rayDirection : La direction du rayon.
            // depth : La profondeur maximale à laquelle le rayon doit être suivi.
            // RETOURNE La couleur résultante après le raymarching.
            fixed4 raymarching(float3 rayOrigin, float3 rayDirection, float depth)
            {
                fixed4 result = fixed4(1,1,1,1);
                float t = 0; //distance parcouru sur le rayon tracé

                for (int i = 0; i < _maxIterations; i++) 
                {
                    if (t > _maxDistance || t >= depth) 
                    {
                        //environment
                        result = fixed4(rayDirection, 0);
                        break;
                    }

                    float3 position = rayOrigin + rayDirection * t;
                    //verif pour hit
                    float4 distance = distanceField(position);

                    if (distance.x < 0.001) //hit
                    { 
                        float3 colorDepth;
                        float light;
                        float shadow;
                        //shading

                        float3 color = distance.yzw;


                        float3 n = getNormal(position);
                        light = dot(- _lightDir, n);


                        if(_useShadows == 1) 
                        {
                            shadow = (hardShadowCalc(position, - _lightDir, 0.1, _maxShadowDistance) * (1 - _shadowIntensity) + _shadowIntensity); //hard shadow
                        }
                        else if(_useShadows == 2) 
                        {
                            shadow = (softShadowCalc(position, - _lightDir, 0.1, _maxShadowDistance, _shadowSoftness) * (1 - _shadowIntensity) + _shadowIntensity); // soft shadow
                        }
                        else 
                            shadow = 1;

                        float ao = (1 - 2 * i / float(_maxIterations)) * (1 - _aoIntensity) + _aoIntensity;

                        float3 colorLight = float3(color * light * shadow * ao);

                        colorDepth = float3(colorLight * (_maxDistance - t) / (_maxDistance) + _skyColor.rgb * (t) / (_maxDistance));

                        result = fixed4(fixed3(1,1,1) * colorLight, 1);
                        break;
                    }
                    t += distance.x;
                }

                return result;
            }


            // Fonction fragment chargée de générer la couleur du pixel en utilisant le raymarching pour le rendu de la scène.
            // i : La structure contenant les informations du vertex transformé.
            // RETOURNE La couleur résultante du pixel après le traitement du raymarching.
            fixed4 frag (v2f i) : SV_Target
            {
                float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv).r);
                depth *= length(i.ray);
                fixed3 color = tex2D(_MainTex, i.uv);

                float3 rayDirection = normalize(i.ray.xyz);
                float3 rayOrigin = _WorldSpaceCameraPos;

                fixed4 result = raymarching(rayOrigin, rayDirection, depth);
                return fixed4(color * (1.0 - result.w) + result.xyz * result.w, 1.0);
            }
            ENDCG
        }
    }
}
