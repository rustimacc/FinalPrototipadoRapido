using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavicitaController : MonoBehaviour
{

    NavesController naves;

    Animator animator;
    SpriteRenderer imagen;
    public Sprite[] sprites;


    public int vida = 1;
    bool muerto;

    public bool escudo;

    public float puntos;
    public int tiponave;

    void Start()
    {


        naves = transform.parent.GetComponent<NavesController>();
        animator = GetComponent<Animator>();
        animator.enabled = false;

        imagen = GetComponent<SpriteRenderer>();


        muerto = false;
    }

    private void Update()
    {
        ControlarVida();
        Animacion();
        Escudo();
        //ganar();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {

            case "paredDerecha":
                naves.BajarNavesDerecha();
                naves.limiteDerecha = true;
                Eventos.evento_camaraLaterales();
                break;
            case "paredIzquierda":
                naves.BajarNavesIzquierda();
                naves.limiteIzquierda = true;
                Eventos.evento_camaraLaterales();
                break;
            case "enemigo":
                //GameController.estadojuego = GameController.estado.finpartida;
                StartCoroutine(terminarpartida(collision.gameObject));
                Eventos.evento_camaraFin();
                break;
            case "Finish":
                //GameController.estadojuego = GameController.estado.finpartida;
                StartCoroutine(terminarpartida(collision.gameObject));
                Eventos.evento_camaraFin();
                break;
        }
    }
    IEnumerator terminarpartida(GameObject enemigo)
    {

        Destroy(enemigo);
        yield return new WaitForSeconds(1);
        GameController.estadojuego = GameController.estado.finpartida;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {

            case "paredDerecha":
                naves.limiteDerecha = false;
                break;
            case "paredIzquierda":
                naves.limiteIzquierda = false;
                break;
            
        }
    }
    
    private void Escudo()
    {
        if(vida==1 && escudo)
        {
            //puntos -= 25;
            imagen.color = Color.red;
        }
    }

    private void ControlarVida()
    {
        if(vida<=0 && !muerto)
        {
            
            StartCoroutine(morir());
            muerto = true;
        }
    }
    
    IEnumerator morir()
    {
        animator.enabled = true;
        animator.Play("Nave_muerta");

        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);
    }

    private void Animacion()
    {
        if (naves.ani1)
        {
            imagen.sprite = sprites[0];
        }
        else
        {
            imagen.sprite = sprites[1];
        }
    }

    

}
