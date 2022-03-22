Shader "Unlit/WaterShader"
{
    // this thread was very helpful: https://forum.unity.com/threads/decodedepthnormal-linear01depth-lineareyedepth-explanations.608452/
    Properties {
        // input data dev can manipulate
        _Size ("Size", Int) = 1

        ShallowColor ("ShallowColor", Color) = (0, 0, 0, 1)
        DeepColor ("DeepColor", Color) = (0, 0, 0, 1)
        foamColor ("foamColor", Color) = (0, 0, 0, 1)

        edgeFade ("edgeFade", float) = 0
        foamThresh ("foamThresh", float) = 0
        foamSize ("foamSize", float) = 0

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
            float4 ShallowColor;
            float4 DeepColor;
            float4 FoamColor;
            float edgeFade;
            float foamThresh;
            float foamSize;

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
                float3 position : TEXTCORD0;
                //float3 normal : TEXTCORD0;
                float4 color : TEXTCORD1;
                //float3 viewVector : TEXTCORD2;
                float2 uv : TEXCOORD3;
                float4 screenPos : TEXTCORD4;
            };

            // most work should be done here (func happens over every vertex)
            Interpolator vert (meshData v) {
                Interpolator o;

                // determines how the mesh moves
                v.vertex.x = (v.vertex.x + _Time.y) % (_Size-1);

                // pass info to fragment shader
                o.vertex = UnityObjectToClipPos(v.vertex); // clip space = screen location
                //o.position = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
                o.position = mul(unity_ObjectToWorld, v.vertex).xyz;
                //o.color = v.color;
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(float4(o.position.xyz, 1));
                //o.screenPos = ComputeScreenPos(v.vertex);
                return o;
            }

            // could change float4 -> half4 for better performance/less detail
            // function happens over every pixel
            float4 frag (Interpolator i) : SV_Target {

                // would prob need to update this when player is added
                //float distToWater = i.screenPos.w; // - i.position;
                float distToWater = distance(_WorldSpaceCameraPos, i.position);
                // maybe UNITY_SAMPLE_SHADOW
                float waterViewDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos));

                float alphaEdge = 1 - exp(-(waterViewDepth + distToWater) * edgeFade);
                /*
                float foamCol = 0;
                if (waterViewDepth < foamThresh+sin(_Time.y)) {
                    foamCol = saturate(waterViewDepth);
                }*/

                //float foamSize = 4;

                float foamTrans = saturate(waterViewDepth / foamSize);
                float foamAlpha = smoothstep(1, foamThresh, foamTrans);
                FoamColor.a = foamAlpha;

                float3 col = lerp(ShallowColor, DeepColor, alphaEdge);
                col = col + FoamColor;
                i.color = float4(col, alphaEdge);
                return i.color;
            }

            ENDCG
        }
    }
}
