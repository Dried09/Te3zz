using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerNew : MonoBehaviour {

	public static GameManagerNew m_gmMngr;

	public enum State {menu, game, stop}
	public State m_gameState;

	private delegate void Tick();
	private Tick tick;

	public GameObject m_figure;
	public GameObject m_viewBlock;
	public bool m_isFigureExist;

	public GameObject m_rowGhostAnim;

	public int[,] m_gameField;
	public SpriteRenderer[,] m_gameFieldView;
	public GameObject m_gameFieldContainer;

	public int m_currentFigure = 0;
	public FigureManager m_currentFigureManager;

	public bool m_isGameOver;
    public bool m_isWinGame;

	public bool m_isStateMenuInit;
	public bool m_isStateGameInit;
	public bool m_isStateStopInit;

    public float m_timer;

	void Awake() 
	{
		m_gmMngr = this;

		m_gameField = new int[10, 23];
		m_gameFieldView = new SpriteRenderer[10, 23];

		for (int x = 0; x < 10; x++) 
		{
			for (int y = 0; y < 23; y++) 
			{
				GameObject tempBlockObj = Instantiate (m_viewBlock, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				tempBlockObj.transform.parent = m_gameFieldContainer.transform;
				m_gameFieldView [x, y] = tempBlockObj.GetComponent<SpriteRenderer>();
			}
		}

		RenderGameField ();

		m_isStateMenuInit = false;
		m_isStateGameInit = false;
		m_isStateStopInit = false;
	}
		
	void Start () 
	{
		ChangeState (State.menu);
        m_timer = 0f;
	}

	void Update () 
	{
		tick ();
	}

	//STATE MACHINE методи
	//Метод зміни стану
	public void ChangeState(State nextState) 
	{
		Debug.Log("State changed to: " + nextState.ToString());
		m_gameState = nextState;

		switch (m_gameState) 
		{
		case State.menu:
			tick = ToStateMenu;
			break;
		case State.game:
			tick = ToGame;
			break;
		case State.stop:
			tick = ToStop;
			break;
		}

		m_isStateMenuInit = false;
		m_isStateGameInit = false;
		m_isStateStopInit = false;

		UIManager.m_UImngr.m_hudPanel.gameObject.SetActive (false);
		UIManager.m_UImngr.m_menuPanel.gameObject.SetActive (false);
		UIManager.m_UImngr.m_gameOverPanel.gameObject.SetActive (false);
	}

	//Стан menu
	void ToStateMenu()
	{
		//Ініціалізація
		if (!m_isStateMenuInit) 
		{
			if(m_currentFigureManager != null) Destroy (m_currentFigureManager.gameObject);
			GenerateEmptyGameField ();
			RenderGameField ();
			UIManager.m_UImngr.m_difficultySlider.value = DataManager.m_difficulty;
			UIManager.m_UImngr.m_hiscoreTextMenu.text = "Current Hi-Score: " + DataManager.m_hiScores.ToString ();
			UIManager.m_UImngr.m_menuPanel.gameObject.SetActive (true);

			m_isStateMenuInit = true;
		}

        m_timer += Time.deltaTime;
        if(m_timer > 0.2f)
        {
            UIManager.m_UImngr.m_title.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1);
            m_timer = 0;
        }
	}

	//Стан game
	void ToGame()
	{
		//Ініціалізація
		if (!m_isStateGameInit) 
		{
			SetDifficulty (DataManager.m_difficulty);
            if (m_currentFigureManager != null) Destroy(m_currentFigureManager.gameObject);
			m_isGameOver = false;
			GenerateEmptyGameField ();
			UIManager.m_UImngr.m_hudPanel.gameObject.SetActive (true);
			SpawnFigure ();
			m_isFigureExist = true;
			UIManager.m_UImngr.ViewMessage ("Difficulty " + DataManager.m_difficulty);

			m_isStateGameInit = true;
		}
			
		if (!m_isFigureExist && !m_isGameOver) 
		{
			SpawnFigure ();
			m_isFigureExist = true;
		}

		m_currentFigureManager.CustomUpdate ();
		RenderGameField ();

		if (CheckGameFieldOverflow())
		{
			m_isGameOver = true;
			ChangeState (State.stop);
		}
			
		CheckForFullRows ();

		CheckForGameProgress ();
	}

	//Стан stop
	void ToStop()
	{
		//Ініціалізація
		if (!m_isStateStopInit) 
		{
			if (m_isGameOver) {
				UIManager.m_UImngr.m_gameOverPanel.gameObject.SetActive (true);
				UIManager.m_UImngr.m_gameOverPanel.color = new Color32 (255, 100, 100, 113);
				UIManager.m_UImngr.m_gameOverTextUI.text = "YOU LOSE\nyour final score is\n" + DataManager.m_scores;
				DataManager.m_scores = 0;
			} 
            else if(m_isWinGame)
            {
                UIManager.m_UImngr.m_gameOverPanel.gameObject.SetActive(true);
                UIManager.m_UImngr.m_gameOverPanel.color = new Color32(100, 255, 100, 113);
                UIManager.m_UImngr.m_gameOverTextUI.text = "YOU WIN!\nyour final score is\n" + DataManager.m_scores;
                DataManager.m_scores = 0;
            }
			else 
			{
				UIManager.m_UImngr.m_gameOverPanel.gameObject.SetActive (true);
				UIManager.m_UImngr.m_gameOverPanel.color = new Color32 (255, 255, 255, 113);
				UIManager.m_UImngr.m_gameOverTextUI.text = "YOU GAVE UP\nyour final score is\n" + DataManager.m_scores;
				DataManager.m_scores = 0;
			}

			PlayerPrefs.SetInt ("hiscore", DataManager.m_hiScores);
			PlayerPrefs.Save ();

			m_isStateStopInit = true;
		}
	}

	//РОБОЧІ МЕТОДИ
	void GenerateEmptyGameField() 
	{
		m_gameField = new int[10, 23];
	}

	void SpawnFigure() 
	{
		m_currentFigure = Random.Range(1, 10);

		GameObject tempFigure = Instantiate (m_figure, new Vector3 (Random.Range(4, 6), 19, 90), Quaternion.identity) as GameObject;
		m_currentFigureManager = tempFigure.GetComponent<FigureManager>();
		m_currentFigureManager.m_figureID = m_currentFigure;
		tempFigure.SetActive (true);
	}

	void RenderGameField() 
	{
		for (int x = 0; x < 10; x++) 
		{
			for (int y = 0; y < 23; y++) 
			{
				if (m_gameField [x, y] == 0) 
				{
					m_gameFieldView [x, y].color = new Color32 (255, 255, 255, 0);
				} 
				else if (m_gameField [x, y] == 1) 
				{
					m_gameFieldView [x, y].color = new Color32 (255, 113, 113, 255);
				}
				else if (m_gameField [x, y] == 2 || m_gameField [x, y] == 3) 
				{
					m_gameFieldView [x, y].color = new Color32 (113, 255, 113, 255);
				}
				else if (m_gameField [x, y] == 4) 
				{
					m_gameFieldView [x, y].color = new Color32 (113, 113, 255, 255);
				}
				else if (m_gameField [x, y] == 5) 
				{
					m_gameFieldView [x, y].color = new Color32 (255, 255, 113, 255);
				}
				else if (m_gameField [x, y] == 6 || m_gameField [x, y] == 7) 
				{
					m_gameFieldView [x, y].color = new Color32 (113, 255, 255, 255);
				}
				else if (m_gameField [x, y] == 8) 
				{
					m_gameFieldView [x, y].color = new Color32 (255, 113, 255, 255);
				}
                else if (m_gameField [x, y] == 9)
                {
                    m_gameFieldView[x, y].color = new Color32(225, 225, 225, 255);
                }
                else
				{
					m_gameFieldView [x, y].color = new Color32 (255, 255, 255, 255);
				}
			}
		}
	}

	bool CheckGameFieldOverflow() 
	{
		for (int x = 0; x < 10; x++) 
		{
			if (m_gameField [x, 19] != 0)
				return true;			
		}
		return false;
	}

	void CheckForFullRows() 
	{
		List<int> rowsToDelete = new List<int> ();
		bool fullRow = true;
		for (int y = 0; y < 18; y++) 
		{
			for (int x = 0; x < 10; x++) 
			{
				if (m_gameField [x, y] == 0) 
				{
					fullRow = false;
				}
			}
			if (fullRow) 
			{
				rowsToDelete.Add (y);
			} 
			else 
			{
				fullRow = true;
			}
		}
		if (rowsToDelete.Count > 0) 
		{
			//Do Animation
			AnimateDeletionRows(rowsToDelete);
			DeleteAndSlideRows (rowsToDelete);

		}
	}

	void DeleteAndSlideRows(List<int> rowsNumbers) 
	{
		//Delete full rows
		for (int i = 0; i < rowsNumbers.Count; i++) 
		{
			for (int x = 0; x < 10; x++)
			{
				m_gameField [x, rowsNumbers [i]] = 0;
			}
		}
		//Create new array of game field
		int[,] oldGameField = m_gameField;
		int[,] newGameField = new int[10, 23];
		int nextRowCopyIn = 0;

		//Add containing blocks without empty rows to new array
		for (int y = 0; y < 23; y++) 
		{
			if (rowsNumbers.Contains (y)) 
			{
				continue;
			}
			for (int x = 0; x < 10; x++) 
			{
				newGameField [x, nextRowCopyIn] = oldGameField [x, y];
			}
			nextRowCopyIn++;
		}
		m_gameField = newGameField;
		DataManager.m_rowsCompleted += rowsNumbers.Count;
		if (rowsNumbers.Count == 1)
			DataManager.m_scores += 10 * DataManager.m_difficulty;
		else if (rowsNumbers.Count == 2)
			DataManager.m_scores += ((10 * DataManager.m_difficulty) * rowsNumbers.Count) + ((10 * DataManager.m_difficulty) / 2);
		else if (rowsNumbers.Count == 3)
			DataManager.m_scores += ((10 * DataManager.m_difficulty) * rowsNumbers.Count) + (10 * DataManager.m_difficulty);
		else if (rowsNumbers.Count == 4)
			DataManager.m_scores += ((10 * DataManager.m_difficulty) * rowsNumbers.Count) + ((10 * DataManager.m_difficulty) * 2);
		else
			Debug.Log ("Error in calculation score");

	}
	//Керування анімованим знищенням рядків
	void AnimateDeletionRows(List<int> rowsNumbers)
	{
		for (int i = 0; i < rowsNumbers.Count; i++) 
		{
			GameObject rowGhostAnim = Instantiate(m_rowGhostAnim, new Vector3(0, rowsNumbers[i], 90), Quaternion.identity) as GameObject;
			Destroy (rowGhostAnim, 1f);
		}
	}

	//Задає складність гри та всі повязані змінні
	public void SetDifficulty(int newValue)
	{
		if (newValue > 0 && newValue < 11) 
		{
			switch (newValue) 
			{
			case 1:
				DataManager.m_difficulty = 1;
				DataManager.m_speed = 1f;
				DataManager.m_rowsToNextSpeed = 15;
				break;
			case 2:
				DataManager.m_difficulty = 2;
				DataManager.m_speed = 1.5f;
				DataManager.m_rowsToNextSpeed = 13;
				break;
			case 3:
				DataManager.m_difficulty = 3;
				DataManager.m_speed = 2f;
				DataManager.m_rowsToNextSpeed = 11;
				break;
			case 4:
				DataManager.m_difficulty = 4;
				DataManager.m_speed = 2.5f;
				DataManager.m_rowsToNextSpeed = 10;
				break;
			case 5:
				DataManager.m_difficulty = 5;
				DataManager.m_speed = 3f;
				DataManager.m_rowsToNextSpeed = 9;
				break;
			case 6:
				DataManager.m_difficulty = 6;
				DataManager.m_speed = 3.5f;
				DataManager.m_rowsToNextSpeed = 8;
				break;
			case 7:
				DataManager.m_difficulty = 7;
				DataManager.m_speed = 4f;
				DataManager.m_rowsToNextSpeed = 7;
				break;
			case 8:
				DataManager.m_difficulty = 8;
				DataManager.m_speed = 4.5f;
				DataManager.m_rowsToNextSpeed = 6;
				break;
			case 9:
				DataManager.m_difficulty = 9;
				DataManager.m_speed = 5f;
				DataManager.m_rowsToNextSpeed = 5;
				break;
			case 10:
				DataManager.m_difficulty = 10;
				DataManager.m_speed = 6f;
				DataManager.m_rowsToNextSpeed = 3;
				break;
			}
			//UIManager.m_UImngr.m_rowsToNextSpeed.text = "Rows to next: " + DataManager.m_rowsToNextSpeed.ToString();
		}
		else 
		{
            m_isWinGame = true;
            ChangeState(State.stop);
		}
	}

	//Кожен кадр стану гри співставляє кількість зібраних рядків і необхідних для переходу на інший рівень
	void CheckForGameProgress()
	{
		if (DataManager.m_rowsCompleted >= DataManager.m_rowsToNextSpeed) 
		{
			int tempRowsOverflow = 0;
			if (DataManager.m_rowsCompleted - DataManager.m_rowsToNextSpeed > 0)
				tempRowsOverflow = DataManager.m_rowsCompleted - DataManager.m_rowsToNextSpeed;
			SetDifficulty (DataManager.m_difficulty + 1);
			DataManager.m_rowsCompleted = 0 + tempRowsOverflow;
			UIManager.m_UImngr.ViewMessage ("Difficulty " + DataManager.m_difficulty);
		}
	}
}
