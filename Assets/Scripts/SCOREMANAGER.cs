using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SCOREMANAGER : MonoBehaviour
{

    public static SCOREMANAGER instance;

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
    }

    public void SalvarDados(int moeda)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/dadoscoinData.data");

        SaveDados coin = new SaveDados();
        coin.moedas = moeda;

        bf.Serialize(fs, coin);
        fs.Close();
    }

    public int LoadDados()
    {
        int moeda = 0;

        if (File.Exists(Application.persistentDataPath + "/dadoscoinData.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/dadoscoinData.data", FileMode.Open);

            SaveDados coin = (SaveDados)bf.Deserialize(fs);
            fs.Close();

            moeda = (int)coin.moedas;
        }

        return moeda;
    }

    public void PerdeMoedas(int moeda)
    {
        int tempMoeda, novoVal;

        tempMoeda = LoadDados();

        novoVal = tempMoeda - moeda;

        SalvarDados(novoVal);
    }


    [Serializable]
    class SaveDados
    {
        public int moedas;
    }
}
