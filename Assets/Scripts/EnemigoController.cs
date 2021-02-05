using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public enum enemigoestado { izquierda,derecha,idle, congelado};
    public enemigoestado estado =enemigoestado.idle;

    [SerializeField] float velMov;
    [SerializeField] float tiempoCongelado;
    GameObject Naves;

    float tiempoCambio;

    bool cambioDireccion;

    float tiempoParado;

    public bool enemigodelante;

    AudioSource sonido;
    public AudioClip congeladoSonido;


    SpriteRenderer sprite;


    void Start()
    {
        estado = enemigoestado.idle;

        sonido = GetComponent<AudioSource>();

        sprite = GetComponent<SpriteRenderer>();

        Naves = GameObject.FindGameObjectWithTag("Player");


        tiempoCambio = Random.Range(30, 60);
        cambioDireccion = true;


        tiempoParado = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.estadojuego == GameController.estado.resume && estado!=enemigoestado.congelado)
        {
            Movimiento();

            //ControlarEstancamiento();

            if (cambioDireccion)
            {
                CambiarDireccion();
                CambiarLados();
            }
        }
    }

    IEnumerator Congelamiento()
    {
        estado = enemigoestado.congelado;
        sprite.color = Color.red;
        yield return new WaitForSeconds(tiempoCongelado);
        sprite.color = Color.white;
        CambiarEstado();

    }
    IEnumerator acomodarTiempo()
    {
        cambioDireccion = false;
        if (transform.position.x >= 3.5f)
        {
            estado = enemigoestado.izquierda;
        }
        else if (transform.position.x <= -3.5f)
        {
            estado = enemigoestado.derecha;
        }
        yield return new WaitForSeconds(1.3f);
        cambioDireccion = true;
    }
    void CambiarDireccion()
    {
        tiempoCambio--;
        if (tiempoCambio <= 0)
        {
            //print(tiempoCambio);
            CambiarEstado();
            tiempoCambio = Random.Range(25, 70);
        }
    }



    void CambiarEstado()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                estado = enemigoestado.idle;
                break;
            case 1:
                int probabilidad = Random.Range(0, 100);
                if(probabilidad>40)
                    estado = enemigoestado.derecha;
                else if(probabilidad<=40 && probabilidad>35)
                    estado = enemigoestado.idle;
                else
                    estado = enemigoestado.izquierda;


                break;
            case 2:
                int probabilidad2 = Random.Range(0, 100);
                if (probabilidad2 > 40)
                    estado = enemigoestado.izquierda;
                else if (probabilidad2 <= 40 && probabilidad2 > 35)
                    estado = enemigoestado.idle;
                else
                    estado = enemigoestado.derecha;
                
                break;
        }
    }

    private void Movimiento()
    {
        switch (estado)
        {
            case enemigoestado.idle:

                break;
            case enemigoestado.izquierda:
                transform.position += new Vector3(-velMov,0,0) * Time.deltaTime;
                break;
            case enemigoestado.derecha:
                transform.position += new Vector3(velMov, 0, 0) * Time.deltaTime;
                break;
        }

        transform.position=new Vector3( Mathf.Clamp(transform.position.x, -6.6f, 6.6f),
                                          transform.position.y,transform.position.z);
    }

    void CambiarLados()
    {
        

        if (Naves.transform.childCount == 1)
        {
            if (transform.position.x <= NavesController.extremoIzquierdo.x-.7)
            {
                estado = enemigoestado.derecha;
            }
            if (transform.position.x >= NavesController.extremoDerecho.x+.7f)
            {
                estado = enemigoestado.izquierda;
            }
        }
        else
        {
            if (transform.position.x <= NavesController.extremoIzquierdo.x)
            {
                estado = enemigoestado.derecha;
                tiempoCambio = 70;
            }
            if (transform.position.x >= NavesController.extremoDerecho.x)
            {
                estado = enemigoestado.izquierda;
                tiempoCambio = 70;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("balajugador"))
        {
            if (estado != enemigoestado.congelado)
            {
                sonido.PlayOneShot(congeladoSonido);
                Destroy(collision.gameObject);
                StartCoroutine(Congelamiento());
            }
        }
    }

}
