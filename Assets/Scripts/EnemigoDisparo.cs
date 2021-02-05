using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDisparo : MonoBehaviour
{
    public GameObject bala;

    private int tiempoDisparo;
    public int tiempoMinDisparo, tiempoMaxDisparo;

    bool permitirDisparo;

    EnemigoController enemigocontrol;
    GameObject naves;

    AudioSource sonido;
    public AudioClip disparosonido;


    [SerializeField] int probabilidadTriple;
    void Start()
    {
        sonido = GetComponent<AudioSource>();

        enemigocontrol = GetComponent<EnemigoController>();

        naves = GameObject.FindGameObjectWithTag("Player");

        tiempoMinDisparo = 100;
        tiempoMaxDisparo = 160;
        tiempoDisparo = Random.Range(tiempoMinDisparo,tiempoMaxDisparo);
    }


    void Update()
    {
        //if(enemigocontrol.enemigodelante)

        if (GameController.estadojuego == GameController.estado.resume && enemigocontrol.estado!=EnemigoController.enemigoestado.congelado)
        {

            Disparo();
            //StartCoroutine(Tripledisparo());
            RayoDisparo();

        }
    }

    void Disparo()
    {

        tiempoDisparo--;

        if (tiempoDisparo <= 0)
        {

            if (Random.Range(0, 100) <= probabilidadTriple)
            {
                sonido.PlayOneShot(disparosonido);
                Instantiate(bala, transform.position, Quaternion.identity);
                tiempoDisparo = Random.Range(tiempoMinDisparo, tiempoMaxDisparo);
            }
            else
            {
                StartCoroutine(Tripledisparo());
                tiempoDisparo = Random.Range(tiempoMinDisparo, tiempoMaxDisparo);
            }
            
        }
    }

    IEnumerator Tripledisparo()
    {
        for (int i = 0; i < 3; i++)
        {
            sonido.PlayOneShot(disparosonido);
            Instantiate(bala, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f); // wait till the next round
        }
        /*
        Instantiate(bala, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f); // wait till the next round
        Instantiate(bala, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f); // wait till the next round
        */
    }



    void RayoDisparo()
    {
        RaycastHit2D hit= Physics2D.Raycast(transform.position, transform.up,10);

        if(hit.collider.CompareTag("permitirdisparo"))
        {
            //print("detecta colision");
            enemigocontrol.enemigodelante = true;
        }
        else
        {
            enemigocontrol.enemigodelante = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.up);
    }
}
