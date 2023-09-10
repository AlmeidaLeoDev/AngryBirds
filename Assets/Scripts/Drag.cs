using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag; //Vai ser o seu objeto usado para referenciar seu gameObject
    public LayerMask layer; //� s� pra ter a op��o de layer no script

    [SerializeField] //Para enxergar a vari�vel no script da unity
    private bool clicked; //Para saber se clicamos no objeto ou n�o
    private Touch touch; //vari�vel necess�ria para toque

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    //Um raio � uma linha infinita com origem � dire��o
    private Ray leftCatapultRay; //Raio usado para calcular a origem e dire��o em que o p�ssaro ser� lan�ado quando solto
    private CircleCollider2D passaroCol; //Calcular o ponto final do raio do el�stico que toca o p�ssaro, para garantir que est� correto
    private Vector2 catapultToBird; //� usada para definir a dire��o do raio leftCatapultRay
    private Vector3 pointL; // ajustar a exibi��o da linha el�stica conforme o p�ssaro � puxado para tr�s

    //Spring
    private SpringJoint2D spring;
    private Vector2 prevVel; //vai ajudar na parte da velocidade
    private Rigidbody2D passaroRB;

    //Trabalhando com part�culas
    public GameObject bomb;

    //Limite do el�stico
    private Transform catapult; //Para passar dire��o
    private Ray rayToMT;

    //Rastro
    private TrailRenderer rastro;

    public Rigidbody2D CatapultRB;
    public bool estouPronto = false;

    //�udio p�ssaro
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
        drag = GetComponent<Collider2D>(); //obter o componente Collider2D do GameObject em que o script est� anexado
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
                    // Aqui, voc� pode ajustar a l�gica de movimento conforme necess�rio
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
            passaroRB.isKinematic = false; //Para garantir que o p�ssaro n�o seja mais kinematic quando voc� solta-lo
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

    //Ajuste da ponta conectada ao p�ssaro
    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position; //Terei a dire��o a ser passada a leftcatapult
        leftCatapultRay.direction = catapultToBird; //Dire��o sendo passada
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + passaroCol.radius); //magnitude retorna o comprimento do vetor

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL); 
    }

    //M�todo para garantr que vou lan�ar o objeto
    void springEffect()
    {
        if(spring != null)
        {
            if(passaroRB.isKinematic == false) 
            {
                if(prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude) //A partir do momento que puxo o p�ssaro pra tr�s tenho exito aqui
                {
                    lineFront.enabled = false; //destruir os renderes
                    lineBack.enabled = false;  //destruir os renderes
                    Destroy(spring);
                    passaroRB.velocity = prevVel; //A velocidade tem que ser ajustada para n�o gerar inconsist�nciaa
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

            //C�digo para limite do el�stico
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
            rastro.enabled = false; //Quando tu clica no mouse o rastro � desativado
            estouPronto = true;
        }
    }
    void OnMouseUp()
    {
        passaroRB.isKinematic = false;
        clicked = false;
        rastro.enabled = true; //Quando tu solta ou bot�o do mouse o rastro � ativado
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