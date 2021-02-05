using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawneadorObjetos : MonoBehaviour
{
    [SerializeField] int probabilidadNAve=50;
    [SerializeField] int tiempoSpawn = 10;
    public GameObject NaveGrande;

    bool invocar;

    void Start()
    {
        invocar = false;

    }
    private void Update()
    {
        if(GameController.estadojuego==GameController.estado.resume && !invocar)
        {
            Invocar();
            invocar = true;
            
        }
        if(GameController.estadojuego == GameController.estado.finpartida)
        {
            Desinvocar();
        }
    }
    void Invocar()
    {
        InvokeRepeating("SpawnNaveGrande", 5, tiempoSpawn);
    }
    void Desinvocar()
    {
        CancelInvoke();
    }
    void SpawnNaveGrande()
    {
        if (Random.Range(0, 100) <= probabilidadNAve)
        {
            Vector3 pos;
            if (Random.Range(0, 100) > 50)
            {
                pos = new Vector3(8.5f, 4.2f, 0);
            }
            else
            {
                pos = new Vector3(-8.5f, 4.2f, 0);
            }
            Instantiate(NaveGrande, pos, Quaternion.identity);
        }
        else
        {
            return;
        }
    }


}
