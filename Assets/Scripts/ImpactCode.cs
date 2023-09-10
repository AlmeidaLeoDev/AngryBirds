using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCode : MonoBehaviour
{
    private int limite;
    private SpriteRenderer spriteR;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject bomb, pontos1000 ;
    private AudioSource audioObj;
    [SerializeField]
    private AudioClip[] clips;

    void Start()
    {
        limite = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];  //Primeira imagem do spriteR

        audioObj = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //relativeVelocity = velocidade linear relativa de dois objetos colidindo
        if(collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 10) //Verificar força de colisão
        {
            if(limite < sprites.Length - 1) //Se limite é menor do que o indice máximo permitio no array "sprites"
            {
                limite++; //limite é incrementado
                spriteR.sprite = sprites[limite]; //o sprite atual é trocado pelo próximo do array

                audioObj.clip = clips[0];
                audioObj.Play();
            }
            else if(limite ==  sprites.Length - 1)
            {
                Instantiate(pontos1000, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                Destroy(gameObject, 1);

                audioObj.clip = clips[1];
                audioObj.Play();
                GAMEMANAGER.instance.pontosGame += 1000;
                UIMANAGER.Instance.pontosTxt.text = GAMEMANAGER.instance.pontosGame.ToString();
            }
        }
        else if (collision.relativeVelocity.magnitude > 12 && collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pontos1000, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject, 1) ;

            audioObj.clip = clips[1];
            audioObj.Play();

            GAMEMANAGER.instance.pontosGame += 1000;
            UIMANAGER.Instance.pontosTxt.text = GAMEMANAGER.instance.pontosGame.ToString();
        }
    }
}
