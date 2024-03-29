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

            sampler2D _MainTex;
            uniform float4x4 _CamFrustum, _CamToWorld;
            uniform float _maxDistance;
            uniform float4 _sphere1;
            uniform float3 _LightDir;

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

            float sdSphere(float3 position, float scale) 
            {
                return length(position) - scale;
            }

            float distanceField(float3 position) 
            {
                float Sphere1 = sdSphere(position - _sphere1.xyz, _sphere1.w);
                return Sphere1;
            }

            float3 getNormal(float3 position) 
            {
                const float2 offset = float2(0.001, 0.0);
                float3 n = float3(
                    distanceField(position + offset.xyy) - distanceField(position - offset.xyy),
                    distanceField(position + offset.yxy) - distanceField(position - offset.yxy),
                    distanceField(position + offset.yyx) - distanceField(position - offset.yyx));
                return normalize(n);
            }

            fixed4 raymarching(float3 rayOrigin, float3 rayDirection)
            {
                fixed4 result = fixed4(1,1,1,1);
                const int maxIterations = 164;
                float t = 0; //distance parcouru sur le rayon tracé

                for (int i = 0; i < maxIterations; i++) 
                {
                    if (t > _maxDistance) 
                    {
                        //environment
                        result = fixed4(rayDirection, 1);
                        break;
                    }

                    float3 position = rayOrigin + rayDirection * t;
                    //verif pour hit
                    float distance = distanceField(position);
                    if (distance < 0.01) //hit
                    { 
                        //shading
                        float3 n = getNormal(position);
                        float light = dot(-_LightDir, n);

                        result = fixed4(1,1,1,1) * light;
                        break;
                    }
                    t += distance;
                }

                return result;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 rayDirection = normalize(i.ray.xyz);
                float3 rayOrigin = _WorldSpaceCameraPos;

                fixed4 result = raymarching(rayOrigin, rayDirection);
                return result;
            }
            ENDCG
        }
    }
}
