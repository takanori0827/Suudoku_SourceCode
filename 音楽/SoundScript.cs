using UnityEngine;
using System.Collections;

//サウンド制御
public class SoundScript : MonoBehaviour {
	public static int ToggleData = 0;
	public static bool Trigger = false;
	public string Scene;

	GameObject AudioObj; 
	GameObject DataObj;

	public AudioClip Quiet_Title_Music;
	public AudioClip Quiet_Game_Music;
	public AudioClip Violence_Title_Music;
	public AudioClip Violence_Game_Music;


	void Start () {
		AudioObj = GameObject.Find ("Audio Source");
		DataObj = GameObject.Find ("DataManege");
		DataScript ds = DataObj.GetComponent<DataScript> ();

		Trigger = true;

	}

	void Update () {
		//全体のサウンド処理...0・・・静かな音楽,1・・・激しい音楽
		if (Trigger == true) {
			//静かな音楽
			if (ToggleData == 0) {
				if (Scene == "タイトル") {
					AudioObj.GetComponent<AudioSource> ().Stop ();

					AudioObj.GetComponent<AudioSource> ().clip = Quiet_Title_Music;
					AudioObj.GetComponent<AudioSource> ().loop = true;
					AudioObj.GetComponent<AudioSource> ().volume = 0.1f;
					AudioObj.GetComponent<AudioSource> ().Play ();

				} else if (Scene == "ゲームモード") {
					AudioObj.GetComponent<AudioSource> ().Stop ();

					AudioObj.GetComponent<AudioSource> ().clip = Quiet_Game_Music;
					AudioObj.GetComponent<AudioSource> ().loop = true;
					AudioObj.GetComponent<AudioSource> ().volume = 0.1f;
					AudioObj.GetComponent<AudioSource> ().Play ();

				}

			//激しい音楽
			} else if (ToggleData == 1) {
				if (Scene == "タイトル") {
					AudioObj.GetComponent<AudioSource> ().Stop ();

					AudioObj.GetComponent<AudioSource> ().clip = Violence_Title_Music;
					AudioObj.GetComponent<AudioSource> ().loop = true;
					AudioObj.GetComponent<AudioSource> ().volume = 0.1f;
					AudioObj.GetComponent<AudioSource> ().Play ();

				} else if (Scene == "ゲームモード") {
					AudioObj.GetComponent<AudioSource> ().Stop ();

					AudioObj.GetComponent<AudioSource> ().clip = Violence_Game_Music;
					AudioObj.GetComponent<AudioSource> ().loop = true;
					AudioObj.GetComponent<AudioSource> ().volume = 0.1f;
					AudioObj.GetComponent<AudioSource> ().Play ();

				}
			}
			Trigger = false;
		}
	}

}
