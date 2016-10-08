using UnityEngine;
using System.IO;
public class CanvasOps : MonoBehaviour
{
    Texture2D texture;
    System.Random r;

    void Start()
    {
        texture = LoadPNG(Application.dataPath + "/base.png");

        GetComponent<Renderer>().material.mainTexture = texture;
        r = new System.Random();
    }

    void Update()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if(r.NextDouble() < 0.01)
                    texture.SetPixel(x, y, new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 0));
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
}