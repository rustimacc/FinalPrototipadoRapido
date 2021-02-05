using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaJugador : MonoBehaviour
{
    [SerializeField] float velBala = 1;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Disparo();
        Destruir();
    }

    private void Disparo()
    {
        transform.position += new Vector3(0, -velBala, 0) * Time.deltaTime;
    }

    void Destruir()
    {
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemigo"))
        {
            
            Destroy(this.gameObject);

            print("Congelado");

            }
    }

}
