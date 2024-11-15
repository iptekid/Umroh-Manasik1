Shader "Custom/Sphere360VR"
{
    Properties
    {
        _MainTex("360 Texture", 2D) = "white" {}
        _Rotation("Rotation", Range(0, 360)) = 0
        _Opacity("Opacity", Range(0, 1)) = 1.0  // Added opacity property
    }
        SubShader
        {
            Tags
            {
                "RenderType" = "Transparent"  // Changed to Transparent
                "RenderPipeline" = "UniversalPipeline"
                "Queue" = "Transparent"       // Changed to Transparent queue
            }
            Pass
            {
                Name "ForwardLit"
                Tags { "LightMode" = "UniversalForward" }

                Blend SrcAlpha OneMinusSrcAlpha  // Enable alpha blending
                Cull Front
                ZWrite On
                ZTest LEqual

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 viewDir : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                CBUFFER_START(UnityPerMaterial)
                    float4 _MainTex_ST;
                    float _Rotation;
                    float _Opacity;  // Added opacity variable
                CBUFFER_END

                float DegreesToRadians(float degrees)
                {
                    return degrees * (PI / 180.0);
                }

                Varyings vert(Attributes input)
                {
                    Varyings output = (Varyings)0;

                    UNITY_SETUP_INSTANCE_ID(input);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                    output.viewDir = normalize(input.positionOS.xyz);

                    float angle = DegreesToRadians(_Rotation);
                    float sinAngle, cosAngle;
                    sincos(angle, sinAngle, cosAngle);

                    float2x2 rotationMatrix = float2x2(cosAngle, -sinAngle,
                                                     sinAngle, cosAngle);
                    output.viewDir.xz = mul(rotationMatrix, output.viewDir.xz);

                    return output;
                }

                half4 frag(Varyings input) : SV_Target
                {
                    float3 viewDir = normalize(input.viewDir);

                    float2 sphereCoords;
                    sphereCoords.x = atan2(viewDir.x, viewDir.z) / (2.0 * PI) + 0.5;
                    sphereCoords.y = acos(viewDir.y) / PI;

                    float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sphereCoords);
                    color.a *= _Opacity;  // Apply opacity to alpha channel
                    return color;
                }
                ENDHLSL
            }
        }
}