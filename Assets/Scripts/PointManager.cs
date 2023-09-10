using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POINTMANAGER : MonoBehaviour {

	public static POINTMANAGER instance;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		else
		{
			Destroy (gameObject);
		}
	}


	public void MelhorPontuacaoSave(string level,int pt)
	{
		if (!ZPlayerPrefs.HasKey (level + "best")) {
            ZPlayerPrefs.GetInt(level + "best" + pt);
		} else {
			if(GAMEMANAGER.instance.pontosGame > ZPlayerPrefs.GetInt(level + "best"))
			{
				ZPlayerPrefs.GetInt (level + "best" + GAMEMANAGER.instance.pontosGame);
			}
		}
	}

	public int MelhorPontuacaoLoad(string level)
	{
		if (ZPlayerPrefs.HasKey (level + "best")) {
			return ZPlayerPrefs.GetInt (level + "best");
		} else {
			return 0;
		}
	}
}
