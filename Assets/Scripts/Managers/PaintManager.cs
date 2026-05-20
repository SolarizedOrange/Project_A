using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : MonoBehaviour
{
#region SINGLETON
    static PaintManager instance;

    public static PaintManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PaintManager>();
                if (instance == null)
                {
                    var paintManager = new GameObject()
                    {
                        name = "PaintManager"
                    };
                    instance = paintManager.AddComponent<PaintManager>();
                }
            }
            return instance;
        }
    }
#endregion
    Material paintMaterial;
    public Shader TexturePaint;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    CommandBuffer command;

    void Awake()
    {
        instance = GetComponent<PaintManager>();
        command = new()
        {
            name = "CommandBuffer - " + gameObject.name
        };

        paintMaterial = new(TexturePaint);
    }

    public void init(Paintable paintable)
    {
        var mask = paintable.GetMask();
        var extend = paintable.GetExtend();
        var support = paintable.GetSupport();
        var rend = paintable.GetRenderer();

        command.SetRenderTarget(mask);
        // command.SetRenderTarget(extend);
        // command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        // command.SetRenderTarget()
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f,  Color? color = null)
    {
        RenderTexture mask = paintable.GetMask();
        RenderTexture support = paintable.GetSupport();
        Renderer rend = paintable.GetRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetTexture(textureID, mask);
        paintMaterial.SetColor(colorID, color ?? Color.red);

        command.SetRenderTarget(support);
        command.DrawRenderer(rend, paintMaterial, 0);

        command.SetRenderTarget(mask);
        command.Blit(support, mask);
        // command.SetRenderTarget(support);
        // command.Blit(mask, support);

        // command.SetRenderTarget(extend);
        // command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }
}
