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
    public bool lose;

    public bool estrela1Fim, estrela2Fim;
    public int aux;

    public int estrelasNum;
    public bool trava = false;

    public int pontosGame, bestPontoGame;
    public int moedasGame;

    public bool pausado = false;

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

        UIMANAGER.Instance.painelGameOver.Play("MenuLoseAnimado");
        if (!UIMANAGER.Instance.loseSom.isPlaying && tocaLose == false)
        {
            UIMANAGER.Instance.loseSom.Play();
            tocaLose = true;
        }
    }

    private void WinGame() 
    {
        SCOREMANAGER.instance.SalvarDados(moedasGame);

        if(jogoComecou != false)
        {
            jogoComecou = false;
            UIMANAGER.Instance.painelWin.Play("MenuWinAnimado");

            if (!UIMANAGER.Instance.winSom.isPlaying && tocaWin == false)
            {
                UIMANAGER.Instance.winSom.Play();
                tocaWin = true;
            }

            //Pontos
            POINTMANAGER.instance.MelhorPontuacaoSave(ONDESTOU.instance.faseN, pontosGame);
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
        lose = false;
        trava = false;

        pontosGame = 0;
        bestPontoGame = POINTMANAGER.instance.MelhorPontuacaoLoad(ONDESTOU.instance.faseN);

        UIMANAGER.Instance.pontosTxt.text = pontosGame.ToString();
        UIMANAGER.Instance.bestPontoTxt.text = bestPontoGame.ToString();
        
        moedasGame = SCOREMANAGER.instance.LoadDados();
        UIMANAGER.Instance.moedasTxt.text = SCOREMANAGER.instance.LoadDados().ToString();   
    }

    private void Start()
    {
        StartGame();
        ZPlayerPrefs.DeleteAll();
    }

    void Update()
    {
        if(numPorcosCena <= 0)
        {
            win = true;
        }
        else if(numPorcosCena > 0 && passarosNum <= 0)
        {
            lose = true;
        }

        if (win)
        {
            WinGame();
        }
        else if(lose)
        {
            GameOver();
        }

        if (jogoComecou)
        {
            NascPassaro();
        }
    }
}
