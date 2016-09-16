using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//メイン関数
//数独の問題を自動的に行い,問題が解けるかどうかの判定も行う
public class Suudoku : MonoBehaviour {
	//for文用
	int i = 0;
	int j = 0;
	int k = 0;
	int l = 0;
	int m = 0;
	int n = 0;

	//配列の階層を跨いで処理に使用
	int MaskCounter_i = 0;
	int MaskCounter_k = 0;
	int MaskCounter_j = 0;
	int Counter_i = 0;
	int Counter_k = 0;
	int Counter_j = 0;

	//カウント用
	int Count = 0;
	int ShuffleCount = 0;		//シャッフルカウント
	int AgainCount = 0;			//繰り返しカウント
	int IdenticallyCount = 0;	//同じ数字の個数カウント
	int NumberCount = 0;		//配列内の個数カウント

	//加算用
	int sum = 0;
	//保持用
	int num = 0;
	//設定された空白マスの数
	public int BreakMasu = 0;

	//UI表示処理
	int Text_Number = 0;
	int Button_Number = 0;

	//乱数を用いて4種類の入れ替え方法を判別様
	int ShuffleNumber = 0;
	int ColorRow = 0;
	//行
	int RowNumber_1 = 0;	
	int RowNumber_2 = 0;	
	int BlockRow = 0;
	//列
	int ColumnNumber_1 = 0;
	int ColumnNumber_2 = 0;
	int BlockColumn = 0;

	//81個の数字を格納
	int[,,] _1to9Rank = new int[3, 3, 9];	
	//空白マスの候補数字の格納
	int[,,,] Mask_Rank = new int[3, 3, 9, 9]; 
	//配列比較用
	int[] Compare = new int[9];
	//解答用
	public static int[,,] Solution = new int[3, 3, 9];	//シャッフルした配列を回答用に保持する
	//配列内交換用
	int[,] ChangeNumber = new int [3,9];

	//最初の動作を制御
	bool StartTrigger = false;
	//各処理の制御
	bool[] StepTrigger = new bool[15]; 

	//ペア・トリオの判別
	bool[] Identically_Number = new bool[9];
	bool[,] Identically_Number_2 = new bool[3,3];
	bool[,,] Identically_Number_3 = new bool[3,3,9];
	//候補数字からペア数字を削除するかの制御
	bool IdenticallyFlag = false;

	//他関数にゲーム開始処理を行わせる
	public static string GameStart ="";

	//GameObject
	GameObject NumberObj;
	GameObject NumberImage;
	GameObject DataObj;

	//Sprite
	public Sprite[] Animal = new Sprite[9];
	public Sprite NoMeshAnimal;

	//他関数で判別等を行う答え配列
	public static int[] Answer = new int[81];
	public static int[] Input_Judge = new int[81];



	//縦横が被らない様9x9の配列に代入
	void Start () {
		DataObj = GameObject.Find ("DataManege");
		BreakMasu = DataScript.BreakMasu;
		GameStart = "";

		if (DataScript.TypeMode == "Normal") {
			for (i = 1; i <= 81; i++) {
				NumberImage = GameObject.Find ("Number/Panel/Button" + i + "/Image" + i);
				NumberImage.GetComponent<Image> ().enabled = false;
			}
		} else if (DataScript.TypeMode == "Color") {
			for (i = 1; i <= 81; i++) {
				NumberObj = GameObject.Find ("Number/Panel/Button" + i + "/" + i);
				NumberObj.GetComponent<Text> ().enabled = false;
			}
		} else if (DataScript.TypeMode == "Animal") {
			for (i = 1; i <= 81; i++) {
				NumberObj = GameObject.Find ("Number/Panel/Button" + i + "/" + i);
				NumberObj.GetComponent<Text> ().enabled = false;
			}
		}

		for (i = 1; i <= 81; i++) {
			Button_Number++;
			NumberObj = GameObject.Find ("Number/Panel/Button" + Button_Number + "/" + i);
			NumberObj.SetActive (true);
		}

		StartCoroutine (SettingStart());
	}
		

	IEnumerator SettingStart ()
	{

		yield return StartCoroutine (NumberSet ());

		for (ShuffleCount = 0; ShuffleCount <= 100; ShuffleCount++) {
			yield return StartCoroutine (Shuffle ());
		}

		yield return StartCoroutine (Solution_Step1 ());

		yield return StartCoroutine (Solution_Step2 ());

		for (AgainCount = 0; AgainCount <= 5; AgainCount++) {
			yield return StartCoroutine (Solution_Step3 ());

			yield return StartCoroutine (Solution_Step4 ());

			yield return StartCoroutine (Solution_Step5 ());

			yield return StartCoroutine (Solution_Step6 ());

			yield return StartCoroutine (Solution_Step7 ());

			yield return StartCoroutine (Solution_Step8 ());

			yield return StartCoroutine (Solution_Step9 ());

			yield return StartCoroutine (Solution_Step10 ());

			yield return StartCoroutine (Solution_Step11 ());
		}

		/* ランダムで作られた数独の問題を解けるのかチェックを行う
			非表示にする前の配列と比較し全て同じであるかをチェック */
		for (i = 0; i < 3; i++) {
			for (k = 0; k < 3; k++) {
				for (j = 0; j < 9; j++) {
					if (_1to9Rank [i, k, j] == Solution [i, k, j]) {
						Count++;
					}
				}
			}
		}

		//全て同じであればランダムに作られた問題は解けるものとする
		if (Count == 81) {
			//ゲーム開始
			GameStart = "PlayGameStart";
		} else {
			//一つでも違うものがあれば答えが2つ以上の重解とし,問題を作り直すところからやり直す.
			yield return StartCoroutine (SettingStart());
		}

		yield return null;

	}

	//配列に81個の1~9を格納
	public IEnumerator NumberSet(){
		sum = 0;

		//配列に1~9を格納
		for (i = 0; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					//配列に1~9を格納し終わるたびに+1した数値で次の階層配列に格納
					sum = (i * 3) + j + k + 1;
					if (sum <= 9) {
						_1to9Rank [i, j, k] = sum;

					//10の場合は1に戻す
					} else if (sum >= 10) {
						sum -= 9;
						_1to9Rank [i, j, k] = sum;

					}

				}
			}
		}

		//行と列と3x3ブロックの9つが合計で45になるよう 2列目と4列目 3列目と7列目 6列目と8列目の配列の数値を変える
		for (j = 0; j < 9; j++) {
			ChangeNumber [0, j] = _1to9Rank [0, 1, j];
			_1to9Rank [0, 1, j] = _1to9Rank [1, 0, j];
			_1to9Rank [1, 0, j] = ChangeNumber [0, j];

			ChangeNumber [1, j] = _1to9Rank [0, 2, j];
			_1to9Rank [0, 2, j] = _1to9Rank [2, 0, j];
			_1to9Rank [2, 0, j] = ChangeNumber [1, j];

			ChangeNumber [2, j] = _1to9Rank [1, 2, j];
			_1to9Rank [1, 2, j] = _1to9Rank [2, 1, j];
			_1to9Rank [2, 1, j] = ChangeNumber [2, j];

		}
		//-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --

		yield return null;

	}
				
	//81個の数字をシャッフルする
	IEnumerator Shuffle(){
		ShuffleNumber = UnityEngine.Random.Range (1, 3); 		//1～2をランダムで生成
		ColorRow = UnityEngine.Random.Range (1, 3); 			//1～2をランダムで生成

		//3行・列まとめてシャッフルするか,１行・列ずつシャッフルするかをランダムで決定
		switch (ShuffleNumber) {

		//3行・列まとめてシャッフル
		case 1:
		
		//行か列のどちらかシャッフルするかをランダムで決定
			switch (ColorRow) {
			//行でシャッフル
			case 1:
				do {
					//どの3行かをランダムに決定
					RowNumber_1 = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成
					RowNumber_2 = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成

				} while(RowNumber_1 == RowNumber_2);

			//2つの3行を交換
				for (k = 0; k < 3; k++) {
					for (j = 0; j < 9; j++) {
						ChangeNumber [0, j] = _1to9Rank [RowNumber_1, k, j];
						_1to9Rank [RowNumber_1, k, j] = _1to9Rank [RowNumber_2, k, j];
						_1to9Rank [RowNumber_2, k, j] = ChangeNumber [0, j];
					}
				}

				break;

			//列でシャッフル
			case 2:
				do {
					//どの3列かをランダムに決定 (0~2 3~5 6~8)
					ColumnNumber_1 = (UnityEngine.Random.Range (0, 3) * 3); //0,3,6をランダムで生成
					ColumnNumber_2 = (UnityEngine.Random.Range (0, 3) * 3); //0,3,6をランダムで生成

				} while(ColumnNumber_1 == ColumnNumber_2);



			//2つの3列を交換
				do {
					for (i = 0; i < 3; i++) {
						for (k = 0; k < 3; k++) {
							ChangeNumber [i, k] = _1to9Rank [i, k, ColumnNumber_1];
							_1to9Rank [i, k, ColumnNumber_1] = _1to9Rank [i, k, ColumnNumber_2];
							_1to9Rank [i, k, ColumnNumber_2] = ChangeNumber [i, k];
						}
					}
					ColumnNumber_1++;
					ColumnNumber_2++;

				} while(ColumnNumber_1 % 3 != 0 && ColumnNumber_2 % 3 != 0);

				break;
			}

			break;

		//1行・列でシャッフル
		case 2:
		//行か列のどちらかシャッフルするかをランダムで決定
			switch (ColorRow) {
			//行でシャッフル
			case 1:
				do {
					//どの行かをランダムに決定
					RowNumber_1 = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成
					RowNumber_2 = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成

					//3行を指定
					BlockRow = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成

				} while(RowNumber_1 == RowNumber_2);


			//2つの行を交換
				for (j = 0; j < 9; j++) {
					//同じ最上位階層の中でしか交換ができない
					ChangeNumber [0, j] = _1to9Rank [BlockRow, RowNumber_1, j];
					_1to9Rank [BlockRow, RowNumber_1, j] = _1to9Rank [BlockRow, RowNumber_2, j];
					_1to9Rank [BlockRow, RowNumber_2, j] = ChangeNumber [0, j];
				}

				break;

			//列でシャッフル
			case 2:
				do {
					//列ブロックの基準を決める
					BlockColumn = (UnityEngine.Random.Range (0, 3) * 3); //0,3,6をランダムで生成

					//決められた列ブロックのどの列かを決定
					ColumnNumber_1 = BlockColumn + UnityEngine.Random.Range (0, 3); //0~2をランダムで生成
					ColumnNumber_2 = BlockColumn + UnityEngine.Random.Range (0, 3); //0~2をランダムで生成

				} while(ColumnNumber_1 == ColumnNumber_2);


			//2つの列を交換
				for (i = 0; i < 3; i++) {
					for (k = 0; k < 3; k++) {
						ChangeNumber [i, k] = _1to9Rank [i, k, ColumnNumber_1];
						_1to9Rank [i, k, ColumnNumber_1] = _1to9Rank [i, k, ColumnNumber_2];
						_1to9Rank [i, k, ColumnNumber_2] = ChangeNumber [i, k];
					}
				}
				break;
			}
			break;
		}

		yield return null;

	}

	//ランダムで複数個数空白にする
	IEnumerator Solution_Step1(){
		Count = 0;

		//シャッフルされたものが答えとなるので答えを別の配列に格納
		for (i = 0; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					Solution [i, j, k] = _1to9Rank [i, j, k];
				}
			}
		}

		do {
			//空白にする部分をランダムに設定
			MaskCounter_i = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成
			MaskCounter_j = UnityEngine.Random.Range (0, 3); //0～2をランダムで生成
			MaskCounter_k = UnityEngine.Random.Range (0, 9); //0～8をランダムで生成

			//既に空白になっていないかチェック
			if (_1to9Rank [MaskCounter_i, MaskCounter_j, MaskCounter_k] != 0) {
				//空白にする部分は0とする
				_1to9Rank [MaskCounter_i, MaskCounter_j, MaskCounter_k] = 0;
				Count++;
			}

			//空白を指定された数をあける

		} while(Count < BreakMasu);
			
		yield return null;

	}

	//テキスト及びイメージを出力
	IEnumerator Solution_Step2(){
		Text_Number = 0;
		Button_Number = 0;
		Count = 0;

		for (i = 0; i < 3; i++) {
			for (k = 0; k < 3; k++) {
				for (j = 0; j < 9; j++) {
					//入力された答えや答えの表示を簡単にするため1次元配列に格納
					Input_Judge [Count] = _1to9Rank [i, k, j];
					Answer [Count] = _1to9Rank [i, k, j];
					Count++;

					//配列の中身によって出力するものを変える
					if (_1to9Rank [i, k, j] == 0) {
						Text_Number++;
						//空白マスの出力
						if (DataScript.TypeMode == "Normal") {
							NumberObj = GameObject.Find ("Number/Panel/Button" + Text_Number + "/" + Text_Number);
							NumberObj.GetComponent<Text> ().text = "";
						} else if (DataScript.TypeMode == "Color") {
							NumberImage = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);
							NumberImage.GetComponent<Image> ().color = Color.white;
						} else if (DataScript.TypeMode == "Animal") {
							NumberImage = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);
							NumberImage.GetComponent<Image> ().sprite = NoMeshAnimal;

						}

					} else if(_1to9Rank [i, k, j] != 0) {
						Text_Number++;
						//空白マスではないマスの出力
						if (DataScript.TypeMode == "Normal") {
							NumberObj = GameObject.Find ("Number/Panel/Button" + Text_Number + "/" + Text_Number);
							NumberObj.GetComponent<Text> ().text = _1to9Rank [i, k, j].ToString ();
						} else if (DataScript.TypeMode == "Color") {
							NumberImage = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);


							if (_1to9Rank [i, k, j] == 1) {
								NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //赤
							} else if (_1to9Rank [i, k, j] == 2) {
								NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //緑
							} else if (_1to9Rank [i, k, j] == 3) {
								NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //青
							} else if (_1to9Rank [i, k, j] == 4) {
								NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 170f / 255f, 0f / 255f, 255f / 255f); //オレンジ
							} else if (_1to9Rank [i, k, j] == 5) {
								NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //黒
							} else if (_1to9Rank [i, k, j] == 6) {
								NumberImage.GetComponent<Image> ().color = new Color (0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //水色
							} else if (_1to9Rank [i, k, j] == 7) {
								NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //ピンク
							} else if (_1to9Rank [i, k, j] == 8) {
								NumberImage.GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //黄色
							} else if (_1to9Rank [i, k, j] == 9) {
								NumberImage.GetComponent<Image> ().color = new Color (160f / 255f, 0f / 255f, 255f / 255f, 255f / 255f); //紫
							}

						} else if (DataScript.TypeMode == "Animal") {
							NumberImage = GameObject.Find ("Number/Panel/Button" + Text_Number + "/Image" + Text_Number);

							if (_1to9Rank [i, k, j] == 1) {
								NumberImage.GetComponent<Image> ().sprite = Animal [0];
							} else if (_1to9Rank [i, k, j] == 2) {
								NumberImage.GetComponent<Image> ().sprite = Animal [1];
							} else if (_1to9Rank [i, k, j] == 3) {
								NumberImage.GetComponent<Image> ().sprite = Animal [2];
							} else if (_1to9Rank [i, k, j] == 4) {
								NumberImage.GetComponent<Image> ().sprite = Animal [3];
							} else if (_1to9Rank [i, k, j] == 5) {
								NumberImage.GetComponent<Image> ().sprite = Animal [4];
							} else if (_1to9Rank [i, k, j] == 6) {
								NumberImage.GetComponent<Image> ().sprite = Animal [5];
							} else if (_1to9Rank [i, k, j] == 7) {
								NumberImage.GetComponent<Image> ().sprite = Animal [6];
							} else if (_1to9Rank [i, k, j] == 8) {
								NumberImage.GetComponent<Image> ().sprite = Animal [7];
							} else if (_1to9Rank [i, k, j] == 9) {
								NumberImage.GetComponent<Image> ().sprite = Animal [8];
							}
						}

					}
				}
			}
		}


		yield return null;

	}

	//空白マスに1~9の数字を入れる
	IEnumerator Solution_Step3(){
		//配列内のすべての数字をチェック
		for (i = 0; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					//配列の数字が0の場合
					if (_1to9Rank [i, j, k] == 0) {
						for (l = 0; l < 9; l++) {
							//答えとなる1~9の候補を格納
							Mask_Rank [i, j, k, l] = (l + 1);
						}
					}
				}
			}
		}


		yield return null;

	}

	//行毎に候補の確定数字をチェック
	IEnumerator Solution_Step4 (){
		for (i = 0; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//空白(0)ではないマスをチェック
						for (l = 0; l < 9; l++) {
							if (_1to9Rank [i, j, l] != 0) {
								//候補数字から既に埋まっている確定数字を削除(0)する
								for(m = 0; m < 9; m++){
									if (Mask_Rank [i, j, k, m] == _1to9Rank [i, j, l]) {
										Mask_Rank [i, j, k, m] = 0;
									}
								}
							}
						}

					}
				}
			}
		}

		Setting ();

		yield return null;
	}

	//列毎に候補の確定数字をチェック
	IEnumerator Solution_Step5(){
		for (j = 0; j < 9; j++) {
			for (i = 0; i < 3; i++) {
				for (k = 0; k < 3; k++) {
					if (_1to9Rank [i, k, j] == 0) {

						for (m = 0; m < 3; m++) {
							for (n = 0; n < 3; n++) {
								if (m != i || n != k) {
									if (_1to9Rank [m, n, j] != 0) {
										for (l = 0; l < 9; l++) {
											if (_1to9Rank [m, n, j] == Mask_Rank [i, k, j, l]) {
												Mask_Rank [i, k, j, l] = 0;
											}
										}
									}
								}
							}
						}
							
					}
				}
			}
		}
	
		Setting ();

		yield return null;
	}

	//3x3ブロックずつに候補の確定数字をチェック
	IEnumerator Solution_Step6(){
		//上段の3x3の3ブロック
		Counter_i = 0;	//上段
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 0; i < 1; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//3x3ブロックを3つのブロックに分けた起点を指定
						if (0 <= k && k < 3) {
							Counter_k = 0;
							num = 3;

						} else if (3 <= k && k < 6) {
							Counter_k = 3;
							num = 6;

						} else if (6 <= k && k < 9) {
							Counter_k = 6;
							num = 9;
						}

						do {
							do {
								for (l = 0; l < 9; l++) {
									//候補から確定数字を削除
									if (Mask_Rank [i, j, k, l] == _1to9Rank [Counter_i, Counter_j, Counter_k]) {
										Compare [l] = 0;
									}

								}
								//比較範囲を変更
								Counter_k++;
	
							} while(Counter_k < num);

							//配列の階層を変更
							Counter_j++;
							//起点に戻す
							Counter_k = Counter_k - 3;
	
							//3x3ブロック全てを比較し終わったら抜ける
						} while(Counter_j <= 2);

						Counter_j = 0;

					}
				}
			}
		}

		//中段の3x3の3ブロック
		Counter_i = 1;	//中段
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 1; i < 2; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						if (0 <= k && k < 3) {
							Counter_k = 0;
							num = 3;

						} else if (3 <= k && k < 6) {
							Counter_k = 3;
							num = 6;

						} else if (6 <= k && k < 9) {
							Counter_k = 6;
							num = 9;
						}

						do {
							do {
								for (l = 0; l < 9; l++) {
									if (Mask_Rank [i, j, k, l] == _1to9Rank [Counter_i, Counter_j, Counter_k]) {
										Mask_Rank [i, j, k, l] = 0;
									}
								}

								Counter_k++;
		
							} while(Counter_k < num);

							Counter_j++;
							Counter_k = Counter_k - 3;
						
						} while(Counter_j <= 2);

						Counter_j = 0;

					}
				}
			}
		}

		//下段の3x3の3ブロック
		Counter_i = 2;	//下段
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 2; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {
	
						if (0 <= k && k < 3) {
							Counter_k = 0;
							num = 3;

						} else if (3 <= k && k < 6) {
							Counter_k = 3;
							num = 6;

						} else if (6 <= k && k < 9) {
							Counter_k = 6;
							num = 9;
						}

						do {
							do{
								for (l = 0; l < 9; l++) {
									if (Mask_Rank [i, j, k, l] == _1to9Rank [Counter_i,Counter_j,Counter_k]) {
										Mask_Rank [i, j, k, l] = 0;
									}
								}

								Counter_k++;

							}while(Counter_k < num);

							Counter_j++;
							Counter_k = Counter_k - 3;

						} while(Counter_j <= 2);
							
						Counter_j = 0;

					}
				}
			}
		}
			
		Setting ();

		yield return null;

	}

	//行の候補数字で他の候補数字とペア（トリオ）になっているかをチェック
	//他のペア以外の候補数字からペア数字を削除
	IEnumerator Solution_Step7(){
		IdenticallyCount = 0;
		NumberCount = 0;

		for (i = 0; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//比較用配列に候補の数字を格納
						for (l = 0; l < 9; l++) {
							Compare [l] = Mask_Rank [i, j, k, l];
							//候補数字が何個かをカウント
							if(Compare[l] != 0){
								NumberCount++;
							}
						}

						//候補数字が2つ以上の場合
						if (NumberCount >= 2) {

							//他の空白マスのチェック
							for (l = 0; l < 9; l++) {
								for (m = 0; m < 9; m++) {
									//比較する空白マスの配列以外の配列
									if (l != k) {
										if (_1to9Rank [i, j, l] == 0) {
											//9つの配列の内容を比較
											if (Mask_Rank [i, j, l, m] == Compare [m]) {
												IdenticallyCount++;
											}
										}
									}
								}

								//9つすべてが一致していた時ペア（トリオ）と判定
								if (IdenticallyCount == 9) {
									//一致していた配列番号を保存
									Identically_Number [l] = true;

									//ペア以外の候補数字からペアの数字を削除制御
									IdenticallyFlag = true;
									IdenticallyCount = 0;
								} else {
									IdenticallyCount = 0;
								}
							}

						}

						//ペア以外の候補数字からペアの数字を削除
						if (IdenticallyFlag == true) {

							//空白マスをチェック(比較を行ったマスとペアになったマスを除く)
							for (l = 0; l < 9; l++) {
								if (_1to9Rank [i, j, l] == 0) {
									if (Identically_Number [l] != true) {
										if (l != k) {
											//他のペア以外の候補数字からペアの数字を削除
											for (m = 0; m < 9; m++) {
												if (Mask_Rank [i, j, l, m] == Compare [m]) {
													Mask_Rank [i, j, l, m] = 0;
												}
											}
										}
									}
								}
							}
							IdenticallyFlag = false;

						}

						//初期化
						NumberCount = 0;
						IdenticallyCount = 0;
						for (l = 0; l < 9; l++) {
							Identically_Number [l] = false;
						}

					}
				}
			}
		}

		Setting ();

		yield return null;
	}

	//列の候補数字で他の候補数字とペア（トリオ）になっているかをチェック
	//他のペア以外の候補数字からペア数字を削除
	IEnumerator Solution_Step8(){
		IdenticallyCount = 0;
		NumberCount = 0;

		for (k = 0; k < 9; k++) {
			for (i = 0; i < 3; i++) {
				for (j = 0; j < 3; j++) {
					if (_1to9Rank [i, j, k] == 0) {

						//比較用配列に候補の数字を格納
						for (l = 0; l < 9; l++) {
							Compare [l] = Mask_Rank [i, j, k, l];
							if (Compare [l] != 0) {
								//候補数字が何個かをカウント
								NumberCount++;
							}
						}

						//候補数字が2つ以上の場合
						if (NumberCount >= 2) {
							
							//他の空白マスのチェック
							for (l = 0; l < 3; l++) {
								for (m = 0; m < 3; m++) {
									//比較する空白マスの配列以外の配列
									if (l == i && m == j) {
									} else {
										if (_1to9Rank [l, m, k] == 0) {
											for (n = 0; n < 9; n++) {
												//9つの配列の内容を比較
												if (Mask_Rank [l, m, k, n] == Compare [n]) {
													IdenticallyCount++;
												}
											}

											//9つすべてが一致していた時ペア（トリオ）と判定
											if (IdenticallyCount == 9) {
												//一致していた配列番号を保存
												Identically_Number_2 [l, m] = true;

												//ペア以外の候補数字からペアの数字を削除制御
												IdenticallyFlag = true;
												IdenticallyCount = 0;
											} else {
												IdenticallyCount = 0;
											}
										}
									}
								}
							}

						}

						//ペア以外の候補数字からペアの数字を削除
						if (IdenticallyFlag == true) {

							//空白マスをチェック(比較を行ったマスとペアになったマスを除く)
							for (l = 0; l < 3; l++) {
								for (m = 0; m < 3; m++) {
									if (_1to9Rank [l, m, k] == 0) {
										if (Identically_Number_2 [l, m] != true) {
											if (l == i && m == j) {
											} else {
												//他のペア以外の候補数字からペアの数字を削除
												for (n = 0; n < 9; n++) {
													if (Mask_Rank [l, m, k, n] == Compare [n]) {
														Mask_Rank [l, m, k, n] = 0;
													}
												}
											}
										}
									}
								}
							}
							IdenticallyFlag = false;

						}

						//初期化
						NumberCount = 0;
						for (l = 0; l < 3; l++) {
							for (m = 0; m < 3; m++) {
								Identically_Number_2 [l, m] = false;
							}
						}

					}
				}
			}
		}

		Setting ();

		yield return null;

	}

	//3x3のブロックの候補数字で他の候補数字とペア（トリオ）になっているかをチェック
	//他のペア以外の候補数字からペア数字を削除
	IEnumerator Solution_Step9(){
		//上段3つの3x3ブロック
		Counter_i = 0;	//上段 //
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 0; i < 1; i++) { //
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//比較用配列に候補の数字を格納
						for (l = 0; l < 9; l++) {
							Compare [l] = Mask_Rank [i, j, k, l];
							if (Compare [l] != 0) {
								//候補数字が何個かをカウント
								NumberCount++;
							}

						}

						//候補数字が2つ以上の場合
						if (NumberCount >= 2) {
							if (0 <= k && k < 3) {
								Counter_k = 0;
								num = 3;

							} else if (3 <= k && k < 6) {
								Counter_k = 3;
								num = 6;

							} else if (6 <= k && k < 9) {
								Counter_k = 6;
								num = 9;
							}

							do {
								do {
									//他の空白マスのチェック
									for (l = 0; l < 9; l++) {
										//比較する空白マスの配列以外の配列
										if (Counter_j == j && Counter_k == k) {
										} else {
											if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
												//9つの配列の内容を比較
												if (Compare [l] == Mask_Rank [Counter_i, Counter_j, Counter_k, l]) {
													IdenticallyCount++;

												}

												//9つすべてが一致していた時ペア（トリオ）と判定
												if (IdenticallyCount == 9) {
													Identically_Number_3 [Counter_i, Counter_j, Counter_k] = true;

													//ペア以外の候補数字からペアの数字を削除制御
													IdenticallyFlag = true;
													IdenticallyCount = 0;
												} else {
													IdenticallyCount = 0;
												}
											}
										}
									}
									Counter_k++;
								
								} while(Counter_k < num);
	
								Counter_j++;
								Counter_k = Counter_k - 3;
						
							} while(Counter_j <= 2);
							Counter_j = 0;
						}
							
						//ペア以外の候補数字からペアの数字を削除
						if (IdenticallyFlag == true) {
							
							do {
								do {
									//空白マスをチェック(比較を行ったマスとペアになったマスを除く)
									if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
										if (Identically_Number_3 [Counter_i, Counter_j, Counter_k] != true) {
											if (Counter_j == j && Counter_k == k) {
											} else {
												for (l = 0; l < 9; l++) {
													//他のペア以外の候補数字からペアの数字を削除
													if (Mask_Rank [Counter_i, Counter_j, Counter_k, l] == Compare [l]) {
														Mask_Rank [Counter_i, Counter_j, Counter_k, l] = 0;
													}
												}
											}
										}
									}
											
									Counter_k++;
						
								} while(Counter_k < num);

								Counter_j++;
								Counter_k = Counter_k - 3;
						
							} while(Counter_j <= 2);
							IdenticallyFlag = false;

						}
					
						//初期化
						NumberCount = 0;
						Counter_j = 0;
						Counter_k = 0;
						num = 0;
						for (m = 0; m < 3; m++) {
							for (n = 0; n < 9; n++) {
								Identically_Number_3 [Counter_i, m, n] = false;
							}
						}

					}
				}
			}
		}

		Setting ();

		yield return null;

	}

	IEnumerator Solution_Step10(){
		//中段3つの3x3ブロック
		Counter_i = 1;	//中段
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 1; i < 2; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//比較用配列に候補の数字を格納
						for (l = 0; l < 9; l++) {
							Compare [l] = Mask_Rank [i, j, k, l];
							if (Compare [l] != 0) {
								//候補数字が何個かをカウント
								NumberCount++;
							}

						}

						//候補数字が2つ以上の場合
						if (NumberCount >= 2) {
							if (0 <= k && k < 3) {
								Counter_k = 0;
								num = 3;

							} else if (3 <= k && k < 6) {
								Counter_k = 3;
								num = 6;

							} else if (6 <= k && k < 9) {
								Counter_k = 6;
								num = 9;
							}

							do {
								do {
									//他の空白マスのチェック
									for (l = 0; l < 9; l++) {
										//比較する空白マスの配列以外の配列
										if (Counter_j == j && Counter_k == k) {
										} else {
											if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
												//9つの配列の内容を比較
												if (Compare [l] == Mask_Rank [Counter_i, Counter_j, Counter_k, l]) {
													IdenticallyCount++;

												}

												//9つすべてが一致していた時ペア（トリオ）と判定
												if (IdenticallyCount == 9) {
													Identically_Number_3 [Counter_i, Counter_j, Counter_k] = true;

													//ペア以外の候補数字からペアの数字を削除制御
													IdenticallyFlag = true;
													IdenticallyCount = 0;
												} else {
													IdenticallyCount = 0;
												}
											}
										}
									}
									Counter_k++;
							
								} while(Counter_k < num);

								Counter_j++;
								Counter_k = Counter_k - 3;
							
							} while(Counter_j <= 2);
							Counter_j = 0;
						}

						//ペア以外の候補数字からペアの数字を削除
						if (IdenticallyFlag == true) {

							do {
								do {
									//空白マスをチェック(比較を行ったマスとペアになったマスを除く)
									if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
										if (Identically_Number_3 [Counter_i, Counter_j, Counter_k] != true) {
											if (Counter_j == j && Counter_k == k) {
											} else {
												for (l = 0; l < 9; l++) {
													//他のペア以外の候補数字からペアの数字を削除
													if (Mask_Rank [Counter_i, Counter_j, Counter_k, l] == Compare [l]) {
														Mask_Rank [Counter_i, Counter_j, Counter_k, l] = 0;
													}
												}
											}
										}
									}

									Counter_k++;

								} while(Counter_k < num);

								Counter_j++;
								Counter_k = Counter_k - 3;
							
							} while(Counter_j <= 2);
							IdenticallyFlag = false;

						}

						//初期化
						NumberCount = 0;
						Counter_j = 0;
						Counter_k = 0;
						num = 0;
						for (m = 0; m < 3; m++) {
							for (n = 0; n < 9; n++) {
								Identically_Number_3 [Counter_i, m, n] = false;
							}
						}

					}
				}
			}
		}

		Setting ();

		yield return null;

	}

	IEnumerator Solution_Step11(){
		//下段3つの3x3ブロック
		Counter_i = 2;	//下段
		Counter_j = 0;
		Counter_k = 0;
		num = 0;

		for (i = 2; i < 3; i++) {
			for (j = 0; j < 3; j++) {
				for (k = 0; k < 9; k++) {
					if (_1to9Rank [i, j, k] == 0) {

						//比較用配列に候補の数字を格納
						for (l = 0; l < 9; l++) {
							Compare [l] = Mask_Rank [i, j, k, l];
							if (Compare [l] != 0) {
								//候補数字が何個かをカウント
								NumberCount++;
							}

						}

						//候補数字が2つ以上の場合
						if (NumberCount >= 2) {
							if (0 <= k && k < 3) {
								Counter_k = 0;
								num = 3;

							} else if (3 <= k && k < 6) {
								Counter_k = 3;
								num = 6;

							} else if (6 <= k && k < 9) {
								Counter_k = 6;
								num = 9;
							}

							do {
								do {
									//他の空白マスのチェック
									for (l = 0; l < 9; l++) {
										//比較する空白マスの配列以外の配列
										if (Counter_j == j && Counter_k == k) {
										} else {
											if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
												//9つの配列の内容を比較
												if (Compare [l] == Mask_Rank [Counter_i, Counter_j, Counter_k, l]) {
													IdenticallyCount++;

												}

												//9つすべてが一致していた時ペア（トリオ）と判定
												if (IdenticallyCount == 9) {
													Identically_Number_3 [Counter_i, Counter_j, Counter_k] = true;

													//ペア以外の候補数字からペアの数字を削除制御
													IdenticallyFlag = true;
													IdenticallyCount = 0;
												} else {
													IdenticallyCount = 0;
												}
											}
										}
									}
									Counter_k++;
							
								} while(Counter_k < num);

								Counter_j++;
								Counter_k = Counter_k - 3;
						
							} while(Counter_j <= 2);
							Counter_j = 0;
						}

						//ペア以外の候補数字からペアの数字を削除
						if (IdenticallyFlag == true) {

							do {
								do {
									//空白マスをチェック(比較を行ったマスとペアになったマスを除く)
									if (_1to9Rank [Counter_i, Counter_j, Counter_k] == 0) {
										if (Identically_Number_3 [Counter_i, Counter_j, Counter_k] != true) {
											if (Counter_j == j && Counter_k == k) {
											} else {
												for (l = 0; l < 9; l++) {
													//他のペア以外の候補数字からペアの数字を削除
													if (Mask_Rank [Counter_i, Counter_j, Counter_k, l] == Compare [l]) {
														Mask_Rank [Counter_i, Counter_j, Counter_k, l] = 0;
													}
												}
											}
										}
									}

									Counter_k++;
								
								} while(Counter_k < num);

								Counter_j++;
								Counter_k = Counter_k - 3;
					
							} while(Counter_j <= 2);
							IdenticallyFlag = false;

						}

						//初期化
						NumberCount = 0;
						Counter_j = 0;
						Counter_k = 0;
						num = 0;
						for (m = 0; m < 3; m++) {
							for (n = 0; n < 9; n++) {
								Identically_Number_3 [Counter_i, m, n] = false;
							}
						}

					}
				}
			}
		}

		Setting ();

		yield return null;

	}

	//残り候補から確定数字を割り出す
	public void Setting(){
		//残り候補のチェック
		for (i = 0; i < 3; i++) {
			for (k = 0; k < 3; k++) {
				for (j = 0; j < 9; j++) {
					Count = 0;
					//空白マスの場合
					if (_1to9Rank [i, k, j] == 0) {
						for (l = 0; l < 9; l++) {
							//残りの候補数字をカウント
							if (Mask_Rank [i, k, j, l] != 0) {
								Count++;
							}
						}

						//残りの候補数字が1つのみの場合
						if (Count == 1) {
							for (l = 0; l < 9; l++) {
								if (Mask_Rank [i, k, j, l] != 0) {
									//候補数字を確定数字とする
									_1to9Rank [i, k, j] = Mask_Rank [i, k, j, l];
								}
							}
						} else {
							Count = 0;
						}
					}
				}
			}
		}
	}
		
}
