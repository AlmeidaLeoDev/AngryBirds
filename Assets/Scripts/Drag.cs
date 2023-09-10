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

    public Rigidbody2D CatapultRB;
    public bool estouPronto = false;

    //Áudio pássaro
    public AudioSource audioPassaro;
    public GameObject audioMortePassaro;

    private void Awake()
    {
        /*
        lineFront = (LineRenderer)GameObject.FindWithTag("LF").GetComponent<LineRenderer>();
        lineBack = (LineRenderer)GameObject.FindWithTag("LB").GetComponent<LineRenderer>();
        CatapultRB = GameObject.FindWithTag("LB").GetComponent<Rigidbody2D>();
        spring.connectedBody = CatapultRB;
        */

        spring = GetComponent<SpringJoint2D>();
        spring.connectedBody = CatapultRB;
        drag = GetComponent<Collider2D>(); //obter o componente Collider2D do GameObject em que o script está anexado
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero); 
        passaroCol = GetComponent<CircleCollider2D>(); 
        passaroRB = GetComponent<Rigidbody2D>();
        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero); 
        rastro = GetComponentInChildren<TrailRenderer>();

        audioPassaro = GetComponent<AudioSource>();
    }

    void Start()
    {
        SetupLine();
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

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layer.value);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    clicked = true;
                }
                else
                {
                    clicked = false;
                }
            }

            if (clicked)
            {
                if (touch.phase == UnityEngine.TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    // Aqui, você pode ajustar a lógica de movimento conforme necessário
                    transform.position = tPos;
                }
            }

            if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
            {
                clicked = false;
            }
        }


        /*if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
        {
            passaroRB.isKinematic = false; //Para garantir que o pássaro não seja mais kinematic quando você solta-lo
            clicked = false;
            MataPassaro();
        }*/


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

        if (passaroRB.isKinematic == false)
        {
            Vector3 posCam = Camera.main.transform.position;
            posCam.x = transform.position.x;
            posCam.x = Mathf.Clamp(posCam.x, GAMEMANAGER.instance.objE.position.x, GAMEMANAGER.instance.objD.position.x);
            Camera.main.transform.position = posCam;
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
        GAMEMANAGER.instance.passarosNum -= 1;
        GAMEMANAGER.instance.passarosEmCena = 0;
        GAMEMANAGER.instance.passaroLancado = false;
        estouPronto = false;

        Instantiate(audioMortePassaro, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

    }


    //MOUSE
    void Dragging()
    {
        if (passaroRB.isKinematic)
        {
            Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWP.z = 0f;

            //Código para limite do elástico
            catapultToBird = mouseWP - catapult.position;

            if (catapultToBird.sqrMagnitude > 9f)
            {
                rayToMT.direction = catapultToBird;
                mouseWP = rayToMT.GetPoint(3f);
            }
            transform.position = mouseWP;
        }
    }

    
    void OnMouseDown()
    {
        if (GAMEMANAGER.instance.pausado == false)
        {
            clicked = true;
            rastro.enabled = false; //Quando tu clica no mouse o rastro é desativado
            estouPronto = true;
        }
    }
    void OnMouseUp()
    {
        passaroRB.isKinematic = false;
        clicked = false;
        rastro.enabled = true; //Quando tu solta ou botão do mouse o rastro é ativado
        GAMEMANAGER.instance.passaroLancado = true;

        audioPassaro.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("moedasTag"))
        {
            GAMEMANAGER.instance.moedasGame += 50;
            
            UIMANAGER.Instance.moedasTxt.text = GAMEMANAGER.instance.moedasGame.ToString(); 
            Destroy(collision.gameObject );
        }
    }
}