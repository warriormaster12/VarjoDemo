using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectMajorityColor : MonoBehaviour
{
    private string majorityColorHex = Color.black.ToHexString();
    void Awake()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer)
        {
            Material mat = renderer.materials[0];
            Texture albedo = mat.GetTexture("_MainTex");
            if (albedo && albedo.GetType() == typeof(Texture2D))
            {
                majorityColorHex = GetMajorityColor(TextureToTexture2D(albedo)).ToHexString();
            }
        }
    }

    public string GetMajorityColorHex() { return majorityColorHex; }

    private Color GetMajorityColor(Texture input)
    {
        Texture2D tex = TextureToTexture2D(input);
        Color[] pixels = tex.GetPixels();
        Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

        foreach (Color pixel in pixels)
        {
            if (colorCounts.ContainsKey(pixel))
                colorCounts[pixel]++;
            else
                colorCounts[pixel] = 1;
        }

        Color majorityColor = Color.black; // Default color if no majority color found
        int maxCount = 0;

        foreach (var kvp in colorCounts)
        {
            if (kvp.Value > maxCount)
            {
                majorityColor = kvp.Key;
                maxCount = kvp.Value;
            }
        }
        return majorityColor;
    }

    private Texture2D TextureToTexture2D(Texture tex)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        Graphics.Blit(tex, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D tex2D = new Texture2D(tex.width, tex.height);
        tex2D.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        tex2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return tex2D;
    }
}
