Shader "Unlit/WaterShader"
{
    Properties {
        // input data
        //_MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Int) = 1
        _Color ("Color", Color) = (0, 0, 0, 1)
        //_RayDistance ("RayDistance", float) = 1

    }
    SubShader {
        // "RenderType"="Opaque"
        // "RenderType"="transparent"
        // "LightMode"="ShadowCaster"
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Size;
            float4 _Color;
            //float _RayDistance;

            sampler2D _CameraDepthTexture;

            struct meshData {
                // data per vertex (needs to be floats) automatically initialized by Unity
                float4 vertex : POSITION;
                //float3 normal : NORMAL;
                //float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator {
                float4 vertex : SV_POSITION;
                //float3 normal : TEXTCORD0;
                //float4 color : TEXTCORD1;
                float3 viewVector : TEXTCORD2;
                float2 uv : TEXCOORD3;
            };

            // most work should be done here
            Interpolator vert (meshData v) {
                Interpolator o;

                v.vertex.x = (v.vertex.x + _Time.y) % (_Size-1);
                //v.vertex.z = (v.vertex.z + _Time.y) % (_Size-1);

                // ndc = clipspace / w
                float3 vw = mul(unity_CameraInvProjection, float4(1, 1, 1, 1));
                //o.viewVector = mul(unity_CameraToWorld, float4(vw,0));

                // convert local space to clip space
                //o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.normal = UnityObjectToWorldNormal(v.normal);
                //o.color = v.color;
                o.uv = v.uv;
                COMPUTE_EYEDEPTH(o.vertex.z);
                return o;
            }

            // could change float4 -> half4 for better performance/less detail
            float4 frag (Interpolator i) : SV_Target {
                // get ray vector distance from other meshs

                //float nonlin_depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				        //float sceneDepth = LinearEyeDepth(nonlin_depth) * length(i.viewVector);
                //float alpha = 1;

                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.vertex)));
                float alpha = saturate(sceneZ-i.vertex.z);

                //return float4(i.color.x, i.color.y, i.color.z, (alpha*sceneDepth));
                return float4(_Color.x, _Color.y, _Color.z, alpha);
            }
            /*
            float distFromObj(float4 origin, float4 camVector, float maxDist) {
                float dist = 0;
                while(float i = 0; i < maxDist; i++) {

                }
            } */
            ENDCG
        }
    }
    /*
    SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert
        struct Input {
            float4 color : COLOR;
        };
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = 1;
        }
        ENDCG
    }
    Fallback "Diffuse"*/
}
