using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CanvasOps : MonoBehaviour
{
    Texture2D texture;
    // cmds
    float[,,] cmy = new float[1600, 1200, 3];
    float[,] wetness = new float[1600, 1200];
    LinkedList<int[]> new_paint;
    System.Random r;

    void Start()
    {
        new_paint = new LinkedList<int[]>();
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
                new_paint.AddFirst(new int[2] { x, y});
            }
        }
        GetComponent<Renderer>().material.mainTexture = texture;
        r = new System.Random();
    }

    void Update()
    {
        //convert cmy to rgb
        //apply paint to these pixels
        foreach (int[] element in new_paint)
        {
            int x = element[0];
            int y = element[1];
            float r = (1f - cmy[x, y ,0]);
            float g = (1f - cmy[x, y, 1]);
            float b = (1f - cmy[x, y, 2]);
            texture.SetPixel(x,y, new Color(r, g, b, 0));
        }
        new_paint = new LinkedList<int[]>();
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

    public void applyBrush(float old_x, float old_y, float targ_x, float targ_y, int brush_radius, float cy, float ma, float ye, float str)
    {
        //Don't draw over !

        int delta_steps = 10;
        for (int s = 0; s != delta_steps; s++)
        { 
           float x = old_x + ((targ_x - old_x) * ((float)s / delta_steps));
           float y = old_y + ((targ_y - old_y) * ((float)s / delta_steps));
            
            // Transform on Y axis
            y = -y;
            //change x to reference top right corner
            x = x + 1.5F;
            y = y + 2.0F;
            //scale x, y from 0.3 to 0.4: MULTIPLY BY 100
            if (x < 0.0 || y < 0.0 || x >= 3 || y >= 4)
                return;

            //apply paint to these pixels
            for (int dx = -brush_radius; dx != brush_radius; dx++)
            {
                for (int dy = -brush_radius; dy != brush_radius; dy++)
                {
                    if ((((float)dx * (float)dx) + ((float)dy * (float)dy)) < ((float)brush_radius * (float)brush_radius))
                    {
                        //don't ask
                        int canvas_x = texture.width - (dx + (int)(y * 400F));
                        int canvas_y = texture.height - (dy + (int)(x * 400F));
                        if (canvas_x < 0 || canvas_y < 0 || canvas_x >= texture.width || canvas_y >= texture.height)
                            continue;
                        float new_paint_ratio = (str) / (1F + wetness[canvas_x, canvas_y]);
                        cmy[canvas_x, canvas_y, 0] = (new_paint_ratio * cy) + ((1F - new_paint_ratio)*cmy[canvas_x, canvas_y, 0]);
                        cmy[canvas_x, canvas_y, 1] = (new_paint_ratio * ma) + ((1F - new_paint_ratio)*cmy[canvas_x, canvas_y, 1]);
                        cmy[canvas_x, canvas_y, 2] = (new_paint_ratio * ye) + ((1F - new_paint_ratio)*cmy[canvas_x, canvas_y, 2]);
                        wetness[canvas_x, canvas_y] = str;
                        new_paint.AddFirst(new int[2] { canvas_x, canvas_y });
                    }
                }
            }
        }
    }
}