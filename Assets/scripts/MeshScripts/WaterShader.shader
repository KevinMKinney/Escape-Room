Shader "Unlit/WaterShader"
{
    Properties {
        // input data
        //_MainTex ("Texture", 2D) = "white" {}

    }
    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct meshData {
                // data per vertex (needs to be floats) automatically initialized by Unity
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float4 color : COLOR;
                //float2 uv : TEXCOORD0;
            };

            struct Interpolator {
                float4 vertex : SV_POSITION;
            };

            Interpolator vert (meshData v) {
                Interpolator o;
                // convert local space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // could change float4 -> half4 for better performance/less detail
            float4 frag (Interpolator i) : SV_Target {
                return float4(.25, .25, 1, 1);
            }
            ENDCG
        }
    }
}
