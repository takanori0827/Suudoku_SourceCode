using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//UIを表示や非表示にしページをめくっているように制御
public class HowToPlayScript : MonoBehaviour {
	int i = 0;
	int Count = 0;
	string PanelInfo = "";
	public GameObject[] Choice_Text = new GameObject[1];
	public GameObject[] Description_Text = new GameObject[1];
	public GameObject[] Sudoku_Text = new GameObject[1];
	public GameObject[] GameRule_Text = new GameObject[4];
	public GameObject[] GameMode_Text = new GameObject[5];
	public GameObject[] GameType_Text = new GameObject[1];


	void Start () {
		PanelInfo = "";
		Choice_Text [0].SetActive (true);
		Description_Text [0].SetActive (true);
		Sudoku_Text [0].SetActive (false);
		GameType_Text [0].SetActive (false);

		for (i = 0; i < 4; i++) {
			GameRule_Text [i].SetActive (false);
		}

		for (i = 0; i < 5; i++) {
			GameMode_Text [i].SetActive (false);
		}
			
	}
		
	public void SudokuPanel(){
		Sudoku_Text [0].SetActive (true);

		Description_Text [0].SetActive (false);
		GameType_Text [0].SetActive (false);

		for (i = 0; i < 4; i++) {
			GameRule_Text [i].SetActive (false);
		}

		for (i = 0; i < 5; i++) {
			GameMode_Text [i].SetActive (false);
		}

	}

	public void GameRuluPanel(){
		PanelInfo = "GameRuluPanel";
		Count = 0;

		GameRule_Text [0].SetActive (true);

		Description_Text [0].SetActive (false);
		Sudoku_Text [0].SetActive (false);
		GameType_Text [0].SetActive (false);

		for (i = 1; i < 4; i++) {
			GameRule_Text [i].SetActive (false);
		}
		for (i = 0; i < 5; i++) {
			GameMode_Text [i].SetActive (false);
		}

	}

	public void GameModePanel(){
		PanelInfo = "GameModePanel";
		Count = 0;

		GameMode_Text [0].SetActive (true);

		Description_Text [0].SetActive (false);
		Sudoku_Text [0].SetActive (false);
		GameType_Text [0].SetActive (false);

		for (i = 0; i < 4; i++) {
			GameRule_Text [i].SetActive (false);
		}
		for (i = 1; i < 5; i++) {
			GameMode_Text [i].SetActive (false);
		}

	}

	public void GameTypePanel(){
		GameType_Text [0].SetActive (true);

		Description_Text [0].SetActive (false);
		Sudoku_Text [0].SetActive (false);

		for (i = 0; i < 4; i++) {
			GameRule_Text [i].SetActive (false);
		}
		for (i = 0; i < 5; i++) {
			GameMode_Text [i].SetActive (false);
		}

	}

	//一つ後のUIページに戻る
	public void NextPage(){
		Count++;
		switch (PanelInfo) {
		case "GameRuluPanel":
			if (Count == 1) {
				GameRule_Text [Count-1].SetActive (false);
				GameRule_Text [Count].SetActive (true);

			} else if (Count == 2) {
				GameRule_Text [Count-1].SetActive (false);
				GameRule_Text [Count].SetActive (true);

			} else if (Count == 3) {
				GameRule_Text [Count-1].SetActive (false);
				GameRule_Text [Count].SetActive (true);
			
			}

			if (Count > 3) {
				Count = 3;
			}

			break;

		case "GameModePanel":
			if (Count == 1) {
				GameMode_Text [Count - 1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 2) {
				GameMode_Text [Count - 1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 3) {
				GameMode_Text [Count - 1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 4) {
				GameMode_Text [Count - 1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			}

			if (Count > 4) {
				Count = 4;
			}

			break;
		}
				
	}

	//一つ前のUIページに戻る
	public void BackPage(){
		Count--;
		switch (PanelInfo) {
		case "GameRuluPanel":
			if (Count == 2) {
				GameRule_Text [Count + 1].SetActive (false);
				GameRule_Text [Count].SetActive (true);

			} else if (Count == 1) {
				GameRule_Text [Count + 1].SetActive (false);
				GameRule_Text [Count].SetActive (true);

			} else if (Count == 0) {
				GameRule_Text [Count + 1].SetActive (false);
				GameRule_Text [Count].SetActive (true);

			}

			if (Count < 0) {
				Count = 0;
			}

			break;

		case "GameModePanel":
			if (Count == 3) {
				GameMode_Text [Count+1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 2) {
				GameMode_Text [Count+1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 1) {
				GameMode_Text [Count+1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			} else if (Count == 0) {
				GameMode_Text [Count+1].SetActive (false);
				GameMode_Text [Count].SetActive (true);

			}

			if (Count < 0) {
				Count = 0;
			}

			break;
		}

	}

	public void Title(){
		Application.LoadLevel ("タイトル");

	}

}
