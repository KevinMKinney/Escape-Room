Shader "Unlit/WaterShader"
{
    // this thread was very helpful: https://forum.unity.com/threads/decodedepthnormal-linear01depth-lineareyedepth-explanations.608452/
    // this pdf was also helpful: https://beta.unity3d.com/talks/Siggraph2011_SpecialEffectsWithDepth_WithNotes.pdf
    Properties {
        // input data dev can manipulate
        _Size ("Size", Int) = 1
        Speed ("Speed", float) = 1

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
        ZWrite On // this makes it shader will write to depth buffer
		    Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // initialize variables
            float _Size;
            float Speed;
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

            // code from Unity's node library (https://docs.unity3d.com/Packages/com.unity.shadergraph@7.1/manual/Simple-Noise-Node.html)
            inline float unity_noise_randomValue (float2 uv) {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate (float a, float b, float t) {
                return (1.0-t)*a + (t*b);
            }

            inline float unity_valueNoise (float2 uv) {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out) {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3-0));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3-1));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3-2));
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                Out = t;
            }

            // most work should be done here (func happens over every vertex)
            Interpolator vert (meshData v) {
                Interpolator o;

                // determines how the mesh moves
                v.vertex.x = (v.vertex.x + _Time.x*Speed) % (_Size-1);

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

                //float density = tex3D(DensityTex, i.position);

                //float distToWater = i.screenPos.w; // - i.position;
                float distToWater = distance(_WorldSpaceCameraPos, i.position);
                // maybe UNITY_SAMPLE_SHADOW
                float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
                float waterViewDepth = LinearEyeDepth(i.vertex); //LinearEyeDepth(depth);

                float alphaEdge = 1 - exp(-(waterViewDepth + distToWater) * edgeFade);

                /*
                float densityMap = density * 500;
                float foamTrans = saturate(densityMap / foamSize);
                float foamAlpha = smoothstep(1, foamThresh, foamTrans);
                FoamColor.a = foamAlpha;
                */

                float3 waterCol = lerp(ShallowColor, DeepColor, alphaEdge);
                /*
                // hmm...
                float2 noiseScroll = float2(_Time.x, _Time.x);
                float foamAlpha;
                Unity_SimpleNoise_float((noiseScroll * foamSize), (i.position.y - foamThresh), foamAlpha);
                FoamColor.a = foamAlpha;
                */

                i.color = float4(waterCol, alphaEdge);

                /*
                // code for interesting shader thing
                float foamAlpha;
                Unity_SimpleNoise_float((noiseScroll * foamSize), (i.position.y - foamThresh), foamAlpha);
                FoamColor.a = foamAlpha;
                float3 col = lerp(ShallowColor, DeepColor, alphaEdge);
                i.color = float4(col, alphaEdge) + FoamColor;
                */

                return i.color;
            }

            ENDCG
        }
    }
}
