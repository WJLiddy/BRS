using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Plane : MonoBehaviour {

    public MovieTexture movTexture;
    double endgame_timer = 0;
    bool ratings_ready = false;
    int[,] score_ratings = new int[4,6];
    TextMesh t0;
    TextMesh t1;
    TextMesh t2;
    TextMesh t3;

    void Start()
    {
        t0 = GameObject.Find("Canvas0text").GetComponent<TextMesh>();
        t1 = GameObject.Find("Canvas1text").GetComponent<TextMesh>();
        t2 = GameObject.Find("Canvas2text").GetComponent<TextMesh>();
        t3 = GameObject.Find("Canvas3text").GetComponent<TextMesh>();
        movTexture = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
        movTexture.Play();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void Update()
    {

        //if (Time.fixedTime > ((5 * 60) + 45))
            if (Time.fixedTime > (30))
            {

            
            if (ratings_ready)
            {
                //  int t = (int)((Time.fixedTime - ((5 * 60) + 45)) / 4.0);
                int t = (int)((Time.fixedTime - 30) / 4.0);
                t0.text = get_msg(0, t);
                t1.text = get_msg(1, t);
                t2.text = get_msg(2, t);
                t3.text = get_msg(3, t);
            } else
            {
                ratings_ready = true;
                give_scores();
                Destroy(GameObject.Find("Brush0"));
                Destroy(GameObject.Find("Brush1"));
                Destroy(GameObject.Find("Brush2"));
                Destroy(GameObject.Find("Brush3"));
            }

        }
    }
    
    string get_msg(int pid, int reveal)
    {
        string n = "";
        if (reveal > 0)
            // How well did you match on macro?
            n += "STRUCTURE: " + stars(score_ratings[pid, 0]) + "\n\n";
        if (reveal > 1)
            // How were your colors?
            n += "CONTRAST: " + stars(score_ratings[pid, 1]) + "\n\n";
        if (reveal > 2)
            // Random lol
            n += "CREATIVITY: " + stars(score_ratings[pid, 2]) + "\n\n";
        if (reveal > 3)
            // How much time did you spend painting vs not painting?
            n += "PASSION: " + stars(score_ratings[pid, 3]) + "\n\n";
        if (reveal > 4)
            // how much did you NOT color on other peoples paintings
            n += "FRIENDSHIP: " + stars(score_ratings[pid, 4]) + "\n\n";
        if (reveal > 5)
            n += "TOTAL: " + score_ratings[pid, 5]  + " " +  stars(1) + "\n\n";
        return n;
    }

    string stars(int starcount)
    {
        string n = "";
        for (int i = 0; i != starcount; i++)
        {
            n += '\u2605';
        }
        return n;
    }

    void give_scores()
    {
        for(int i = 0;i != 4; i++)
        {

            // each score
            int score = 9001;
            int total = 0;
            for (int s = 0; s != 5; s++)
            {
                if (s == 0 || s == 1)
                {
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(File.ReadAllBytes("Assets/misc_materials/final.jpg")); //..this will auto-resize the texture dimensions.
                    Texture2D usr = GameObject.Find("Canvas" + i).GetComponent<CanvasOps>().texture;
                    // Unzip into an array
                    //convert texture to cmy completely dry.
                    float[,,] ross = new float[tex.width, tex.height, 3];
                    float[,,] player = new float[tex.width, tex.height, 3];

                    for (int x = 0; x != tex.width; x++)
                    {
                        for (int y = 0; y != tex.height; y++)
                        {
                            ross[x, y, 0] = tex.GetPixel(x, y)[0];
                            ross[x, y, 1] = tex.GetPixel(x, y)[1];
                            ross[x, y, 2] = tex.GetPixel(x, y)[2];

                            player[x, y, 0] = usr.GetPixel(x, y)[0];
                            player[x, y, 1] = usr.GetPixel(x, y)[1];
                            player[x, y, 2] = usr.GetPixel(x, y)[2];

                        }
                    }

                    if (s == 0)
                    {
                        int serr = StructError(ross, player, 3, 0.01f);
                        Debug.Log(i + " SCORED " + serr + "IN SERR");

                        if (serr < 200)
                            score = 5;
                        else if (serr < 300)
                            score = 4;
                        else if (serr < 400)
                            score = 3;
                        else
                            score = 2;
                    }

                    if (s == 1)
                    {
                        float ddef = DeviDiff(ross, player);
                        Debug.Log(i + " SCORED " +ddef + "IN DDEF");
                        if (ddef < 0.5)
                            score = 5;
                        else if (ddef < 0.6)
                            score = 4;
                        else if (ddef < 0.7)
                            score = 3;
                        else
                            score = 2;
                    }

                }
                else
                {
                    //the other 3 scores are random!
                    if(s == 3)
                    {
                        score = UnityEngine.Random.Range(2, 6);
                    }

                    if (s == 4)
                    {
                        //sort by passion
                        int p = GameObject.Find("Brush" + i).GetComponent<Brush>().passion;
                        Debug.Log(i + "ID HAS PASSION " + p);
                        //TEMP!
                        score = 1;
                    }

                    if (s == 5)
                    {
                        //sort by passion
                        int p = GameObject.Find("Brush" + i).GetComponent<Brush>().evil;
                        Debug.Log(i + "ID HAS EVIL " + p);
                        //TEMP!
                        score = 1;
                    }


                }

                score_ratings[i, s] = score;
                total += score;
            }
            score_ratings[i, 5] = total;
        }
    }

    static float Mean(float[,] matrix)
    {
        float mean = 0.0F;
        int width = matrix.GetLength(0);
        int length = matrix.GetLength(1);

        for (int lindex = 0; lindex < length; lindex++)
        {
            for (int windex = 0; windex < width; windex++)
            {
                mean += matrix[windex, lindex];
            }
        }
        mean = mean / (width * length); // we find the mean
                                        //Console.WriteLine(mean);

        return mean;
    }

    static float Deviation(float[,] matrix)
    {
        float dev = 0.0F; //
        float sum = 0.0F;
        float mean = Mean(matrix);

        int width = matrix.GetLength(0);
        int length = matrix.GetLength(1);

        for (int lindex = 0; lindex < length; lindex++)
        {
            for (int windex = 0; windex < width; windex++)
            {
                sum = sum + (matrix[windex, lindex] - mean) * (matrix[windex, lindex] - mean);
                dev = matrix[windex, lindex];
                //Console.WriteLine("matrix: {0}", (matrix[windex, lindex] - mean) * (matrix[windex, lindex] - mean));
            }
        }
        sum = sum / (length * width);
        dev = (float)(Math.Sqrt(sum));
        //Console.WriteLine("SD: {0}", dev);

        return dev;//returning st deviation.

    }

    static float DeviDiff(float[,,] Ross, float[,,] Draw)
    {
        float diff;//the difference to be returned
        int rossW = Ross.GetLength(0);
        int rossL = Ross.GetLength(1);

        int drawW = Draw.GetLength(0);
        int drawL = Draw.GetLength(1);

        float[,] Ross0, Ross1, Ross2;
        Ross0 = new float[rossW, rossL];//make three 2 dimention ross arrays
        Ross1 = new float[rossW, rossL];//make three 2 dimention ross arrays
        Ross2 = new float[rossW, rossL];//make three 2 dimention ross arrays
        float[,] Draw0, Draw1, Draw2;
        Draw0 = new float[drawW, drawL];//make three 2 dimention draw arrays
        Draw1 = new float[drawW, drawL];//make three 2 dimention draw arrays
        Draw2 = new float[drawW, drawL];//make three 2 dimention draw arrays
        for (int lindex = 0; lindex < rossL; lindex++)
        {
            for (int windex = 0; windex < rossW; windex++)
            {
                Ross0[windex, lindex] = Ross[windex, lindex, 0];
                Ross1[windex, lindex] = Ross[windex, lindex, 1];
                Ross2[windex, lindex] = Ross[windex, lindex, 2];

                Draw0[windex, lindex] = Draw[windex, lindex, 0];
                Draw1[windex, lindex] = Draw[windex, lindex, 1];
                Draw2[windex, lindex] = Draw[windex, lindex, 2];

            }
        }

        float rossDev1, rossDev2, rossDev3;
        rossDev1 = Deviation(Ross0);
        rossDev2 = Deviation(Ross1);
        rossDev3 = Deviation(Ross2);

        float drawDev1, drawDev2, drawDev3;
        drawDev1 = Deviation(Draw0);
        drawDev2 = Deviation(Draw1);
        drawDev3 = Deviation(Draw2);

        // Console.WriteLine(rossDev1);


        diff = Math.Abs(rossDev1 - drawDev1) + Math.Abs(rossDev2 - drawDev2) + Math.Abs(rossDev3 - drawDev3);

        return diff;
    }

    static float[,,] reduce(float[,,] big, int scale)
    {
        int bigWidth = big.GetLength(0);
        int bigLength = big.GetLength(1);
        int wBigIndex = bigWidth / scale;
        int lBigIndex = bigLength / scale;

        float[,,] scaled = new float[bigWidth / scale, bigLength / scale, 3];
        float average;

        for (int width = 0; width < wBigIndex; width += scale)//Here we find the average of the submatricies.
        {
            for (int length = 0; length < lBigIndex; length += scale)//we work by each individual matrix
            {
                for (int color = 0; color < 3; color++)//covers width from width to width + scale.
                {
                    average = 0;//Set average color
                    for (int windex = width; windex < (width + scale); windex += scale) // cover each length
                    {
                        for (int lindex = length; lindex < (length + scale); lindex++)//... and each column
                        {
                            average += (big[windex, lindex, color]);
                        }
                        average = average / (scale * scale);
                    }
                    //After all that, set the scaled color gere
                    average = average / (scale * scale);
                    scaled[width / scale, length / scale, color] = average; //set the color, finally. 
                }
            }
        }

        return scaled;

    }


    static int StructError(float[,,] Ross, float[,,] Draw, int scale, float handicap) //Structure
    {
        int errors = 0;
        float[,,] scaleRoss;
        float[,,] scaleDraw; // We determine the measurements above

        scaleRoss = reduce(Ross, scale);
        scaleDraw = reduce(Draw, scale);

        int rossW = scaleRoss.GetLength(0);
        int rossL = scaleRoss.GetLength(1);

        for (int color = 0; color < 3; color++)
        {
            for (int lindex = 0; lindex < rossL; lindex++)
            {
                for (int windex = 0; windex < rossW; windex++)
                {
                    if ((scaleRoss[windex, lindex, color] + handicap) < (scaleDraw[windex, lindex, color]))
                    { errors++; }
                    if ((scaleRoss[windex, lindex, color] - handicap) > (scaleDraw[windex, lindex, color]))
                    { errors++; }
                }
            }
        }

        return errors;



    }
}

