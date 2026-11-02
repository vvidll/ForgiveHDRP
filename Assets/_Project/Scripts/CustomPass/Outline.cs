using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

class Outline : CustomPass
{
    public LayerMask outlineLayer; // Interaction layer
    public Color colorOutline = Color.red;
    public float threshold = 1;

    Shader outlineShader;
    Material fullScreenOutline;
    RTHandle outlineBuffer;

    int passIndex;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        outlineShader = Shader.Find("HDRP/Outline");
        fullScreenOutline = CoreUtils.CreateEngineMaterial(outlineShader);

        passIndex = fullScreenOutline.FindPass("Silhouette");


        outlineBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            useDynamicScale: true, name: "Outline Buffer"
            );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);
        CustomPassUtils.DrawRenderers(
        ctx,
        outlineLayer,
        overrideMaterial: fullScreenOutline,
        overrideMaterialIndex: passIndex
    );

        ctx.propertyBlock.SetColor("OutlineColor", colorOutline);
        ctx.propertyBlock.SetTexture("OutlineBuffer", outlineBuffer);
        ctx.propertyBlock.SetFloat("Threshold", threshold);

        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, fullScreenOutline, ctx.propertyBlock, shaderPassId: 1);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(fullScreenOutline);
        outlineBuffer?.Release();
    }
}