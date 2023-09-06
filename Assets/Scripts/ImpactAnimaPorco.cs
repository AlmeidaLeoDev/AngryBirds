using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ImpactAnimaPorco : MonoBehaviour
{
    private Animator animacoes;
    private int limite = -1;
    public string[] clips;
    [SerializeField]
    private GameObject bomb, pontos1000;

    // Start is called before the first frame update
    void Start()
    {
        animacoes = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //relativeVelocity = velocidade linear relativa de dois objetos colidindo
        if (collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 10) //Verificar for�a de colis�o
        {
            if (limite < clips.Length - 1) //Se limite � menor do que o indice m�ximo permitio no array "sprites"
            {
                limite++; //limite � incrementado
                animacoes.Play (clips[limite]); //o sprite atual � trocado pelo pr�ximo do array
            }
            else if (limite == clips.Length - 1)
            {
                Instantiate(pontos1000, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                GAMEMANAGER.instance.numPorcosCena -= 1;
                Destroy(gameObject);
            }
        }
        else if (collision.relativeVelocity.magnitude > 12 && collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pontos1000, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            GAMEMANAGER.instance.numPorcosCena -= 1;
            Destroy(gameObject);
        }
    }
}
