using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag; //Vai ser o seu objeto usado para referenciar seu gameObject
    public LayerMask layer; //É só pra ter a opção de layer no script
    
    [SerializeField] //Para enxergar a variável no script da unity
    private bool clicked; //Para saber se clicamos no objeto ou não
    private Touch touch; //variável necessária para toque

    // Start is called before the first frame update
    void Start()
    {
        drag = GetComponent<Collider2D>(); //obter o componente Collider2D do GameObject em que o script está anexado
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // "wp" se refere ao "ScreenToWorldPoint" que transforma o espaço da tela no espaço do mundo, para termos o raio lazer disparado
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position); 
            //Lança o raio para verificar se colide com o objeto
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layer.value); //Origem, direção, distância, layer

            if(hit.collider != null ) // Para saber se estou clicando no objeto
            {
                clicked = true;
            }
            if(clicked) //Se eu cliquei no objeto 
            {
                if(touch.phase == UnityEngine.TouchPhase.Stationary || touch.phase == UnityEngine.TouchPhase.Moved) 
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    transform.position = tPos;
                }
            }
            if(touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
            {
                clicked = false;
            }
        }
    }
}
