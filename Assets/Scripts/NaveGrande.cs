using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveGrande : MonoBehaviour
{

    [SerializeField] float vel;

    GameController gamecontrol;

    Animator animator;
    AudioSource sonido;
    void Start()
    {
        if (transform.position.x > 0)
        {
            vel *= -1;
        }
        sonido = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        gamecontrol = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(vel, 0, 0) * Time.deltaTime;
        Destruir();
    }

    void Destruir()
    {
        if(transform.position.x>10 || transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bala"))
        {
            gamecontrol.segundos += 30;
            gamecontrol.tiempototal += 30;
            StartCoroutine(muerte());
            Destroy(collision.gameObject);
        }
    }
    IEnumerator muerte()
    {
        sonido.PlayOneShot(sonido.clip);
        animator.enabled = true;
        animator.Play("NaveGrande_muerte");
        yield return new WaitForSeconds(.4f);
            Destroy(this.gameObject);
    }
}
