using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//問題を生成している間の待ち時間のミニゲーム
public class WaitMiniGame : MonoBehaviour {
	//2DImage モーション処理用
	int SpriteCount = 0;
	float SpriteTime = 0.0f;
	float ExplosionTime = 0.0f;
	public GameObject KnightObj;
	public GameObject Dragon1Obj;
	public GameObject Dragon2Obj;
	public GameObject Dragon3Obj;
	public GameObject HimeObj;
	public GameObject KnightExplosion;
	public GameObject DragonExplosion;
	public Sprite[] Knight = new Sprite[3];
	public Sprite[] Dragon1 = new Sprite[3];
	public Sprite[] Dragon2 = new Sprite[3];
	public Sprite[] Dragon3 = new Sprite[3];
	public Sprite[] Hime = new Sprite[3];

	//ミニゲーム 問題
	public GameObject QuestionNumberObj1;
	public GameObject QuestionNumberObj2;
	public GameObject QuestionMarkObj;
	public GameObject QuestionEqualObj;
	public GameObject AnswerNumberObj1;
	public GameObject AnswerNumberObj2;
	public GameObject AnswerButton1Obj;
	public GameObject AnswerButton2Obj;
	public GameObject AnswerButton3Obj;
	int QuestionNumber1 = 0;
	int QuestionNumber2 = 0;
	int QuestionAnswer = 0;
	int PlayerAnswerNumber1 = 0;
	int PlayerAnswerNumber2 = 0;
	int PlayerAnswerNumber3 = 0;
	int AttackAnswer = 0;
	string Mark = "";
	string Equal = "=";
	//制御
	bool CreateTrigger = false;
	bool PlayerAnswerTrigger = false;
	bool ResetTrigger = false;
	//乱数
	int SelectMark = 0;
	int WhereAnswer = 0;
	int TrueAnswer = 0;


	// Use this for initialization
	void Start () {
		KnightExplosion.SetActive (false);
		DragonExplosion.SetActive (false);
		//初期化
		CreateTrigger = true;
		PlayerAnswerTrigger = false;
		ResetTrigger = false;
	}
	
	// Update is called once per frame
	void Update () {
		//2DImageのモーション処理	
		SpriteTime += Time.deltaTime * 3.0f;
		if (SpriteTime >= 3) {
			SpriteTime = 0.0f;
		}
		KnightObj.GetComponent<Image> ().sprite = Knight [Mathf.FloorToInt (SpriteTime)];
		Dragon1Obj.GetComponent<Image> ().sprite = Dragon1 [Mathf.FloorToInt (SpriteTime)];
		Dragon2Obj.GetComponent<Image> ().sprite = Dragon2 [Mathf.FloorToInt (SpriteTime)];
		Dragon3Obj.GetComponent<Image> ().sprite = Dragon3 [Mathf.FloorToInt (SpriteTime)];
		HimeObj.GetComponent<Image> ().sprite = Hime [Mathf.FloorToInt (SpriteTime)];

		//問題を作る
		if (CreateTrigger == true) {
			StartCoroutine ("CreateMiniGameQuestion");
			CreateTrigger = false;
		}

		//答えを作る
		if (PlayerAnswerTrigger == true) {
			StartCoroutine ("PlayerAnswer");
			PlayerAnswerTrigger = false;
		}

		//問題をリセット
		if (ResetTrigger == true) {
			//2Dspriteを非表示
			ExplosionTime += Time.deltaTime;
			if (ExplosionTime > 0.5f) {
				DragonExplosion.SetActive (false);
				KnightExplosion.SetActive (false);
				//最初から処理をし直す
				CreateTrigger = true;

				ExplosionTime = 0.0f;
				ResetTrigger = false;
			}
		}

	}

	IEnumerator CreateMiniGameQuestion(){
		//問題作成
		SelectMark = UnityEngine.Random.Range (0, 4);		//0~3
		WhereAnswer = UnityEngine.Random.Range (0, 2);		//0~1
		//演算子
		if (SelectMark == 0) { 
			//2つの整数をランダムに生成
			QuestionNumber1 = UnityEngine.Random.Range (0, 10);	//0~9
			QuestionNumber2 = UnityEngine.Random.Range (0, 10);	//0~9
			Mark = "+";
			QuestionAnswer = QuestionNumber1 + QuestionNumber2;

		} else if (SelectMark == 1) {
			do {
				QuestionNumber1 = UnityEngine.Random.Range (0, 10);	//0~9
				QuestionNumber2 = UnityEngine.Random.Range (0, 10);	//0~9
				yield return null;
			} while(QuestionNumber2 > QuestionNumber1);
			Mark = "-";
			QuestionAnswer = QuestionNumber1 - QuestionNumber2;

		} else if (SelectMark == 2) {
			QuestionNumber1 = UnityEngine.Random.Range (0, 10);	//0~9
			QuestionNumber2 = UnityEngine.Random.Range (0, 10);	//0~9
			Mark = "*";
			QuestionAnswer = QuestionNumber1 * QuestionNumber2;

		} else if (SelectMark == 3) {
			do {
				QuestionNumber1 = UnityEngine.Random.Range (0, 10);	//0~9
				QuestionNumber2 = UnityEngine.Random.Range (0, 10);	//0~9
				yield return null;
			} while(QuestionNumber1 == 0 || QuestionNumber2 == 0);
			Mark = "/";
			QuestionAnswer = QuestionNumber1 / QuestionNumber2;

		}

		//テキスト表示
		QuestionNumberObj1.GetComponent<Text> ().text = QuestionNumber1.ToString ();
		QuestionNumberObj2.GetComponent<Text> ().text = QuestionNumber2.ToString ();
		QuestionMarkObj.GetComponent<Text> ().text = Mark;
		QuestionEqualObj.GetComponent<Text> ().text = Equal;

		//一桁目か二桁目のどちらを問題にするか
		if (WhereAnswer == 0) {
			AnswerNumberObj1.GetComponent<Text> ().text = (QuestionAnswer / 10).ToString ();
			AnswerNumberObj2.GetComponent<Text> ().text = "?";
		} else if (WhereAnswer == 1) {
			AnswerNumberObj1.GetComponent<Text> ().text = "?";
			AnswerNumberObj2.GetComponent<Text> ().text = (QuestionAnswer % 10).ToString ();
		}

		//次の処理へ
		PlayerAnswerTrigger = true;
	}

	IEnumerator PlayerAnswer ()	{
		//どのボタンを正解にするかを判定
		TrueAnswer = UnityEngine.Random.Range (0, 3); //0~2
		if (TrueAnswer == 0) {
			//問題の答えがどの部分なのか
			if (WhereAnswer == 0) {
				do {
					//正解とフェイクを2つ設定
					PlayerAnswerNumber1 = (QuestionAnswer % 10);
					PlayerAnswerNumber2 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber3 = UnityEngine.Random.Range (0, 10); //0~10
					yield return null;
					//全て違う数字にする
				} while((QuestionAnswer % 10) == PlayerAnswerNumber2 || (QuestionAnswer % 10) == PlayerAnswerNumber3 || PlayerAnswerNumber2 == PlayerAnswerNumber3);
				AnswerButton1Obj.GetComponent<Text> ().text = (QuestionAnswer % 10).ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();
			} else if (WhereAnswer == 1) {
				do {
					PlayerAnswerNumber1 = (QuestionAnswer / 10);
					PlayerAnswerNumber2 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber3 = UnityEngine.Random.Range (0, 10); //0~10
					yield return null;
				} while((QuestionAnswer / 10) == PlayerAnswerNumber2 || (QuestionAnswer / 10) == PlayerAnswerNumber3 || PlayerAnswerNumber2 == PlayerAnswerNumber3);
				AnswerButton1Obj.GetComponent<Text> ().text = (QuestionAnswer / 10).ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();
			}

		} else if (TrueAnswer == 1) {
			if (WhereAnswer == 0) {
				do {
					PlayerAnswerNumber1 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber2 = (QuestionAnswer % 10);
					PlayerAnswerNumber3 = UnityEngine.Random.Range (0, 10); //0~10
					yield return null;
				} while((QuestionAnswer % 10) == PlayerAnswerNumber1 || (QuestionAnswer % 10) == PlayerAnswerNumber3 || PlayerAnswerNumber1 == PlayerAnswerNumber3);
				AnswerButton1Obj.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = (QuestionAnswer % 10).ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();
			} else if (WhereAnswer == 1) {
				do {
					PlayerAnswerNumber1 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber2 = (QuestionAnswer / 10);
					PlayerAnswerNumber3 = UnityEngine.Random.Range (0, 10); //0~10
					yield return null;
				} while((QuestionAnswer / 10) == PlayerAnswerNumber1 || (QuestionAnswer / 10) == PlayerAnswerNumber3 || PlayerAnswerNumber1 == PlayerAnswerNumber3);
				AnswerButton1Obj.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = (QuestionAnswer / 10).ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();
			}

		} else if (TrueAnswer == 2) {
			if (WhereAnswer == 0) {
				do {
					PlayerAnswerNumber1 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber2 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber3 = (QuestionAnswer % 10);
					yield return null;
				} while((QuestionAnswer % 10) == PlayerAnswerNumber1 || (QuestionAnswer % 10) == PlayerAnswerNumber2 || PlayerAnswerNumber1 == PlayerAnswerNumber2);
				AnswerButton1Obj.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = (QuestionAnswer % 10).ToString ();
			} else if (WhereAnswer == 1) {
				do {
					PlayerAnswerNumber1 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber2 = UnityEngine.Random.Range (0, 10); //0~10
					PlayerAnswerNumber3 = (QuestionAnswer / 10);
					yield return null;
				} while((QuestionAnswer / 10) == PlayerAnswerNumber1 || (QuestionAnswer / 10) == PlayerAnswerNumber2 || PlayerAnswerNumber1 == PlayerAnswerNumber2);
				AnswerButton1Obj.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();
				AnswerButton2Obj.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();
				AnswerButton3Obj.GetComponent<Text> ().text = (QuestionAnswer / 10).ToString ();
			}
		}

	}

	//ボタン制御
	public void PlayerAnswerButton1(){
		//答えの場所にユーザの答えを表示
		if (WhereAnswer == 0) {
			AnswerNumberObj2.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();

		} else if (WhereAnswer == 1) {
			AnswerNumberObj1.GetComponent<Text> ().text = PlayerAnswerNumber1.ToString ();
		}
		AttackAnswer = PlayerAnswerNumber1;
	}

	public void PlayerAnswerButton2(){
		if (WhereAnswer == 0) {
			AnswerNumberObj2.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();

		} else if (WhereAnswer == 1) {
			AnswerNumberObj1.GetComponent<Text> ().text = PlayerAnswerNumber2.ToString ();
		}
		AttackAnswer = PlayerAnswerNumber2;
	}

	public void PlayerAnswerButton3(){
		if (WhereAnswer == 0) {
			AnswerNumberObj2.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();

		} else if (WhereAnswer == 1) {
			AnswerNumberObj1.GetComponent<Text> ().text = PlayerAnswerNumber3.ToString ();
		}
		AttackAnswer = PlayerAnswerNumber3;
	}

	//答え合わせ
	public void AnswerAttack(){
		if (WhereAnswer == 0) {
			if (AttackAnswer == (QuestionAnswer % 10)) {
				//答えが合っていればドラゴンが爆発
				DragonExplosion.SetActive (true);
			} else {
				//不正解であればナイトが爆発
				KnightExplosion.SetActive (true);
			}

		} else if (WhereAnswer == 1) {
			if (AttackAnswer == (QuestionAnswer / 10)) {
				DragonExplosion.SetActive (true);
			} else {
				KnightExplosion.SetActive (true);
			}

		}
		//リセット
		ResetTrigger = true;
	}

}
