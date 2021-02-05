using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class NavesController : MonoBehaviour
{
    [SerializeField] float distanciaMov=.5f;
    [SerializeField] float distanciaBajar=.5f;
    [SerializeField] float tiempoBajar=.1f;

    public enum direccion { izquierda,derecha,principio};
    public direccion dire=direccion.principio;

    public bool limiteDerecha;
    public bool limiteIzquierda;

    public bool ani1;

    public static Vector3 extremoIzquierdo, extremoDerecho;

    AudioSource sonido;
    public AudioClip[] sonidos;


    public GameObject balaJugador;
    [SerializeField] int MaximoDisparo,MinimoDisparo;
    int limiteparaDisparo;

    
    int movParaDisparo;

    GameObject[] navecitas;

    private void Awake()
    {
        sonido = GetComponent<AudioSource>();





        Eventos.evento_camaraDanio = null;
        Eventos.evento_camaraDanio = null;
        Eventos.evento_camaraFin = null;
        Eventos.evento_SonidoDanio = null;

        Eventos.evento_camaraLaterales += camaraLateral;
        Eventos.evento_camaraDanio += camaraDanio;
        Eventos.evento_camaraFin += camaraFin;
        Eventos.evento_SonidoDanio += SonidoDanio;
    }
    void Start()
    {


        direccion dire = direccion.principio;

        limiteDerecha = false;
        limiteIzquierda = false;
        ani1 = true;

        extremoDerecho = Vector3.zero;
        extremoIzquierdo = Vector3.zero;

        limiteparaDisparo = Random.Range(MinimoDisparo, MaximoDisparo);
        movParaDisparo=0;

    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.estadojuego!=GameController.estado.finpartida)
            Controles();

        if(transform.childCount >0)
            MedirExtremos();
        else//Si no hay mas naves
        {
            Eventos.evento_Perder();
            //StartCoroutine(tiempoPerder());            
        }

    }
    IEnumerator tiempoPerder()
    {
        //camaraFin();
        yield return new WaitForSeconds(.5f);
        GameController.estadojuego = GameController.estado.finpartida;
    }
    private void Controles()
    {
        if (Input.GetKeyDown(KeyCode.D) && !limiteDerecha || Input.GetKeyDown(KeyCode.RightArrow) && !limiteDerecha)
        {
            movParaDisparo++;
            transform.position += new Vector3(distanciaMov, 0, 0);
            ani1 = !ani1;
        }
        if (Input.GetKeyDown(KeyCode.A) && !limiteIzquierda || Input.GetKeyDown(KeyCode.LeftArrow) && !limiteIzquierda)
        {
            movParaDisparo++;
            transform.position += new Vector3(-distanciaMov, 0, 0);
            ani1 = !ani1;
        }

        if (movParaDisparo >= limiteparaDisparo)
        {
            Disparo();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Disparo();
        }

    }

    void Disparo()
    {
        int posnave = Random.Range(0, transform.childCount - 1);
        StartCoroutine(animarNavecita(posnave));

        Vector3 pos = transform.GetChild(posnave).transform.position;
        Instantiate(balaJugador, pos, Quaternion.identity);

        movParaDisparo = 0;
        limiteparaDisparo = Random.Range(MinimoDisparo, MaximoDisparo); ;
    }
    IEnumerator animarNavecita(int posnave)
    {
        transform.GetChild(posnave).transform.GetComponent<Animator>().enabled=true;
        transform.GetChild(posnave).transform.GetComponent<Animator>().Play("Navecita Disparo");
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(posnave).transform.GetComponent<Animator>().enabled = false;

    }
    public void BajarNavesDerecha()
    {
        if (dire != direccion.derecha)
        {
            StartCoroutine(Bajar());

            dire = direccion.derecha;
        }
    }
    public void BajarNavesIzquierda()
    {
        if (dire != direccion.izquierda)
        {
            StartCoroutine(Bajar());
            dire = direccion.izquierda;
        }
    }

    

    IEnumerator Bajar()
    {
        sonido.PlayOneShot(sonidos[0]);
        yield return new WaitForSeconds(tiempoBajar);
        transform.position += new Vector3(0, -distanciaBajar, 0);
    }

    public void MedirExtremos()
    {
        if(transform.childCount>1)
            extremoDerecho = transform.GetChild(transform.childCount-1).transform.position;
        else
        {
        extremoDerecho = transform.GetChild(0).transform.position;
        }
        extremoIzquierdo = transform.GetChild(0).transform.position;
    }

    void camaraLateral()
    {
        CameraShaker.Instance.ShakeOnce(.2f, 2.5f, .8f, .8f);
        //sonido.PlayOneShot(sonidos[0]);
    }
    void camaraFin()
    {
        CameraShaker.Instance.ShakeOnce(1.5f, 2.5f, .6f, 1.5f);
        print("este se mueve");
    }
    void camaraDanio()
    {
        CameraShaker.Instance.ShakeOnce(.2f, 1.5f, .7f, .7f);
    }

    void SonidoDanio()
    {
        sonido.PlayOneShot(sonidos[1]);
    }
}


