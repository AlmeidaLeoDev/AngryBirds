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
        // Inicia o processo de carregamento assíncrono da cena com índice 2
        AsyncOperation async = SceneManager.LoadSceneAsync(0);

        // Enquanto a operação assíncrona não está concluída
        while (!async.isDone)
        {
            // Habilita o componente de texto para mostrar a mensagem de carregamento
            txtCarregando.enabled = true;

            // Yield null é usado para pausar a execução da coroutine neste ponto
            // e dar uma chance para outros processos ocorrerem
            yield return null;
        }
    }
}
