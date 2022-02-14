Shader "Unlit/WaterShader"
{
    Properties {
        // input data
        //_MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        //_Size ("Size", Int) = 100

    }
    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            //float _Size;

            struct meshData {
                // data per vertex (needs to be floats) automatically initialized by Unity
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                //float2 uv : TEXCOORD0;
            };

            struct Interpolator {
                float4 vertex : SV_POSITION;
                float3 normal : TEXTCORD0;
                //float4 color : TEXTCORD1;
            };

            // most work should be done here
            Interpolator vert (meshData v) {
                Interpolator o;

                //v.vertex.x = (v.vertex.x * _Time.y) % _Size;
                v.vertex.y += sin(_Time.y);

                // convert local space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            // could change float4 -> half4 for better performance/less detail
            float4 frag (Interpolator i) : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
}
