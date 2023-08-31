using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingInfo : MonoBehaviour
{
    public Text txtCarregando;

    public void BtnClick()
    {
        StartCoroutine(LoadGameProg());
    }

    IEnumerator LoadGameProg()
    {
        // Inicia o processo de carregamento ass�ncrono da cena com �ndice 2
        AsyncOperation async = SceneManager.LoadSceneAsync(0);

        // Enquanto a opera��o ass�ncrona n�o est� conclu�da
        while (!async.isDone)
        {
            // Habilita o componente de texto para mostrar a mensagem de carregamento
            txtCarregando.enabled = true;

            // Yield null � usado para pausar a execu��o da coroutine neste ponto
            // e dar uma chance para outros processos ocorrerem
            yield return null;
        }
    }
}
