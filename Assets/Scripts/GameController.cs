using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum estado { finpartida,resume, idle};
    public static estado estadojuego = estado.idle;

    public static float puntos;

    public GameObject PanelPerder;


    public Text tiempotexto;
    public float tiempototal,segundos,minutos;

    GameObject [] navecitas;

    public GameObject PanelFinPartida;
    public Text textNave1, textNave2, textNave3;
    public Text textoTiempo, textoPuntaje;


    public GameObject UIplay;
    public Text CantiNave1, CantiNave2, CantiNave3;


    bool finpartida;

    NavesController NavesJugador;
    public GameObject lineaLateralDerecha, lineaLateralIzquierda;

    public GameObject PanelTutorial;

    void Start()
    {
        PanelFinPartida.SetActive(false);
        estadojuego = estado.idle;
        finpartida = false;


        NavesJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<NavesController>();
        lineaLateralDerecha.SetActive(false);
        lineaLateralIzquierda.SetActive(false);



        minutos = 0;
        puntos = 0;
        tiempototal = 0;
        navecitas = GameObject.FindGameObjectsWithTag("nave");



        Eventos.evento_RevisarNaves = null;
        Eventos.evento_Perder = null;
        Eventos.evento_RevisarNaves += RevisarNaves;
        Eventos.evento_Perder += Perder;


        UIplay.SetActive(false);

        PanelPerder.SetActive(false);
        PanelTutorial.SetActive(true);
        RevisarNaves();

    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow) && estadojuego==estado.idle)
        {
            estadojuego = estado.resume;
            PanelTutorial.SetActive(false);
            //Eventos.evento_invocarNaveGrande();
        }

        if (estadojuego == estado.resume)
        {
            UIplay.SetActive(true);
            reloj();
            RevisarNaves();
            ControlarLineasLaterales();
        }


        if (estadojuego==estado.finpartida && !finpartida)
        {
            lineaLateralDerecha.SetActive(false);
            lineaLateralIzquierda.SetActive(false);
            UIplay.SetActive(false);
            PanelFinPartida.SetActive(true);
            Puntaje();
            //Eventos.evento_DesinvocarNaveGrande();
            finpartida = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
            Reiniciar();

    }

    void reloj()
    {
        segundos += Time.deltaTime;
        tiempototal += Time.deltaTime;
        if (segundos >= 59)
        {
            minutos++;
            segundos -= 59;
        }


        // tiempotexto.text =minutos.ToString("00")+":"+ segundos.ToString("00");
        tiempotexto.text = tiempototal.ToString("00");

    }

    void Puntaje()
    {
        navecitas = GameObject.FindGameObjectsWithTag("nave");

        int tiponave1 = 0;
        int tiponave2 = 0;
        int tiponave3 = 0;
        float resultadonav1 = 0;
        float resultadonav2 = 0;
        float resultadonav3 = 0;
        foreach (GameObject navecita in navecitas)
        {
            puntos += navecita.GetComponent<NavicitaController>().puntos;
            switch (navecita.GetComponent<NavicitaController>().tiponave)
            {
                case 1:
                    tiponave1++;
                    resultadonav1 += navecita.GetComponent<NavicitaController>().puntos;
                    break;
                case 2:
                    tiponave2++;
                    resultadonav2 += navecita.GetComponent<NavicitaController>().puntos;
                    break;
                case 3:
                    tiponave3++;
                    resultadonav3 += navecita.GetComponent<NavicitaController>().puntos;
                    break;
            }
        }
        textNave1.text = "X " + tiponave1 + " =" + resultadonav1;
        textNave2.text = "X " + tiponave2 + " =" + resultadonav2;
        textNave3.text = "X " + tiponave3 + " =" + resultadonav3;
        textoTiempo.text =""+ (int)tiempototal;
        puntos -= (int)tiempototal;
        if (puntos > 0)
        {
            textoPuntaje.text = "SCORE "+(int)puntos;

            Application.ExternalCall("kongregate.stats.submit", "Score", puntos);

        }
        else
        {
            textoPuntaje.text = "0";
        }
    }

    void RevisarNaves()
    {
        navecitas = GameObject.FindGameObjectsWithTag("nave");

        int tiponave1 = 0;
        int tiponave2 = 0;
        int tiponave3 = 0;
        foreach (GameObject navecita in navecitas)
        {
            switch (navecita.GetComponent<NavicitaController>().tiponave)
            {
                case 1:
                    tiponave1++;
                    break;
                case 2:
                    tiponave2++;
                    break;
                case 3:
                    tiponave3++;
                    break;
            }
        }
        CantiNave1.text = tiponave1.ToString();
        CantiNave2.text = tiponave2.ToString();
        CantiNave3.text = tiponave3.ToString();

        

    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Perder()
    {
        if (!finpartida)
        {
            finpartida = true;
            StartCoroutine(TiempoPerder());

        }
    }

    IEnumerator TiempoPerder()
    {
        Eventos.evento_camaraFin();
        yield return new WaitForSeconds(.5f);
        estadojuego = estado.finpartida;
        PanelPerder.SetActive(true);

    }

    void ControlarLineasLaterales()
    {
        if (NavesJugador.dire != NavesController.direccion.derecha)
        {
            lineaLateralDerecha.SetActive(true);
        }
        else
        {
            lineaLateralDerecha.SetActive(false);
        }
        if (NavesJugador.dire != NavesController.direccion.izquierda)
        {
            lineaLateralIzquierda.SetActive(true);
        }
        else
        {
            lineaLateralIzquierda.SetActive(false);
        }
    }
}
