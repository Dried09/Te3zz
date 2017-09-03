using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager m_UImngr;

    public Text m_title;

	//HUD
	public Image m_hudPanel;
	public Text m_scoreTextUI;
	public Text m_hiscoreText;
	public Text m_difficultyHudText;
	public Text m_rowsProgress;
	public Button m_menuButton;
	public Text m_messageText;

    //Панель та кнопки керування
    public Image m_controlButtonsPanel;
    public Button m_leftButton;
    public Button m_rightButton;
    public Button m_downButton;
    public Button m_rotateButton;

    public bool m_isLeftHold;
    public bool m_isRightHold;
    public bool m_isDownHold;

	//Меню в грі
	public Image m_inGameMenuPanel;
	public Button m_inGameResumeButton;
	public Button m_inGameRestartButton;
	public Button m_inGameReturnToMenuButton;

	//Головне меню
	public Image m_menuPanel;
	public Button m_startGameButton;
	public Button m_quitGameButton;
	public Text m_hiscoreTextMenu;
	public Text m_difficultyText;
	public Slider m_difficultySlider;

	//Меню кінця гри
	public Image m_gameOverPanel;
	public Text m_gameOverTextUI;
	public Button m_returnToMenuButton;
	public Button m_restartButton;

	void Awake () 
	{
        m_isLeftHold = false;
        m_isRightHold = false;
        m_isDownHold = false;

    	DataManager.m_hiScores = PlayerPrefs.GetInt ("hiscore", 0);
		DataManager.m_difficulty = PlayerPrefs.GetInt("difficulty", 1);
		m_difficultySlider.value = DataManager.m_difficulty;
	

		m_UImngr = this;

		//Головне меню
		m_startGameButton.onClick.AddListener (() => 
			{
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.game);
				m_isLeftHold = false;
				m_isRightHold = false;
				m_isDownHold = false;
			}
		);

		m_quitGameButton.onClick.AddListener (() => 
			{
				Application.Quit();
			}
		);

		//Внутрішнє меню
		m_menuButton.onClick.AddListener (() => 
			{
				if(!m_inGameMenuPanel.gameObject.activeInHierarchy)
				{ 
					m_inGameMenuPanel.gameObject.SetActive(true);
                    m_controlButtonsPanel.gameObject.SetActive(false);
					Time.timeScale = 0f;
				}
			}
		);

		m_inGameResumeButton.onClick.AddListener (() => 
			{
				m_inGameMenuPanel.gameObject.SetActive(false);
                m_controlButtonsPanel.gameObject.SetActive(true);
                Time.timeScale = 1f;
				m_isLeftHold = false;
				m_isRightHold = false;
				m_isDownHold = false;
			}
		);

		m_inGameRestartButton.onClick.AddListener (() => 
			{
				m_inGameMenuPanel.gameObject.SetActive(false);
                m_controlButtonsPanel.gameObject.SetActive(true);
                Time.timeScale = 1f;
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.game);
				DataManager.m_scores = 0;
				m_isLeftHold = false;
				m_isRightHold = false;
				m_isDownHold = false;
			}
		);

		m_inGameReturnToMenuButton.onClick.AddListener (() => 
			{
				m_inGameMenuPanel.gameObject.SetActive(false);
				Time.timeScale = 1f;
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.stop);
			}
		);

		//Головне меню
		m_startGameButton.onClick.AddListener (() => 
			{
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.game);
                m_controlButtonsPanel.gameObject.SetActive(true);
            }
		);

		//Кінець гри меню
		m_restartButton.onClick.AddListener (() => 
			{
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.game);
				DataManager.m_scores = 0;
                m_controlButtonsPanel.gameObject.SetActive(true);
            }
		);

		m_returnToMenuButton.onClick.AddListener (() => 
			{
				GameManagerNew.m_gmMngr.ChangeState (GameManagerNew.State.menu);
			}
		);

		m_difficultySlider.onValueChanged.AddListener ((float value) => 
			{
				DataManager.m_difficulty = (int)m_difficultySlider.value;
				PlayerPrefs.SetInt ("difficulty", (int)DataManager.m_difficulty);
				PlayerPrefs.Save ();
			}
		);
	}

	void Update ()
    {
		m_scoreTextUI.text = "Scores: " + DataManager.m_scores.ToString();
		m_hiscoreText.text = "Hi-Scores: " + DataManager.m_hiScores.ToString ();
		m_difficultyText.text = "Difficulty: " + DataManager.m_difficulty.ToString ();
		m_difficultyHudText.text = "Difficulty: " + DataManager.m_difficulty.ToString();
		m_rowsProgress.text = "Rows downed: " + DataManager.m_rowsCompleted + "/" + DataManager.m_rowsToNextSpeed;
	
		if (DataManager.m_scores > DataManager.m_hiScores) 
		{
			DataManager.m_hiScores = DataManager.m_scores;
		}

		if (Input.GetKeyDown (KeyCode.L)) 
		{
			GameManagerNew.m_gmMngr.SetDifficulty (DataManager.m_difficulty + 1);
			Debug.Log (DataManager.m_difficulty);
		}
		if (Input.GetKeyDown (KeyCode.K)) 
		{
			GameManagerNew.m_gmMngr.SetDifficulty (DataManager.m_difficulty - 1);
			Debug.Log (DataManager.m_difficulty);
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			ViewMessage ("This is fuckin' test!");
		}
			
	}

    public void ButtonDown(string buttonName)
    {
        switch(buttonName)
        {
            case "left":
                m_isLeftHold = true;
                break;
            case "right":
                m_isRightHold = true;
                break;
            case "down":
                m_isDownHold = true;
                break;             
        }
    }

    public void ButtonUp(string buttonName)
    {
        switch (buttonName)
        {
            case "left":
                m_isLeftHold = false;
                break;
            case "right":
                m_isRightHold = false;
                break;
            case "down":
                m_isDownHold = false;
                break;
        }
    }

	//Обертання фігури тачем
    public void Rotate()
    {
        if(GameManagerNew.m_gmMngr.m_currentFigureManager != null)
        {
            GameManagerNew.m_gmMngr.m_currentFigureManager.Rotate();
        }
    }

	public void ViewMessage(string message)
	{
		m_messageText.text = message;
		m_messageText.CrossFadeAlpha (1f, 0f, false);
		m_messageText.CrossFadeAlpha (0f, 3f, false);
	}
}
