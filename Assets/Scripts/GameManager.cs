using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static int m_scores;
    public Text m_scoresUI;
    public Text m_gameOverUI;
    public GameObject m_bg;

	public GameObject m_figure;
	public GameObject m_viewBlock;
	public static bool m_isFigureExist;
	public static float m_speed;
    public bool m_isGameOver;

	public static int[,] m_gameField;
	public SpriteRenderer[,] m_gameFieldView;

    public int m_currentFigure = 0;

	void Awake() 
	{
		m_speed = 1f;

		m_gameField = new int[10, 23];
		m_gameFieldView = new SpriteRenderer[10, 23];

		for (int x = 0; x < 10; x++) 
		{
			for (int y = 0; y < 23; y++) 
			{
				GameObject tempBlockObj = Instantiate (m_viewBlock, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				tempBlockObj.transform.parent = gameObject.transform;
				m_gameFieldView [x, y] = tempBlockObj.GetComponent<SpriteRenderer>();
			}
		}

        m_isGameOver = false;

    }

	void Start() 
	{
		SpawnFigure ();
		m_isFigureExist = true;
        m_scores = 0;
	}

	void Update() 
	{
		if (!m_isFigureExist && !m_isGameOver) 
		{
			SpawnFigure ();
			m_isFigureExist = true;
		}

		RenderGameField ();

		if (CheckGameFieldOverflow())
        {
            m_isGameOver = true;
            m_bg.GetComponent<SpriteRenderer>().color = new Color32(255, 100, 100, 255);
            m_scoresUI.gameObject.SetActive(false);
            m_gameOverUI.gameObject.SetActive(true);
            m_gameOverUI.text = "GAME OVER\nyour final score is\n " + m_scores;
        }
			

		CheckForFullRows ();

        m_scoresUI.text = "scores: " + m_scores.ToString();
	}

	void SpawnFigure() 
	{
        m_currentFigure = Random.Range(1, 9);

        GameObject tempFigure = Instantiate (m_figure, new Vector3 (Random.Range(4, 6), 19, 90), Quaternion.identity) as GameObject;
		tempFigure.GetComponent<FigureManager> ().m_figureID = m_currentFigure;
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
        m_scores += 10 * rowsNumbers.Count;
	}
}