using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMEMANAGER : MonoBehaviour
{
    public static GAMEMANAGER instance;
    public GameObject[] passaro;
    public int passarosNum;
    public int passarosEmCena = 0;
    public Transform pos;
    public bool win;
    public bool jogoComecou;
    public string nomePassaro;

    public bool passaroLancado = false;
    public Transform objE, objD;

    public int numPorcosCena;
    private bool tocaWin = false, tocaLose = false;

    public bool estrela1Fim, estrela2Fim;
    public int aux;

    public int estrelasNum;
    public bool trava = false;

    void Awake()
    {
        ZPlayerPrefs.Initialize("12345678", "pombobravogame");

        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += Carrega;
    }

    void Carrega(Scene cena, LoadSceneMode modo)
    {
        pos = GameObject.FindWithTag("pos").GetComponent<Transform>();
        objE = GameObject.FindWithTag("PE").GetComponent<Transform>();
        objD = GameObject.FindWithTag("PD").GetComponent<Transform>();

        StartGame ();

        // Passaro Pos
        passarosNum = GameObject.FindGameObjectsWithTag("Player").Length;
        passaro = new GameObject[passarosNum];

        for (int x = 0; x < GameObject.FindGameObjectsWithTag("Player").Length; x++)
        {
            passaro[x] = GameObject.Find("Vulture" + x);
        }

        numPorcosCena = GameObject.FindGameObjectsWithTag("porco").Length;
        aux = passarosNum;
    }

    void NascPassaro()
    {
        if (passarosEmCena == 0 && passarosNum > 0)
        {
            for (int x = 0; x < passaro.Length; x++)
            {
                if (passaro[x] != null)
                {
                    if (passaro[x].transform.position != pos.position && passarosEmCena == 0)
                    {
                        nomePassaro = passaro[x].name;
                        passaro[x].transform.position = pos.position;
                        passarosEmCena = 1;
                    }
                }
            }
        }
    }

    void GameOver()
    {
        jogoComecou = false;
    }

    private void WinGame() 
    {
        if(jogoComecou != false)
        {
            jogoComecou = false;
            UIMANAGER.Instance.painelWin.Play("MenuWinAnimado");

            if (!UIMANAGER.Instance.winSom.isPlaying && tocaWin == false)
            {
                UIMANAGER.Instance.winSom.Play();
                tocaWin = true;
            }
        }

        if(tocaWin && !UIMANAGER.Instance.winSom.isPlaying && trava == false)
        {
            if(passarosNum == aux - 1)
            {
                UIMANAGER.Instance.estrela1.Play("Estrela1_animada");

                if (estrela1Fim)
                {
                    UIMANAGER.Instance.estrela2.Play("Estrela2_animada");

                    if (estrela2Fim)
                    {
                        UIMANAGER.Instance.estrela3.Play("Estrela3_animada");
                        trava = true;
                    }
                }
                estrelasNum = 3;
            }
            else if (passarosNum == aux - 2)
            {
                UIMANAGER.Instance.estrela1.Play("Estrela1_animada");

                if (estrela1Fim)
                {
                    UIMANAGER.Instance.estrela2.Play("Estrela2_animada");
                    trava = true;
                }
                estrelasNum = 2;
            }
            else if (passarosNum <= aux - 3)
            {
                UIMANAGER.Instance.estrela1.Play("Estrela1_animada");
                estrelasNum = 1;
                trava = true;
            }
            else
            {
                estrelasNum = 0;
                trava = true;
            }

            if (!ZPlayerPrefs.HasKey("Level" + ONDESTOU.instance.fase + "estrelas"))
            {
                ZPlayerPrefs.SetInt("Level" + ONDESTOU.instance.fase + "estrelas", estrelasNum);
            }
            else
            {
                if(ZPlayerPrefs.GetInt("Level" + ONDESTOU.instance.fase + "estrelas") < estrelasNum)
                {
                    ZPlayerPrefs.SetInt("Level" + ONDESTOU.instance.fase + "estrelas", estrelasNum);
                }
            }
        }
    }

    private void StartGame() 
    {
        jogoComecou = true;
        passarosEmCena = 0;
        win = false;
    }

    private void Start()
    {
        ZPlayerPrefs.DeleteAll();
    }

    void Update()
    {
        if(numPorcosCena <= 0)
        {
            win = true;
        }

        if (win)
        {
            WinGame();
        }
        else
        {
            NascPassaro();
        }
    }
}
