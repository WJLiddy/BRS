using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

    public MovieTexture movTexture;
    double endgame_timer = 0;
    int[,] score_ratings = new int[4,5];

    void Start()
    {
        movTexture = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
        movTexture.Play();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void Update()
    {
        if (!movTexture.isPlaying)
        {
    


        }
    }
}

