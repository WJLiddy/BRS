using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

    public MovieTexture movTexture;
    double endgame_timer;

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

