using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SalvaDados : MonoBehaviour
{
    //#4
    public Vector3 temp; //usada para armazenar a posição atual do objeto quando o jogo começa

    void Start()
    {
        //#5  atribui a posição atual do objeto à variável temp quando o jogo começa
        temp = transform.position;
    }

    void Update()
    {
        //#3
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Salvar();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadPos();
        }
    }

    void Salvar() //#2
    {
        BinaryFormatter bf = new BinaryFormatter(); //usada para serializar os dados em formato binário
        //cria um arquivo no diretório persistente de dados do aplicativo Unity. O nome do arquivo é "posData"
        FileStream fs = File.Create(Application.persistentDataPath + "/posData");

        SaveClass s = new SaveClass();
        s.posx = transform.position.x; //Quero salvar a posição do meu GameObject

        bf.Serialize(fs, s);
        fs.Close();
    }

    void LoadPos() //#6
    {
        if(File.Exists(Application.persistentDataPath + "/posData"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/posData", FileMode.Open);

            SaveClass s = (SaveClass)bf.Deserialize(fs);
            fs.Close();

            temp.x = s.posx;
            transform.position = temp;
        }
        else
        {
            print("Não Existe");
        }
    }
}

[Serializable] //#1
class SaveClass
{
    public float posx;
}