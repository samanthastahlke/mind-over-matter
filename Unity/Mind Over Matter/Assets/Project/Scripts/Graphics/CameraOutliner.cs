using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
CameraOutliner.cs
CameraOutliner (c) Ominous Games 2017

...
*/


[RequireComponent(typeof(Camera))]
public class CameraOutliner : MonoBehaviour
{
    public int blurIterations = 2;
    public int downsample = 1;
    public int resolutionDownsample = 4;
    public int blurSize = 3;

    public Shader glowShader = null;
    public Shader occlusionShader = null;
    public Shader diffMaskShader = null;
    public Shader compositeShader = null;
    public Shader blurShader = null;

    public LayerMask silhouetteMask;
    public LayerMask silhouetteOcclusionMask;

    private Material compositeMat = null;
    private Material blurMat = null;

    public Camera cam;
    
    private void Awake()
    {
        compositeMat = new Material(compositeShader);
        blurMat = new Material(blurShader);

        cam.CopyFrom(this.GetComponent<Camera>());
        cam.depth = -10;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtW = source.width;
        int rtH = source.height;

        RenderTexture maskTex = RenderTexture.GetTemporary(rtW, rtH, 16);
        RenderTexture glowTex = RenderTexture.GetTemporary(rtW, rtH, 16);

        //Masking and glow passes.

        RenderTexture.active = glowTex;
        GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        cam.targetTexture = glowTex;
        
        cam.cullingMask = silhouetteMask;
        cam.clearFlags = CameraClearFlags.Depth;
        cam.RenderWithShader(glowShader, "");

        RenderTexture.active = null;

        Graphics.Blit(glowTex, maskTex);
        
        cam.cullingMask = silhouetteOcclusionMask;
        cam.clearFlags = CameraClearFlags.Nothing;
        cam.RenderWithShader(occlusionShader, "");
        
        cam.targetTexture = null;

        Graphics.Blit(glowTex, destination);

        //Blurring.
        
        rtW = source.width / resolutionDownsample;
        rtH = source.height / resolutionDownsample;
        
        float widthMod = 1.0f / (1.0f * (1 << downsample));
        
        blurMat.SetVector("_Parameter", new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));
        source.filterMode = FilterMode.Bilinear;
        
        //Downsample.
        RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
        
        rt.filterMode = FilterMode.Bilinear;
        Graphics.Blit(glowTex, rt, blurMat, 0);
        
        int passOffs = 0;
        
        for (int i = 0; i < blurIterations; i++)
        {
            float iterationOffs = (i * 1.0f);
            blurMat.SetVector("_Parameter", new Vector4(blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));
        
            //Vertical.
            RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, glowTex.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, blurMat, 1 + passOffs);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        
            //Horizontal.
            rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, glowTex.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, blurMat, 2 + passOffs);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }
        
        compositeMat.SetTexture("_GlowTex", rt);
        compositeMat.SetTexture("_MaskTex", maskTex);
        
        Graphics.Blit(source, destination, compositeMat);

        RenderTexture.ReleaseTemporary(rt);
        RenderTexture.ReleaseTemporary(maskTex);
        RenderTexture.ReleaseTemporary(glowTex);
    }
}


