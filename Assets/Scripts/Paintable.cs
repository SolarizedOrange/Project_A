using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Paintable : MonoBehaviour
{
    const int TEXTURE_SIZE = 1024;

    RenderTexture extendIsland;
    RenderTexture mask;
    RenderTexture support;
    Renderer rend;
    
    int maskID = Shader.PropertyToID("_Mask");

    public RenderTexture GetMask() => mask;
    public RenderTexture GetExtend() => extendIsland;
    public RenderTexture GetSupport() => support;
    public Renderer GetRenderer() => rend;

    void Start()
    {
        extendIsland = new(TEXTURE_SIZE, TEXTURE_SIZE, 0)
        {
            filterMode = FilterMode.Bilinear
        };

        mask = new(TEXTURE_SIZE, TEXTURE_SIZE, 0)
        {
            filterMode = FilterMode.Bilinear
        };

        support = new(TEXTURE_SIZE, TEXTURE_SIZE, 0)
        {
            filterMode = FilterMode.Bilinear
        };

        rend = GetComponent<Renderer>();
        PaintManager.Instance.init(this);
        rend.material.SetTexture("_BaseMap", mask);
    }
}
