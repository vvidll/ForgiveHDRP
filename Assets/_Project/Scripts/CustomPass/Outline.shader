Shader "HDRP/Outline"
{
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch switch2

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    TEXTURE2D_X(OutlineBuffer);
    float4 OutlineColor;
    float Threshold;

    float4 FragFill(Varyings i) : SV_Target
    {
        return float4(1, 1, 1, 1);
    }

    float4 FullScreenPass(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);

        float2 uv = varyings.positionCS.xy * _ScreenSize.zw;

        float4 screenColor = float4(0.0, 0.0, 0.0, 0.0);
        if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
            screenColor = float4(CustomPassLoadCameraColor(varyings.positionCS.xy, 0), 1);

        float2 texelSize = _ScreenSize.zw * Threshold;
        float center = SAMPLE_TEXTURE2D_X(OutlineBuffer, s_linear_clamp_sampler, uv).r;
        float outlineMask = 0.0;

        if (center < 0.1)
        {
            float up    = SAMPLE_TEXTURE2D_X(OutlineBuffer, s_linear_clamp_sampler, uv + float2(0, texelSize.y)).r;
            float down  = SAMPLE_TEXTURE2D_X(OutlineBuffer, s_linear_clamp_sampler, uv + float2(0, -texelSize.y)).r;
            float left  = SAMPLE_TEXTURE2D_X(OutlineBuffer, s_linear_clamp_sampler, uv + float2(-texelSize.x, 0)).r;
            float right = SAMPLE_TEXTURE2D_X(OutlineBuffer, s_linear_clamp_sampler, uv + float2(texelSize.x, 0)).r;

            if (up > 0.1 || down > 0.1 || left > 0.1 || right > 0.1)
            {
                outlineMask = 1.0;
            }
        }

        float4 finalColor = lerp(screenColor, OutlineColor, outlineMask * OutlineColor.a);
        return finalColor;
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }

        Pass
        {
            Name "Silhouette"
            ZWrite Off ZTest Always Blend Off
            HLSLPROGRAM
                #pragma fragment FragFill
            ENDHLSL
        }

        Pass
        {
            Name "Custom Pass 0"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
