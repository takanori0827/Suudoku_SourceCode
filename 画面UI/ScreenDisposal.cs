using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//ゲーム画面UIの表示・非表示や問題の解答,ゲームモード処理,UI上の秒数カウントの制御
public class ScreenDisposal : MonoBehaviour {
	//for文用
	int i = 0;
	int k = 0;
	int j = 0;
	int Count = 0;

	int Text_Number = 0;

	//時間変動用
	int m = 0;

	//ベストタイム保存用
	public static float BestTime;
	public static float ReceiveBestTime;

	//タイムリミット制御用
	public int TimeCount = 0;

	//time.deltatime処理用
	float Oftime = 0.0f;
	float Starttime = -1.0f;
	float Countdown = 0.0f;
	float AccelTime = 0.0f;
	float AccelCount = 1.0f;
	float GameStartTime = 0.0f;

	//ゲームモードの設定
	public string Mode;
	public static string StaticMode;

	//各Object格納用
	GameObject ScreenText;
	GameObject ScreenText2;
	GameObject NumberButton;
	GameObject NumberObj;
	GameObject ClearObj;
	GameObject EndObj;
	GameObject DataObj;

	//Text格納用
	public Text TimeText;
	public Text StartTimeText;
	public Text GameTimeText;
	public Text InfoText;
	public Text ClearTimeText;

	//フラグ処理用
	bool StartCountDownTrigger = false;
	bool TimeOverTrigger = true;
	bool AfterTimeOverTrigger = false;
	bool TimeTrigger = true;
	public static bool AccelTrigger = false;

	public Sprite[] Animal = new Sprite[9];
	public Material[] skybox = new Material[3]; //0...Normal用 1...Colors用 2...Animal用


	void Start () {
		//GameObjectの取得
		ScreenText = GameObject.Find ("TopScreen");
		ScreenText2 = GameObject.Find ("TopScreen2");
		ClearObj = GameObject.Find ("ClearImage");
		EndObj = GameObject.Find ("エンドボタン");
		DataObj = GameObject.Find ("DataManege");

		//他オブジェクトのスクリプトとと紐づけ
		Suudoku suudoku = this.gameObject.GetComponent<Suudoku> ();
		ButtonScript bs = this.gameObject.GetComponent<ButtonScript> ();
		DataScript ds = DataObj.gameObject.GetComponent<DataScript> ();

		//初期化
		StartCountDownTrigger = false;
		TimeOverTrigger = true;
		AfterTimeOverTrigger = false;
		TimeTrigger = true;
		InfoText.text = "　";

		//ゲームタイプで背景を変更
		if (DataScript.TypeMode == "Normal") {
			RenderSettings.skybox = skybox [0];
		} else if (DataScript.TypeMode == "Color") {
			RenderSettings.skybox = skybox [1];
		} else if (DataScript.TypeMode == "Animal") {
			RenderSettings.skybox = skybox [2];
		}

		//Canvasの非表示
		ClearObj.SetActive (false);

		//制限時間とゲームモードを設定
		TimeCount = DataScript.GameTimeSetting;
		Mode = DataScript.GameMode;
	}

	void Update () {
//*************************************
//ゲーム開始までの生成時間等の表示処理*
//*************************************
		//準備(問題を生成)中の時間の表示
		Oftime += Time.deltaTime;
		TimeText.text = "経過時間：" + Mathf.FloorToInt(Oftime).ToString () + "秒";

		//準備完了後
		if (Suudoku.GameStart == "PlayGameStart") {
			ScreenText.SetActive (false);

			//ユーザのタイミングでゲームを開始する
			if (Input.GetKeyDown (KeyCode.Return)) {
				StartCountDownTrigger = true;
			}

			//ゲーム開始の10秒をカウント表示
			if (StartCountDownTrigger == true) {
				Starttime += Time.deltaTime;
				Countdown = 10.0f - Starttime;
				StartTimeText.text = Mathf.FloorToInt (Countdown).ToString () + "秒";
			}

//*************************************
//ゲーム開始の処理　　　　　　　　　　　*
//*************************************
			//カウントダウン終了後
			if (Countdown < 0) {
				StartCountDownTrigger = false;
				ScreenText2.SetActive (false);

				//タイムオーバーになるまで処理を行う
				if (TimeOverTrigger == true) {
					//ゲームクリアかタイムオーバーまでゲーム経過時間を加算
					if (TimeTrigger == true) {
						GameStartTime += Time.deltaTime * AccelCount;
					}

					//経過時間の秒数を分単位に変換
					m = (Mathf.FloorToInt (GameStartTime) / 60);
					//ゲーム時間の表示
					GameTimeText.text = "Time\n" + (Mathf.FloorToInt (GameStartTime) / 60) + ":" + ((Mathf.FloorToInt (GameStartTime) / 10) % 6) + (Mathf.FloorToInt (GameStartTime) % 10);

//*************************************
//各ゲームモードの処理 　　　　　　　　*
//*************************************
					//各ゲームモードの処理
					//時間制限あり
					if (Mode == "TimeMode") {
						StaticMode = "TimeMode";
						//設定された時間を超えた処理
						if (m >= TimeCount) {
							//タイムオーバーへの処理へ移行
							TimeOverTrigger = false;
							AfterTimeOverTrigger = true;
							InfoText.text = "残念！\n時間切れです！";
						}

						//時間制限なし
					} else if (Mode == "NoTimeMode") {
						StaticMode = "NoTimeMode";

						//時間追加あり
					} else if (Mode == "AccelMode") {
						StaticMode = "AccelMode";

						//マス目に入力されるまでの時間を加算
						AccelTime += Time.deltaTime;

						//10秒を超えるとゲーム内の時間を２倍にする
						if (AccelTime >= 10) {
							AccelCount = 2.0f;
						}

						//マス目に入力されたことを判定
						if (AccelTrigger == true) {
							//マス目に入力されることで入力までの時間とゲーム内の時間を初期にもどす
							AccelTime = 0.0f;
							AccelCount = 1.0f;

							//２秒追加
							GameStartTime += 2.0f;
							AccelTrigger = false;
						}

						//設定された時間を超えた処理
						if (m >= TimeCount) {
							//タイムオーバーへの処理へ移行
							TimeOverTrigger = false;
							AfterTimeOverTrigger = true;
							InfoText.text = "残念！\n時間切れです！";
						}

						//入力したマス目を隠す
					} else if (Mode == "NoMaskMode") {
						StaticMode = "NoMaskMode";

						//設定された時間を超えた処理
						if (m >= TimeCount) {
							//タイムオーバーへの処理へ移行
							TimeOverTrigger = false;
							AfterTimeOverTrigger = true;
							InfoText.text = "残念！\n時間切れです！";
						}

						//追加時間＋入力したマス目を隠す
					} else if (Mode == "AccelNoMaskMode") {
						StaticMode = "AccelNoMaskMode";

						//マス目に入力されるまでの時間を加算
						AccelTime += Time.deltaTime;

						//10秒を超えるとゲーム内の時間を２倍にする
						if (AccelTime >= 10) {
							AccelCount = 2.0f;
						}

						//マス目に入力されたことを判定
						if (AccelTrigger == true) {
							//マス目に入力されることで入力までの時間とゲーム内の時間を初期にもどす
							AccelTime = 0.0f;
							AccelCount = 1.0f;

							//２秒追加
							GameStartTime += 2.0f;
							AccelTrigger = false;
						}

						//設定された時間を超えた処理
						if (m >= TimeCount) {
							//タイムオーバーへの処理へ移行
							TimeOverTrigger = false;
							AfterTimeOverTrigger = true;
							InfoText.text = "残念！\n時間切れです！";
						}

					}

					if (ButtonScript.RetireTrigger == true) {
						TimeOverTrigger = false;
						AfterTimeOverTrigger = true;
						InfoText.text = "エンターキーで\nタイトルに戻ります.";
					}
						
				}

			}

//*************************************
//問題の答え合わせの処理 　　　　　　　*
//*************************************
			//問題が正解していた場合
			if (ButtonScript.ClearInfo == "Clear") {
				InfoText.text = "おめでとう！\n正解です！";
				ClearObj.SetActive (true);

				//ゲーム時間加算を終了
				TimeTrigger = false;

				//ゲーム終了時の時間をベストタイムとして一時保存し,一番早いベストタイムと比較し返す
				BestTime = Mathf.FloorToInt (GameStartTime);
				ReceiveBestTime = DataScript.BestScore (BestTime, StaticMode);

				//５分・１０分のゲームモードのそれそれのクリア出力
				if (TimeCount == 5) {
					ClearTimeText.text = "クリアタイム:" + (Mathf.FloorToInt (GameStartTime) / 60) + ":" + ((Mathf.FloorToInt (GameStartTime) / 10) % 6) + (Mathf.FloorToInt (GameStartTime) % 10) + "ベストタイム:" + (Mathf.FloorToInt (ReceiveBestTime) / 60) + ":" + ((Mathf.FloorToInt (ReceiveBestTime) / 10) % 6) + (Mathf.FloorToInt (ReceiveBestTime) % 10);
				} else if (TimeCount == 10) {
					ClearTimeText.text = "クリアタイム:" + (Mathf.FloorToInt (GameStartTime) / 60) + ":" + ((Mathf.FloorToInt (GameStartTime) / 10) % 6) + (Mathf.FloorToInt (GameStartTime) % 10);
				} else {
					ClearTimeText.text = "クリアタイム:" + (Mathf.FloorToInt (GameStartTime) / 60) + ":" + ((Mathf.FloorToInt (GameStartTime) / 10) % 6) + (Mathf.FloorToInt (GameStartTime) % 10);
				}
					
				//EnterKeyでタイトルへ戻る
				if (Input.GetKeyDown (KeyCode.Return)) {
					Application.LoadLevel ("タイトル");

				}

			//問題が不正解の場合
			} else if (ButtonScript.ClearInfo == "NotClear") {
				InfoText.text = "不正解です\nまだ時間はあります！";
				ButtonScript.ClearInfo = "";
			}

//*************************************
//時間切れ処理　　　　　  　　　　　　　*
//*************************************
			//時間切れ答えを画面上に出力
			if (AfterTimeOverTrigger == true) {
				GameTimeText.text = "Game\nOver";

				//入力ボタン系の削除
				Destroy (EndObj);
				for (Count = 1; Count <= 9; Count++) {
					NumberButton = GameObject.Find ("NumberButton" + Count);
					Destroy (NumberButton);
				}

				//ゲームタイプそれぞれで答えを画面上に出力
				for (i = 0; i < 3; i++) {
					for (k = 0; k < 3; k++) {
						for (j = 0; j < 9; j++) {
							Text_Number++;
							if (DataScript.TypeMode == "Normal") {
								NumberObj = GameObject.Find ("Number/Panel/Button" + Text_Number + "/" + Text_Number);
								NumberObj.GetComponent<Text> ().text = Suudoku.Solution [i, k, j].ToString ();

							} else if (DataScript.TypeMode == "Color") {
								NumberObj = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);

								if (Suudoku.Solution [i, k, j] == 1) {
									NumberObj.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
								} else if (Suudoku.Solution [i, k, j] == 2) {
									NumberObj.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
								} else if (Suudoku.Solution [i, k, j] == 3) {
									NumberObj.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
								} else if (Suudoku.Solution [i, k, j] == 4) {
									NumberObj.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
								} else if (Suudoku.Solution [i, k, j] == 5) {
									NumberObj.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
								} else if (Suudoku.Solution [i, k, j] == 6) {
									NumberObj.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
								} else if (Suudoku.Solution [i, k, j] == 7) {
									NumberObj.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
								} else if (Suudoku.Solution [i, k, j] == 8) {
									NumberObj.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
								} else if (Suudoku.Solution [i, k, j] == 9) {
									NumberObj.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
								}

							} else if (DataScript.TypeMode == "Animal") {
								NumberObj = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);

								if (Suudoku.Solution [i, k, j] == 1) {
									NumberObj.GetComponent<Image> ().sprite = Animal [0];
								} else if (Suudoku.Solution [i, k, j] == 2) {
									NumberObj.GetComponent<Image> ().sprite = Animal [1];
								} else if (Suudoku.Solution [i, k, j] == 3) {
									NumberObj.GetComponent<Image> ().sprite = Animal [2];
								} else if (Suudoku.Solution [i, k, j] == 4) {
									NumberObj.GetComponent<Image> ().sprite = Animal [3];
								} else if (Suudoku.Solution [i, k, j] == 5) {
									NumberObj.GetComponent<Image> ().sprite = Animal [4];
								} else if (Suudoku.Solution [i, k, j] == 6) {
									NumberObj.GetComponent<Image> ().sprite = Animal [5];
								} else if (Suudoku.Solution [i, k, j] == 7) {
									NumberObj.GetComponent<Image> ().sprite = Animal [6];
								} else if (Suudoku.Solution [i, k, j] == 8) {
									NumberObj.GetComponent<Image> ().sprite = Animal [7];
								} else if (Suudoku.Solution [i, k, j] == 9) {
									NumberObj.GetComponent<Image> ().sprite = Animal [8];
								}
							}
						}
					}
				}
				//処理終了
				AfterTimeOverTrigger = false;
			}
		}
	}
		
}
