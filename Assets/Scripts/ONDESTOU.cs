using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ONDESTOU : MonoBehaviour
{
    public static ONDESTOU instance;

    public int fase = -1;
    public string faseN;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += VerificaFasee;
    }

    void VerificaFasee(Scene cena, LoadSceneMode modo)
    {
        fase = SceneManager.GetActiveScene().buildIndex;
        faseN = SceneManager.GetActiveScene().name;
    }
}
