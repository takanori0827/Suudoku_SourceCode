using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//ゲーム上のボタン制御
//DataScriptのTypeMode変数からの情報を元に処理を3種類行う
public class ButtonScript : MonoBehaviour {
	//for文用
	int i = 0;
	int k = 0;
	int j = 0;
	int l = 0;
	int Count = 0;

	int Text_Number = 0;
	int ButtonNumber = 0;
	int MaintainNumber = 0;	//マス目ボタンの番号を保存

	//文字列格納
	string StringName;		
	string _1to9StringName;
	string LoadName;

	//クリア状況の共有
	public static string ClearInfo;

	//キープポタンの各81個分のデータを格納
	bool[,] BoolKeepNumber = new bool[81,9];

	//リタイア時に答えを表示制御
	public static bool RetireTrigger = false;

	bool KeepTrigger = true;

	//各GameObject格納用
	GameObject ManegerObj;
	GameObject NumberButton;
	GameObject NumberObj;
	GameObject ButtonNumberObj;
	GameObject AudioObj;
	GameObject NumberImage;
	GameObject DataObj;
	GameObject[] KeepButton = new GameObject[9];
	GameObject KeepButtonText;
	GameObject KeepButtonImage;

	//Audioの格納用
	public AudioClip Correct;
	public AudioClip Disappointment;

	//9種類のスプライト格納
	public Sprite[] Animal = new Sprite[9];

	//ボタンのカラー情報を格納
	ColorBlock ColorButton;

	void Start(){
		Text_Number = 0;
		RetireTrigger = false;

		//GameObjectの取得
		DataObj = GameObject.Find ("DataManege");
		AudioObj = GameObject.Find ("Audio Source");

		//他オブジェクトのスクリプトとと紐づけ
		DataScript ds = DataObj.GetComponent<DataScript> ();
		Suudoku suudoku = this.GetComponent<Suudoku> ();
		ScreenDisposal sd = this.GetComponent<ScreenDisposal> ();

		//初期化
		for(i = 0; i < 81; i++){
			for(k = 0; k < 9; k++){
				BoolKeepNumber[i,k] = true;
			}
		}

		//入力ボタンの表示　数字・色・動物
		for (i = 1; i <= 9; i++) {
			NumberImage = GameObject.Find ("Number/Panel/NumberButton" + i + "/NumberImage" + i);
			if (DataScript.TypeMode == "Normal") {
				NumberImage.GetComponent<Image> ().enabled = false;
			} else if (DataScript.TypeMode == "Color") {
				NumberImage.GetComponent<Image> ().enabled = true;

				if (i == 1) {
					NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
				} else if (i == 2) {
					NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
				} else if (i == 3) {
					NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
				} else if (i == 4) {
					NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
				} else if (i == 5) {
					NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
				} else if (i == 6) {
					NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
				} else if (i == 7) {
					NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
				} else if (i == 8) {
					NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
				} else if (i == 9) {
					NumberImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
				}
			} else if (DataScript.TypeMode == "Animal") {
				NumberImage.GetComponent<Image> ().enabled = true;

				if (i == 1) {
					NumberImage.GetComponent<Image> ().sprite = Animal [0];
				} else if (i == 2) {
					NumberImage.GetComponent<Image> ().sprite = Animal [1];
				} else if (i == 3) {
					NumberImage.GetComponent<Image> ().sprite = Animal [2];
				} else if (i == 4) {
					NumberImage.GetComponent<Image> ().sprite = Animal [3];
				} else if (i == 5) {
					NumberImage.GetComponent<Image> ().sprite = Animal [4];
				} else if (i == 6) {
					NumberImage.GetComponent<Image> ().sprite = Animal [5];
				} else if (i == 7) {
					NumberImage.GetComponent<Image> ().sprite = Animal [6];
				} else if (i == 8) {
					NumberImage.GetComponent<Image> ().sprite = Animal [7];
				} else if (i == 9) {
					NumberImage.GetComponent<Image> ().sprite = Animal [8];
				}
			}
		}

		//3種類のゲームモードの場合のみ入力データをキープを行える
		if (DataScript.GameMode == "NoTimeMode" || DataScript.GameMode == "TimeMode" || DataScript.GameMode == "AccelMode") {
			//入力のキープボタンの表示　数字・色・動物
			for (i = 1; i <= 9; i++) {
				NumberImage = GameObject.Find ("Number/Panel/KeepNumberButton" + i + "/NumberImage" + i);
				KeepButton [i - 1] = GameObject.Find ("Number/Panel/KeepNumberButton" + i);

				if (DataScript.TypeMode == "Normal") {
					NumberImage.GetComponent<Image> ().enabled = false;
				} else if (DataScript.TypeMode == "Color") {
					NumberImage.GetComponent<Image> ().enabled = true;

					if (i == 1) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
					} else if (i == 2) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
					} else if (i == 3) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
					} else if (i == 4) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
					} else if (i == 5) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
					} else if (i == 6) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
					} else if (i == 7) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
					} else if (i == 8) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
					} else if (i == 9) {
						NumberImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
					}
				} else if (DataScript.TypeMode == "Animal") {
					NumberImage.GetComponent<Image> ().enabled = true;

					if (i == 1) {
						NumberImage.GetComponent<Image> ().sprite = Animal [0];
					} else if (i == 2) {
						NumberImage.GetComponent<Image> ().sprite = Animal [1];
					} else if (i == 3) {
						NumberImage.GetComponent<Image> ().sprite = Animal [2];
					} else if (i == 4) {
						NumberImage.GetComponent<Image> ().sprite = Animal [3];
					} else if (i == 5) {
						NumberImage.GetComponent<Image> ().sprite = Animal [4];
					} else if (i == 6) {
						NumberImage.GetComponent<Image> ().sprite = Animal [5];
					} else if (i == 7) {
						NumberImage.GetComponent<Image> ().sprite = Animal [6];
					} else if (i == 8) {
						NumberImage.GetComponent<Image> ().sprite = Animal [7];
					} else if (i == 9) {
						NumberImage.GetComponent<Image> ().sprite = Animal [8];
					}
				}
				KeepButton [i - 1].SetActive (false);
			}
			KeepTrigger = true;
		} else {
			KeepTrigger = false;
		}

	}

	void Update(){
		if(RetireTrigger == true){
			if(Input.GetKeyDown(KeyCode.Return)){
				Application.LoadLevel ("タイトル");
			}
		}

	}

//*************************************
//9x9のボタンからの処理制御　　　　　  *
//*************************************
	//9x9ブロックすべてに1~81の番号を振り分けその番号を取得
	public void ButtonClick(int Number){
		//取得情報を元にオブジェクトを取得する
		ButtonNumberObj = GameObject.Find("Button" + Number);
		ButtonNumberObj.GetComponent<Button> ();

		//そのボタン番号のオブジェクトが持つ０～９の数字から判定
		if (Suudoku.Input_Judge [Number - 1] == 0) {
			//0の場合はボタン番号情報を取得
			MaintainNumber = Number;
			AllSetActiveTrue ();

			if (KeepTrigger == true) {
				//各ゲームタイプに沿って入力ボタンを表示する
				for (i = 1; i <= 9; i++) {
					KeepButton [i - 1].SetActive (true);
					KeepButtonText = GameObject.Find ("Number/Panel/KeepNumberButton" + i + "/Text");
					KeepButtonImage = GameObject.Find ("Number/Panel/KeepNumberButton" + i + "/NumberImage" + i);

					//入力データを一時的に保存を行うキープ用のBool変数をチェック
					if (BoolKeepNumber [MaintainNumber - 1, i - 1] == true) {
						//trueの場合はキープ情報を各ゲームタイプに沿って表示する
						if (DataScript.TypeMode == "Normal") {
							KeepButtonText.GetComponent<Text> ().enabled = true;
						} else if (DataScript.TypeMode == "Color") {
							if (i == 1) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
							} else if (i == 2) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
							} else if (i == 3) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
							} else if (i == 4) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
							} else if (i == 5) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
							} else if (i == 6) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
							} else if (i == 7) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
							} else if (i == 8) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
							} else if (i == 9) {
								KeepButtonImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
							}
						} else if (DataScript.TypeMode == "Animal") {
							KeepButtonImage.GetComponent<Image> ().color = Color.white;
							if (i == 1) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [0];
							} else if (i == 2) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [1];
							} else if (i == 3) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [2];
							} else if (i == 4) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [3];
							} else if (i == 5) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [4];
							} else if (i == 6) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [5];
							} else if (i == 7) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [6];
							} else if (i == 8) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [7];
							} else if (i == 9) {
								KeepButtonImage.GetComponent<Image> ().sprite = Animal [8];
							}
						}
						
					} else {
						//falseの場合は各ゲームタイプに沿って非表示などを行う
						if (DataScript.TypeMode == "Normal") {
							KeepButtonText.GetComponent<Text> ().enabled = false;
						} else if (DataScript.TypeMode == "Color") {
							KeepButtonImage.GetComponent<Image> ().color = Color.white;
						} else if (DataScript.TypeMode == "Animal") {
							KeepButtonImage.GetComponent<Image> ().color = Color.black;
						}
						
					}
				}
			}
		
		} else {
			//0以外の場合保存用変数に初期値の０を代入
			MaintainNumber = 0;

			if (KeepTrigger == true) {
				//キープボタンを非表示にする
				for (i = 0; i < 9; i++) {
					KeepButton [i].SetActive (false);
				}
			}
					
		}
			
	}

	void AllSetActiveTrue(){
		for (i = 1; i <= 9; i++) {
			NumberButton = GameObject.Find ("NumberButton" + i);
			NumberButton.SetActive (true);
		}

	}

	void AllSetActiveFalse(){
		for (i = 1; i <= 9; i++) {
			NumberButton = GameObject.Find ("NumberButton" + i);
			NumberButton.SetActive (false);
		}

	}
		

//*************************************
//9x9のボタンからのボタンの情報を取得 *
//*************************************
	//ボタン情報を取得
	public void ButtonInfo(Button btn){
		//ボタンの色情報を取得
		ColorButton = btn.colors;
		//入力を行ったボタンが分かるようボタンの枠を青色へ変える
		ColorButton.normalColor = new Color (0f/255f, 60f/255f, 255f/255f, 255f/255f); //青

		//空きマス目以外は変更しない
		if (MaintainNumber != 0) {
			btn.colors = ColorButton;
		}
	}

//***************************************
//空きマス目に入力を行うボタンからの制御*
//***************************************
	public void AnswerNumberButton(int A_Number){
		_1to9StringName = "NumberButton" + A_Number;

		//各入力ボタンの情報を元にAnswer関数へ移行
		switch (_1to9StringName) {
		case "NumberButton1":
			ButtonNumber = 1;
			Answer (ButtonNumber);

			break;

		case "NumberButton2":
			ButtonNumber = 2;
			Answer (ButtonNumber);

			break;


		case "NumberButton3":
			ButtonNumber = 3;
			Answer (ButtonNumber);

			break;


		case "NumberButton4":
			ButtonNumber = 4;
			Answer (ButtonNumber);

			break;


		case "NumberButton5":
			ButtonNumber = 5;
			Answer (ButtonNumber);

			break;


		case "NumberButton6":
			ButtonNumber = 6;
			Answer (ButtonNumber);

			break;


		case "NumberButton7":
			ButtonNumber = 7;
			Answer (ButtonNumber);

			break;


		case "NumberButton8":
			ButtonNumber = 8;
			Answer (ButtonNumber);

			break;


		case "NumberButton9":
			ButtonNumber = 9;
			Answer (ButtonNumber);

			break;
		}

	}

	//配列への答えの入力や空きマスへ数字・色・動物のスプライトを表示処理
	void Answer(int _ButtonNumber){
		if (MaintainNumber != 0) {
			//入力ボタンの1~9の数字を答えようの配列に代入
			//ゲームタイプに沿って入力した答えを画面上に表示を行う
			//ただし,ゲーム画面上に表示させないゲームモードの場合は表示は行わない
			Suudoku.Answer [MaintainNumber - 1] = _ButtonNumber;
			NumberObj = GameObject.Find ("Number/Panel/Button" + MaintainNumber + "/" + MaintainNumber);
			NumberImage = GameObject.Find ("Number/Panel/Button" + MaintainNumber + "/Image" + MaintainNumber);

			if (DataScript.TypeMode == "Normal") {
				if (ScreenDisposal.StaticMode == "NoMaskMode" || ScreenDisposal.StaticMode == "AccelNoMaskMode") {
					NumberObj.GetComponent<Text> ().text = "?";
					NumberObj.GetComponent<Text> ().color = Color.red;

				} else {
					NumberObj.GetComponent<Text> ().text = Suudoku.Answer [MaintainNumber - 1].ToString ();
					NumberObj.GetComponent<Text> ().color = Color.red;
				}

			} else if (DataScript.TypeMode == "Color") {
				if (ScreenDisposal.StaticMode == "NoMaskMode" || ScreenDisposal.StaticMode == "AccelNoMaskMode") {
					NumberImage.GetComponent<Image> ().color = Color.white;

				} else {
					if (Suudoku.Answer [MaintainNumber - 1] == 1) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
					} else if (Suudoku.Answer [MaintainNumber - 1] == 2) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
					} else if (Suudoku.Answer [MaintainNumber - 1] == 3) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
					} else if (Suudoku.Answer [MaintainNumber - 1] == 4) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
					} else if (Suudoku.Answer [MaintainNumber - 1] == 5) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
					} else if (Suudoku.Answer [MaintainNumber - 1] == 6) {
						NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
					} else if (Suudoku.Answer [MaintainNumber - 1] == 7) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
					} else if (Suudoku.Answer [MaintainNumber - 1] == 8) {
						NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
					} else if (Suudoku.Answer [MaintainNumber - 1] == 9) {
						NumberImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
					}
				}

			} else if (DataScript.TypeMode == "Animal") {
				if (ScreenDisposal.StaticMode == "NoMaskMode" || ScreenDisposal.StaticMode == "AccelNoMaskMode") {
					NumberImage.GetComponent<Image> ().color = Color.white;

				} else {
					if (Suudoku.Answer [MaintainNumber - 1] == 1) {
						NumberImage.GetComponent<Image> ().sprite = Animal [0];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 2) {
						NumberImage.GetComponent<Image> ().sprite = Animal [1];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 3) {
						NumberImage.GetComponent<Image> ().sprite = Animal [2];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 4) {
						NumberImage.GetComponent<Image> ().sprite = Animal [3];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 5) {
						NumberImage.GetComponent<Image> ().sprite = Animal [4];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 6) {
						NumberImage.GetComponent<Image> ().sprite = Animal [5];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 7) {
						NumberImage.GetComponent<Image> ().sprite = Animal [6];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 8) {
						NumberImage.GetComponent<Image> ().sprite = Animal [7];
					} else if (Suudoku.Answer [MaintainNumber - 1] == 9) {
						NumberImage.GetComponent<Image> ().sprite = Animal [8];
					}
				}

			}

			ScreenDisposal.AccelTrigger = true;
		}

	}

//***************************************
//空きマスの情報をメモ（キープ）制御　  *
//***************************************
	public void keepButton(int A_Number){
		if (KeepTrigger == true) {
			//ボタンからの情報を元にオブジェクトを取得
			_1to9StringName = "KeepNumberButton" + A_Number;
			KeepButtonText = GameObject.Find ("Number/Panel/KeepNumberButton" + A_Number + "/Text");
			KeepButtonImage = GameObject.Find ("Number/Panel/KeepNumberButton" + A_Number + "/NumberImage" + A_Number);

			//各キープボタンの情報を元にKeep関数へ移行
			switch (_1to9StringName) {
			case "KeepNumberButton1":
				Keep (A_Number, 1);

				break;

			case "KeepNumberButton2":
				Keep (A_Number, 2);

				break;


			case "KeepNumberButton3":
				Keep (A_Number, 3);

				break;


			case "KeepNumberButton4":
				Keep (A_Number, 4);

				break;


			case "KeepNumberButton5":
				Keep (A_Number, 5);

				break;


			case "KeepNumberButton6":
				Keep (A_Number, 6);

				break;


			case "KeepNumberButton7":
				Keep (A_Number, 7);

				break;


			case "KeepNumberButton8":
				Keep (A_Number, 8);

				break;


			case "KeepNumberButton9":
				Keep (A_Number, 9);

				break;
			}
		}

	}

	void Keep(int A_Number, int Number){
		//ボタンを押すことで２種類の処理を交互に行う
		if (BoolKeepNumber [MaintainNumber - 1, Number - 1] == true) {
			BoolKeepNumber [MaintainNumber - 1, Number - 1] = false;

			//各ゲームタイプに沿ってキープボタンの非表示などを行う
			if (DataScript.TypeMode == "Normal") {
				KeepButtonText.GetComponent<Text> ().enabled = false;

			} else if (DataScript.TypeMode == "Color") {
				KeepButtonImage.GetComponent<Image> ().color = Color.white;

			} else if (DataScript.TypeMode == "Animal") {
				KeepButtonImage.GetComponent<Image> ().color = Color.black;

			}

		}else{
			BoolKeepNumber [MaintainNumber - 1,Number - 1] = true;

			//各ゲームタイプに沿ってキープボタンの表示などを行う
			if (DataScript.TypeMode == "Normal") {
				KeepButtonText.GetComponent<Text> ().enabled = true;

			} else if (DataScript.TypeMode == "Color") {
				if (Number == 1) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
				} else if (Number == 2) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
				} else if (Number == 3) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
				} else if (Number == 4) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
				} else if (Number == 5) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
				} else if (Number == 6) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
				} else if (Number == 7) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
				} else if (Number == 8) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
				} else if (Number == 9) {
					KeepButtonImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
				}

			} else if (DataScript.TypeMode == "Animal") {
				KeepButtonImage.GetComponent<Image> ().color = Color.white;
				if (Number == 1) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [0];
				} else if (Number == 2) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [1];
				} else if (Number == 3) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [2];
				} else if (Number == 4) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [3];
				} else if (Number == 5) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [4];
				} else if (Number == 6) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [5];
				} else if (Number == 7) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [6];
				} else if (Number == 8) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [7];
				} else if (Number == 9) {
					KeepButtonImage.GetComponent<Image> ().sprite = Animal [8];
				}
			}

		}
	}

//***************************************
//空きマスへ入力された答えを解する  　  *
//***************************************
	public void Kotae(){
		for(i = 0; i < 3; i++){
			for(k = 0; k < 3; k++){
				for(j = 0; j < 9; j++){
					if (Suudoku.Answer [l] == Suudoku.Solution [i, k, j]) {
						Count++;
						l++;
					} else {
						l++;
					}
				}
			}
		}

		//一致したカウントが81個(入力済みを含む)があった場合
		if (Count == 81) {
			//ゲームクリア
			ClearInfo = "Clear";
			AudioObj.GetComponent<AudioSource> ().PlayOneShot (Correct);
			Count = 0;
			l = 0;
	
		} else {
			//再度入力を促す
			ClearInfo = "NotClear";
			AudioObj.GetComponent<AudioSource> ().PlayOneShot (Disappointment);
			Count = 0;
			l = 0;
		}

	}


	public void Retire(){
		RetireTrigger = true;

	}

	public void Reload(){
		LoadName = Application.loadedLevelName;
		Application.LoadLevel (LoadName);
	}

	public void Back(){
		Application.LoadLevel ("タイトル");
	}

	public void SetActiveFalse(){
		for (i = 0; i < 9; i++) {
			KeepButton [i].SetActive (false);
		}

	}

}
