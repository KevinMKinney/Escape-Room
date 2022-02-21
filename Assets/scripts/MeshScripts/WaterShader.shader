Shader "Unlit/WaterShader"
{
    Properties {
        // input data dev can manipulate
        //_MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Int) = 1
        _Color ("Color", Color) = (0, 0, 0, 1)
        edgeFade ("edgeFade", float) = 0

    }
    SubShader {
        // shader type(s)
        Tags { "Queue"="AlphaTest" "RenderType"="transparent" }
        ZWrite On
		    Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // initialize variables
            float _Size;
            float4 _Color;
            float edgeFade;

            sampler2D _CameraDepthTexture;

            struct meshData {
                // data per vertex (needs to be floats) automatically initialized by Unity
                float4 vertex : POSITION;
                //float3 normal : NORMAL;
                //float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator {
                // data set by vertex shader for fragment shader
                float4 vertex : SV_POSITION;
                //float3 normal : TEXTCORD0;
                //float4 color : TEXTCORD1;
                float3 viewVector : TEXTCORD2;
                float2 uv : TEXCOORD3;
            };

            // most work should be done here (func happens over every vertex)
            Interpolator vert (meshData v) {
                Interpolator o;

                // determines how the mesh moves
                v.vertex.x = (v.vertex.x + _Time.y) % (_Size-1);

                // pass info to fragment shader
                o.vertex = UnityObjectToClipPos(v.vertex); // clip space = screen location
                o.uv = v.uv;
                return o;
            }

            // could change float4 -> half4 for better performance/less detail
            // function happens over every pixel
            float4 frag (Interpolator i) : SV_Target {

                float waterViewDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, ComputeScreenPos(i.vertex)));
                float alphaEdge = 1 - exp(-waterViewDepth * edgeFade);

                return float4(_Color.x, _Color.y, _Color.z, alphaEdge);
            }

            ENDCG
        }
    }
}
