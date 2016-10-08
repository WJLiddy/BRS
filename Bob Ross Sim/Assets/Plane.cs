using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

    public MovieTexture movTexture;
    void Start()
    {
        movTexture = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
        movTexture.Play();
    }
}

