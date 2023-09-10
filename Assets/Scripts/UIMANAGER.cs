using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMANAGER : MonoBehaviour
{
    public static UIMANAGER Instance;

    public Animator painelGameOver, painelWin, painelPause;
    [SerializeField]
    private Button winBtnMenu, winBtnNovamente, winBtnProximo;
    public Animator estrela1, estrela2, estrela3;
    [SerializeField]
    private Button loseBtnMenu, loseBtnNovamente;
    [SerializeField]
    private Button pauseBtn, pauseBtnPlay, pauseBtnNovamente, pauseBtnMenu, pauseBtnLoja;
    public AudioSource winSom;

    public AudioSource loseSom;
    public Text pontosTxt, bestPontoTxt;

    public Text moedasTxt;

    private Image fundoPreto;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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
        //Painel
        painelGameOver = GameObject.Find("Menu_Lose").GetComponent<Animator>();
        painelWin = GameObject.Find("Menu_Win").GetComponent<Animator>();
        painelPause = GameObject.Find("Painel_Pause").GetComponent<Animator>();
        //Btn Win
        winBtnMenu = GameObject.Find("Button_Menu").GetComponent<Button>();
        winBtnNovamente = GameObject.Find("Button_Novamente").GetComponent<Button>();
        winBtnProximo = GameObject.Find("Button_Avancar").GetComponent<Button>();
        //Estrelas
        estrela1 = GameObject.Find("Estrela1_win").GetComponent<Animator>();
        estrela2 = GameObject.Find("Estrela2_win").GetComponent<Animator>();
        estrela3 = GameObject.Find("Estrela3_win").GetComponent<Animator>();
        //Btn Lose
        loseBtnMenu = GameObject.Find("Button_Menul").GetComponent<Button>();
        loseBtnNovamente = GameObject.Find("Button_Novamentel").GetComponent<Button>();
        //Btn Puase
        pauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        pauseBtnPlay = loseBtnMenu = GameObject.Find("Play").GetComponent<Button>();
        pauseBtnNovamente = loseBtnMenu = GameObject.Find("again").GetComponent<Button>();
        pauseBtnMenu = GameObject.Find("scene").GetComponent<Button>();
        pauseBtnLoja = GameObject.Find("shop").GetComponent<Button>();
        //audio
        winSom = painelWin.GetComponent<AudioSource>();

        loseSom = painelGameOver.GetComponent<AudioSource>();
        //Pontos
        pontosTxt = GameObject.FindWithTag("pointVal").GetComponent<Text>();
        bestPontoTxt = GameObject.FindWithTag("ptBest").GetComponent<Text>();

        //Text Score
        moedasTxt = GameObject.FindWithTag("moedaTxt").GetComponent<Text>();

        //Imagem fudno preto
        fundoPreto = GameObject.FindWithTag("fundoPreto").GetComponent<Image>();

        //Eventos
        //pause
        pauseBtn.onClick.AddListener(Pausar);
        pauseBtnPlay.onClick.AddListener(PausarInvers);
        pauseBtnNovamente.onClick.AddListener(Again);
        pauseBtnMenu.onClick.AddListener(GoMenu);
    }

    void Pausar()
    {
        GAMEMANAGER.instance.pausado = true;
        Time.timeScale = 0;
        fundoPreto.enabled = true;
        painelPause.Play("MenuPauseAnim");
    }

    void PausarInvers()
    {
        GAMEMANAGER.instance.pausado = false;
        Time.timeScale = 1;
        fundoPreto.enabled = false;
        painelPause.Play("MenuPauseAnimInvers");
    }

    //Método Pause Joga Novamente
    void Again()
    {
        SceneManager.LoadScene(ONDESTOU.instance.fase);
        Time.timeScale = 1;
        GAMEMANAGER.instance.pausado = false;
    }
    //Método pause Menu
    void GoMenu()
    {
        SceneManager.LoadScene("MenuFase2");
        Time.timeScale = 1;
        GAMEMANAGER.instance.pausado = false;
    }

    void GoLoja() 
    {
        //SceneManager.LoadScene("MenuFase2");
        Time.timeScale = 1;
        GAMEMANAGER.instance.pausado = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
