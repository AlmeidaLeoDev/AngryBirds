using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManagerPB : MonoBehaviour {

	public static LevelManagerPB instance;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}

	}

	// Use this for initialization
	void Start () 
	{
		ListaAdd ();
	}

	// Update is called once per frame
	void Update () 
	{

	}

	[System.Serializable]
	public class Level
	{
		public string levelText;
		public bool habilitado;
		public int desbloqueado;
		public bool txtAtivo;
	}

	public GameObject botao;
	public Transform localBtn;
	public List<Level> levelList;


	void ListaAdd()
	{
		foreach(Level level in levelList)
		{
			GameObject btnNovo = Instantiate (botao) as GameObject;
			botaoLevel btnNew = btnNovo.GetComponent<botaoLevel> (); 
			btnNew.levelTxtBTN.text = level.levelText;


			if(PlayerPrefs.GetInt("Level"+btnNew.levelTxtBTN.text)==1)
			{
				level.desbloqueado = 1;
				level.habilitado = true;
				level.txtAtivo = true;
			}

			btnNew.desbloqueadoBTN = level.desbloqueado;
			btnNew.GetComponent<Button> ().interactable = level.habilitado;
			btnNew.GetComponentInChildren<Text> ().enabled = level.txtAtivo;
			btnNew.GetComponent<Button> ().onClick.AddListener (() => ClickLevel ("Level" + btnNew.levelTxtBTN.text));

			btnNew.estrela1.enabled = true;
			btnNew.estrela2.enabled = true;
			btnNew.estrela3.enabled = true;
	
			btnNovo.transform.SetParent (localBtn,false);
		}
	}
    void ClickLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
















