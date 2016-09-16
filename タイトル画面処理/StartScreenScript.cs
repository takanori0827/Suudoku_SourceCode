using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//タイトル画面のUIの表示・非表示やボタン操作を制御
public class StartScreenScript : MonoBehaviour {
	//for分
	int i = 0;

	//1frameの時間加算用
	float time = 0.0f;

	//制御用
	bool Trigger = true;
	bool SettingTrigger = true;
	static bool ToggleTrigger = true;

	//GameObject
	GameObject DataObj;
	GameObject SoundObj;
	GameObject AudioObj;

	//CanvasGameObject
	public GameObject SpaceKeyObj;
	public GameObject SpaceKeyTextObj;
	public GameObject ModeSelectObj;
	public GameObject SpectialSelect;
	public GameObject TimeSelect;
	public GameObject BreakSelect;
	public GameObject VerifySelect;
	public GameObject SettingObj;

	public GameObject MainTitleObj;
	public GameObject SubTitleObj;
	public GameObject SpaceObj;
	GameObject Animals;

	//Toggle
	public Toggle QuietToggle;
	public Toggle ViolenceToggle;

	//Sprite
	public Sprite Main_Normal;
	public Sprite Main_Color;
	public Sprite Main_Animal;
	public Sprite Sub_Normal;
	public Sprite Sub_Color;
	public Sprite Sub_Animal;
	public Sprite PleaseKey_Normal;
	public Sprite PleaseKey_Color;
	public Sprite PleaseKey_Aninal;

	//SkyBox
	public Material[] skybox = new Material[3]; //0...Normal用 1...Colors用 2...Animal用

	//Audio
	public AudioClip Se_1;


	void Start () {
		//GameObjectの取得
		AudioObj = GameObject.Find ("Audio Source");
		DataObj = GameObject.Find ("DataManege");
		SoundObj = GameObject.Find ("SoundManege");

		//他の関数の参照
		DataScript ds = DataObj.GetComponent<DataScript>();
		SoundScript ss = SoundObj.GetComponent<SoundScript> ();

		//UIの非表示設定
		ModeSelectObj.GetComponent<Canvas> ().enabled = false;
		SpectialSelect.GetComponent<Canvas>().enabled = false;
		TimeSelect.GetComponent<Canvas>().enabled = false;
		BreakSelect.GetComponent<Canvas>().enabled = false;
		VerifySelect.GetComponent<Canvas>().enabled = false;
		SettingObj.GetComponent<Canvas> ().enabled = false;

		//アニマルモードの動物の表示・非表示処理
		for (i = 1; i <= 9; i++) {
			Animals = GameObject.Find ("Title/Animal_Image" + i);
			Animals.GetComponent<Image> ().enabled = false;
		}

	}

	void Update () {
		time += Time.deltaTime;
		if (time >= 0.01f) {
			SpaceKeyTextObj.transform.Rotate (new Vector3 (0, 1, 0), 1);
			time = 0.0f;
		}
			

		//画面上の表示,非表示の処理
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (Trigger == true) {
				SpaceKeyObj.GetComponent<Canvas> ().enabled = false;
				ModeSelectObj.GetComponent<Canvas> ().enabled = true;
				AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

				Trigger = false;
			}
		}

	}

	//以下ボタン操作関数
	public void NoTimeMode(){
		ModeSelectObj.GetComponent<Canvas> ().enabled = false;
		BreakSelect.GetComponent<Canvas> ().enabled = true;
		DataScript.GameMode = "NoTimeMode";

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void TimeMode(){
		ModeSelectObj.GetComponent<Canvas> ().enabled = false;
		TimeSelect.GetComponent<Canvas> ().enabled = true;
		DataScript.GameMode = "TimeMode";

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void SpectialModeSelect(){
		ModeSelectObj.GetComponent<Canvas> ().enabled = false;
		SpectialSelect.GetComponent<Canvas>().enabled = true;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void AccelTimeMode(){
		SpectialSelect.GetComponent<Canvas>().enabled = false;
		TimeSelect.GetComponent<Canvas>().enabled = true;
		DataScript.GameMode = "AccelMode";

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void NoMaskMode(){
		SpectialSelect.GetComponent<Canvas>().enabled = false;
		TimeSelect.GetComponent<Canvas>().enabled = true;
		DataScript.GameMode = "NoMaskMode";

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void AccelNoMaskMode(){
		SpectialSelect.GetComponent<Canvas>().enabled = false;
		TimeSelect.GetComponent<Canvas>().enabled = true;
		DataScript.GameMode = "AccelNoMaskMode";

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}


	public void TimeSelect_5m(){
		TimeSelect.GetComponent<Canvas>().enabled = false;
		BreakSelect.GetComponent<Canvas>().enabled = true;
		DataScript.GameTimeSetting = 5;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void TimeSelect_10m(){
		TimeSelect.GetComponent<Canvas>().enabled = false;
		BreakSelect.GetComponent<Canvas>().enabled = true;
		DataScript.GameTimeSetting = 10;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void BreakSelect_20Break(){
		BreakSelect.GetComponent<Canvas>().enabled = false;
		VerifySelect.GetComponent<Canvas>().enabled = true;
		DataScript.BreakMasu = 20;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void BreakSelect_30Break(){
		BreakSelect.GetComponent<Canvas>().enabled = false;
		VerifySelect.GetComponent<Canvas>().enabled = true;
		DataScript.BreakMasu = 30;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void BreakSelect_35Break(){
		BreakSelect.GetComponent<Canvas>().enabled = false;
		VerifySelect.GetComponent<Canvas>().enabled = true;
		DataScript.BreakMasu = 35;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void Yes(){
		Application.LoadLevel ("3x3_Sudoku");

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void No(){
		ModeSelectObj.GetComponent<Canvas> ().enabled = true;
		VerifySelect.GetComponent<Canvas>().enabled = false;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void Back(){
		ModeSelectObj.GetComponent<Canvas> ().enabled = true;
		SpectialSelect.GetComponent<Canvas>().enabled = false;
		TimeSelect.GetComponent<Canvas>().enabled = false;
		BreakSelect.GetComponent<Canvas>().enabled = false;
		VerifySelect.GetComponent<Canvas>().enabled = false;

		AudioObj.GetComponent<AudioSource> ().PlayOneShot (Se_1);

	}

	public void HowToPlay(){
		Application.LoadLevel ("HowToPlay");

	}

	public void Toggle(){
		if(ToggleTrigger == false){
			QuietToggle.GetComponent<Toggle>().isOn = true;
			ViolenceToggle.GetComponent<Toggle>().isOn = false;
			ToggleTrigger = true;
			SoundScript.ToggleData = 0; //0の場合静かな音楽に変わる
			SoundScript.Trigger = true;

		}else if(ToggleTrigger == true){
			ViolenceToggle.GetComponent<Toggle>().isOn = true;
			QuietToggle.GetComponent<Toggle>().isOn = false;
			ToggleTrigger = false;
			SoundScript.ToggleData = 1; //1の場合激しい音楽に変わる
			SoundScript.Trigger = true;

		}

	}

	public void Setting(){
		if (SettingTrigger == true) {
			SettingObj.GetComponent<Canvas> ().enabled = true;
			SettingTrigger = false;

		} else {
			SettingObj.GetComponent<Canvas> ().enabled = false;
			SettingTrigger = true;

		}

	}

	public void GameMode_Normal(){
		MainTitleObj.GetComponent<Image> ().sprite = Main_Normal;
		SubTitleObj.GetComponent<Image> ().sprite = Sub_Normal;
		SpaceObj.GetComponent<Image>().sprite = PleaseKey_Normal;
		DataScript.TypeMode = "Normal";

		//アニマルモードの動物の表示・非表示処理
		for (i = 1; i <= 9; i++) {
			Animals = GameObject.Find ("Title/Animal_Image" + i);
			Animals.GetComponent<Image> ().enabled = false;
		}

		//Skyboxの変更
		RenderSettings.skybox = skybox [0];

	}

	public void GameMode_Color(){
		MainTitleObj.GetComponent<Image> ().sprite = Main_Color;
		SubTitleObj.GetComponent<Image> ().sprite = Sub_Color;
		SpaceObj.GetComponent<Image>().sprite = PleaseKey_Color;
		DataScript.TypeMode = "Color";

		//アニマルモードの動物の表示・非表示処理
		for (i = 1; i <= 9; i++) {
			Animals = GameObject.Find ("Title/Animal_Image" + i);
			Animals.GetComponent<Image> ().enabled = false;
		}

		//Skyboxの変更
		RenderSettings.skybox = skybox [1];

	}

	public void GameMode_Animal(){
		MainTitleObj.GetComponent<Image> ().sprite = Main_Animal;
		SubTitleObj.GetComponent<Image> ().sprite = Sub_Animal;
		SpaceObj.GetComponent<Image>().sprite = PleaseKey_Aninal;
		DataScript.TypeMode = "Animal";

		//アニマルモードの動物の表示・非表示処理
		for (i = 1; i <= 9; i++) {
			Animals = GameObject.Find ("Title/Animal_Image" + i);
			Animals.GetComponent<Image> ().enabled = true;
		}

		//Skyboxの変更
		RenderSettings.skybox = skybox [2];

	}

}
