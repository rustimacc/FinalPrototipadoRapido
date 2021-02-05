using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSonidos : MonoBehaviour
{
    AudioSource sonido;
    public  AudioClip[] sonidos;


    void Start()
    {
        sonido = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tecla(int tecla)
    {
        sonido.PlayOneShot(sonidos[tecla]);
    }
}
