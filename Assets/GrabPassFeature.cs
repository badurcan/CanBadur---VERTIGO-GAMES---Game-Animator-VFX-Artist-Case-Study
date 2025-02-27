using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrabPassFeature : ScriptableRendererFeature
{
    class GrabPassRenderPass : ScriptableRenderPass
    {
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        private string profilerTag = "GrabPass";

        public GrabPassRenderPass()
        {
            tempTexture.Init("_GrabPassTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            source = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.Blit(source, tempTexture.Identifier());
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    private GrabPassRenderPass grabPass;

    public override void Create()
    {
        grabPass = new GrabPassRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingTransparents };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(grabPass);
    }
}
