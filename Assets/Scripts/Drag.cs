using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag; //Vai ser o seu objeto usado para referenciar seu gameObject
    public LayerMask layer; //É só pra ter a opção de layer no script
    [SerializeField] //Para enxergar a variável no script da unity
    private bool clicked; //Para saber se clicamos no objeto ou não
    private Touch touch; //variável necessária para toque

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    //Um raio é uma linha infinita com origem é direção
    private Ray leftCatapultRay; //Raio usado para calcular a origem e direção em que o pássaro será lançado quando solto
    private CircleCollider2D passaroCol; //Calcular o ponto final do raio do elástico que toca o pássaro, para garantir que está correto
    private Vector2 catapultToBird; //É usada para definir a direção do raio leftCatapultRay
    private Vector3 pointL; // ajustar a exibição da linha elástica conforme o pássaro é puxado para trás

    //Spring
    private SpringJoint2D spring;
    private Vector2 prevVel; //vai ajudar na parte da velocidade
    private Rigidbody2D passaroRB;

    //Trabalhando com partículas
    public GameObject bomb;

    //Limite do elástico
    private Transform catapult; //Para passar direção
    private Ray rayToMT;

    //Rastro
    private TrailRenderer rastro;

    void Start()
    {
        drag = GetComponent<Collider2D>(); //obter o componente Collider2D do GameObject em que o script está anexado
        SetupLine();

        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero); //Para calcular origem e direção
        passaroCol = GetComponent<CircleCollider2D>(); //Para obter o component de colisão
        
        //Pegando componentes para inicializar as variáveis usadas em spring
        spring = GetComponent<SpringJoint2D>();
        passaroRB = GetComponent<Rigidbody2D>();

        //Passando valores para minhas variáveis do limite do elástico
        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero); //Origem e direção

        //Rastro
        rastro = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LineUpdate();
        springEffect();
        prevVel = passaroRB.velocity;

        #if UNITY_ANDROID 

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // "wp" se refere ao "ScreenToWorldPoint" que transforma o espaço da tela no espaço do mundo, para termos o raio lazer disparado
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //Lança o raio para verificar se colide com o objeto
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layer.value); //Origem, direção, distância, layer

            if (hit.collider != null) // Para saber se estou clicando no objeto
            {
                clicked = true;
            }
            if (clicked) //Se eu cliquei no objeto 
            {
                if (touch.phase == UnityEngine.TouchPhase.Stationary || touch.phase == UnityEngine.TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                    //Código para limite do elástico
                    catapultToBird = tPos - catapult.position;

                    if (catapultToBird.sqrMagnitude > 9f)
                    {
                        rayToMT.direction = catapultToBird;
                        tPos = rayToMT.GetPoint(3f);
                    }

                    transform.position = tPos;
                }
            }
            if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
            {
                passaroRB.isKinematic = false; //Para garantir que o pássaro não seja mais kinematic quando você solta-lo
                clicked = false;
                MataPassaro();
            }
        }

        #endif

        #if UNITY_EDITOR //Me permite trabalhar com as funcionalidade diretamente na cena, sem precisar simular

        if (clicked)
        {
            Dragging();
        }

        #endif

        print (passaroRB.velocity.magnitude);

        if(clicked == false && passaroRB.isKinematic == false)
        {
            MataPassaro();
        }
    }



    void SetupLine() //Ajuste da ponta conectada ao arco
    {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);
    }

    //Ajuste da ponta conectada ao pássaro
    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position; //Terei a direção a ser passada a leftcatapult
        leftCatapultRay.direction = catapultToBird; //Direção sendo passada
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + passaroCol.radius); //magnitude retorna o comprimento do vetor

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL); 
    }

    //Método para garantr que vou lançar o objeto
    void springEffect()
    {
        if(spring != null)
        {
            if(passaroRB.isKinematic == false) 
            {
                if(prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude) //A partir do momento que puxo o pássaro pra trás tenho exito aqui
                {
                    lineFront.enabled = false; //destruir os renderes
                    lineBack.enabled = false;  //destruir os renderes
                    Destroy(spring);
                    passaroRB.velocity = prevVel; //A velocidade tem que ser ajustada para não gerar inconsistênciaa
                }
            }
        }
    }

    void MataPassaro()
    {
        if (passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping())
        {
            StartCoroutine(TempoMorte());
        }
    }

    IEnumerator TempoMorte()
    {
        yield return new WaitForSeconds(1);
        Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
    }


    //MOUSE
    void Dragging()
    {
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWP.z = 0f;

        //Código para limite do elástico
        catapultToBird = mouseWP - catapult.position;

        if(catapultToBird.sqrMagnitude > 9f)
        {
            rayToMT.direction = catapultToBird;
            mouseWP = rayToMT.GetPoint(3f);
        }

        transform.position = mouseWP;
    }
    void OnMouseDown()
    {
        clicked = true;
        rastro.enabled = false; //Quando tu clica no mouse o rastro é desativado
    }
    void OnMouseUp()
    {
        passaroRB.isKinematic = false;
        clicked = false;
        rastro.enabled = true; //Quando tu solta ou botão do mouse o rastro é ativado
    }
}