using UnityEngine;
using System.IO;
public class CanvasOps : MonoBehaviour
{
    Texture2D texture;
    float[,,] cmy = new float[400, 300, 3];
    float[,] wetness = new float[400, 300];
    System.Random r;

    void Start()
    {
        texture = LoadPNG(Application.dataPath + "/misc_materials/base.png");
        //convert texture to cmy completely dry.
        for (int x = 0; x != texture.width; x++)
        {
            for (int y = 0; y != texture.height; y++)
            {
                Color rgbpx = texture.GetPixel(x, y);
                float cy = (1 - rgbpx.r);
                float ma = (1 - rgbpx.g);
                float ye = (1 - rgbpx.b);
                cmy[x, y, 0] = cy;
                cmy[x, y, 1] = ma;
                cmy[x, y, 2] = ye;
                wetness[x, y] = 0;
            }
        }
        GetComponent<Renderer>().material.mainTexture = texture;
        r = new System.Random();
    }

    void Update()
    {
        //convert cmy to rgb
        //apply paint to these pixels
        for (int x = 0; x != texture.width; x++)
        {
            for (int y = 0; y != texture.height; y++)
            {
                float r = (1f - cmy[x, y ,0]);
                float g = (1f - cmy[x, y, 1]);
                float b = (1f - cmy[x, y, 2]);
                texture.SetPixel(x,y, new Color(r, g, b, 0));
            }
        }
        texture.Apply();
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public void applyBrush(float x, float y, int brush_size, float cy, float ma, float ye, float str)
    {
        /**
        // Transform on Y axis
        y = -y;
        //change x to reference top right corner
        x = x + 1.5F;
        y = y + 2.0F;
        //Debug.Log(x + "," + y);
        //scale x, y from 0.3 to 0.4: MULTIPLY BY 100
        if (x < 0.0 || y < 0.0 || x >= 3 || y >= 4)
            return;

        //apply paint to these pixels
        for (int dx = -bra; dx != 4; dx++)
        {
            for (int dy = -3; dy != 4; dy++)
            {
                texture.SetPixel(400 - (dx + (int)(y * 100F)), 300 - (dy + (int)(x * 100F)), new Color(1F, 0, 0, 0));
            }
        }

    */
    }
}