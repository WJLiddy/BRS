using UnityEngine;
using System.Collections;

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

        if (Time.fixedTime > ((4 * 60) + 45))
        {

            
            if (ratings_ready)
            {
                int t = (int)((Time.fixedTime - ((4 * 60) + 45)) / 4.0);
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

    // Name 0, 1-5, 6.
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
            int total = 0;
            for (int s = 0; s != 5; s++)
            {
                int score = Random.Range(2, 6);
                score_ratings[i, s] = score;
                total += score;
            }
            score_ratings[i, 5] = total;
        }
    }
}

