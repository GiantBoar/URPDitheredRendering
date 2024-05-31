using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitherPass : ScriptableRenderPass
{
    // the settings made in the RenderFeature
    private DitherPassFeature.DitherSettings settings;

    // the two render textures that will be written to and from
    private RenderTargetIdentifier colorBuffer, pixelBuffer;

    // construct the render 
    public DitherPass(DitherPassFeature.DitherSettings settings)
    {
        this.settings = settings;
        renderPassEvent = settings.renderPassEvent;

        pixelBuffer = settings.renderTex;
    }

    // retrieve the colorBuffer 
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
    }

    // execute the two blits and apply the material
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();

        cmd.Blit(colorBuffer, pixelBuffer);
        cmd.Blit(pixelBuffer, colorBuffer, settings.ditherMaterial);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}