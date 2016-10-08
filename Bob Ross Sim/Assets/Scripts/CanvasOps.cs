using UnityEngine;
using System.IO;
public class CanvasOps : MonoBehaviour
{
    Texture2D texture;
    System.Random r;

    void Start()
    {
        texture = LoadPNG(Application.dataPath + "/misc_materials/base.png");

        GetComponent<Renderer>().material.mainTexture = texture;
        r = new System.Random();
    }

    void Update()
    {

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

    public void applyBrush(float x, float y)
    {
        // Transform on Y axis
        y = -y;
        //change x to reference top right corner
        x = x + 1.5F;
        y = y + 2.0F;
        //Debug.Log(x + "," + y);
        //scale x, y from 0.3 to 0.4: MULTIPLY BY 100
        if (x < 0.0 || y < 0.0 || x >= 3 || y >= 4)
            return;
        //(float)r.NextDouble()
        for (int dx = -3; dx != 4; dx++)
        {
            for (int dy = -3; dy != 4; dy++)
            {
                texture.SetPixel(400 - (dx + (int)(y * 100F)), 300 - (dy + (int)(x * 100F)), new Color(1F, 0, 0, 0));
            }        }


    }
}