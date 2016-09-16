using UnityEngine;
using System.Collections;

//共通のデータ保存処理や共通の処理の制御
public class DataScript : MonoBehaviour {
	//他のスクリプトとデータをやり取り
	public static int GameTimeSetting = 5;
	public static int BreakMasu = 20;
	public static string GameMode = "NoTimeMode";
	public static string TypeMode = "Normal";

	//他のスクリプトからのベストタイム値を取得
	public static float Score = 0;
	//一番早いベストタイムを返す
	public static float DeliverBestScore;

	GameObject ManegeObj;

	//保存（セーブ用）
	static float BestScore_NoTimeMode;
	static float BestScore_TimeMode;
	static float BestScore_AccelMode;
	static float BestScore_NoMaskMode;
	static float BestScore_AccelNoMaskMode;

	string NoTimeMode;
	string TimeMode;
	string AccelMode;
	string NoMaskMode;
	string AccelNoMaskMode;


	void Start () {
		BestScore_NoTimeMode = PlayerPrefs.GetFloat ("NoTimeMode", 0);
		BestScore_TimeMode = PlayerPrefs.GetFloat ("TimeMode", 0);
		BestScore_AccelMode = PlayerPrefs.GetFloat ("AccelMode", 0);
		BestScore_NoMaskMode = PlayerPrefs.GetFloat ("NoMaskMode", 0);
		BestScore_AccelNoMaskMode = PlayerPrefs.GetFloat ("AccelNoMaskMode", 0);

	}
	
	//受け取ったスコアをベストスコアと比較しベストスコア更新なら保存を行う.
	public static float BestScore(float Score,string Mode){
		switch (Mode) {
		case "NoTimeMode":
			if (Score >= BestScore_NoTimeMode) {
				DeliverBestScore = Score;
				PlayerPrefs.SetFloat ("NoTimeMode", Score);
			} else if (Score < BestScore_NoTimeMode) {
				DeliverBestScore = BestScore_NoTimeMode;
			}

			break;

		case "TimeMode":
			if (Score >= BestScore_TimeMode) {
				DeliverBestScore = Score;
				PlayerPrefs.SetFloat ("TimeMode", Score);
			} else if (Score < BestScore_TimeMode) {
				DeliverBestScore = BestScore_TimeMode;
			}
				

			break;

		case "AccelMode":
			if (Score >= BestScore_AccelMode) {
				DeliverBestScore = Score;
				PlayerPrefs.SetFloat ("AccelMode", Score);
			} else if (Score < BestScore_AccelMode) {
				DeliverBestScore = BestScore_AccelMode;
			}
				

			break;

		case "NoMaskMode":
			if (Score >= BestScore_NoMaskMode) {
				DeliverBestScore = Score;
				PlayerPrefs.SetFloat ("NoMaskMode", Score);
			} else if (Score < BestScore_NoMaskMode) {
				DeliverBestScore = BestScore_NoMaskMode;
			}
				

			break;

		case "AccelNoMaskMode":
			if (Score >= BestScore_AccelNoMaskMode) {
				DeliverBestScore = Score;
				PlayerPrefs.SetFloat ("AccelNoMaskMode", Score);
			} else if (Score < BestScore_AccelNoMaskMode) {
				DeliverBestScore = BestScore_AccelNoMaskMode;
			}
				

			break;
		}

		return(DeliverBestScore);


	}

	public void Delete(){
		PlayerPrefs.DeleteKey ("NoTimeMode");
		PlayerPrefs.DeleteKey ("TimeMode");
		PlayerPrefs.DeleteKey ("AccelMode");
		PlayerPrefs.DeleteKey ("NoMaskMode");
		PlayerPrefs.DeleteKey ("AccelNoMaskMode");

	}

}
