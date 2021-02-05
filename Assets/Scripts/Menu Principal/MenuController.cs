using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    bool ActivarCreditos;
    bool EnEspaniol;
    public Animator animatorUI;

    public Text textoCreditos;

    [TextArea(5,10)]
    public string Espaniol;
    [TextArea(6,12)]
    public string Ingles;

    void Start()
    {
        ActivarCreditos = false;

        EnEspaniol = true;
    }

    
    void Update()
    {
        
    }

    public void EmpezaraJugar(string nivel)
    {
        SceneManager.LoadScene(nivel);
    }

    public void Creditos()
    {
        ActivarCreditos = !ActivarCreditos;
        if (ActivarCreditos)
        {
            animatorUI.Play("Activar Creditos");
        }
        else
        {
            animatorUI.Play("Desactivar Creditos");
        }
    }

    public void CambiarIdioma()
    {
        EnEspaniol = !EnEspaniol;

        if (EnEspaniol)
        {
            textoCreditos.text = Espaniol;
        }
        else
        {
            textoCreditos.text = Ingles;
        }

    }
}
