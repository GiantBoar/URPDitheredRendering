using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DitherPassFeature : ScriptableRendererFeature
{
    // the settings for our render pass that will be passed in the constructor
    [System.Serializable]
    public struct DitherSettings
    {
        // when during the rendering process this pass will happen
        public RenderPassEvent renderPassEvent;
        // the material that the render will use
        public Material ditherMaterial;
        // the render texture of our squashed-down resolution
        public RenderTexture renderTex;
    }

    private DitherPass ditherPass;
    public DitherSettings ditherSettings;

    // creating the render pass
    public override void Create()
    {
        ditherPass = new DitherPass(ditherSettings);

        ditherPass.renderPassEvent = ditherSettings.renderPassEvent;
    }

    // adding the render pass to the renderer
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // only adding it if the material AND render texture are set up, so that there are no errors
        if (ditherSettings.ditherMaterial != null && ditherSettings.renderTex != null)
        {
            // adding our pass to the render pipeline
            renderer.EnqueuePass(ditherPass);
        }
    }
}