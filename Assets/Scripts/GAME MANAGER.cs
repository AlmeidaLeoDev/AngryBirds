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

    void Awake()
    {
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
        jogoComecou = false;
    }

    private void StartGame() 
    {
        jogoComecou = true;
        passarosEmCena = 0;
        win = false;
    }

    void Update()
    {
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
