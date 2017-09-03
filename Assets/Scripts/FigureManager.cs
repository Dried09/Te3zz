using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureManager : MonoBehaviour {

	public int m_figureID;
    public GameObject m_blockPrefab;
	List<Transform> m_positions;
	Transform m_rotationAxisBlock;
	List<SpriteRenderer> m_sprites;
	FigureInfo m_figureInfo;
	private int m_nextRotationState;

	public float m_timer;
	public float m_timerMoveDown;
	public float m_timerMoveLeftRight;
	private bool m_isHoldDown;

    bool m_isEnteredInBlocks;
    bool m_isLeftBlocks;

	void Awake() 
	{
        m_figureInfo = new FigureInfo(m_figureID);
        if(m_figureID == 9)
        {
            m_isEnteredInBlocks = false;
            m_isLeftBlocks = false;
        }
        CreateBlocks(m_figureInfo.m_blocksCount);
        m_positions = new List<Transform> ();
		m_positions.AddRange(gameObject.GetComponentsInChildren<Transform>());
		m_positions.RemoveAt (0);
		m_rotationAxisBlock = m_positions [m_figureInfo.m_indexRotationAxisBlock];
		m_sprites = new List<SpriteRenderer> ();
		m_sprites.AddRange (gameObject.GetComponentsInChildren<SpriteRenderer> ());
		m_nextRotationState = 0;
		m_isHoldDown = false;
	}

	void Start () {
		m_timer = 0f;
		m_timerMoveDown = 0f;
		m_timerMoveLeftRight = 0f;
		foreach (SpriteRenderer sr in m_sprites) 
		{
			sr.color = m_figureInfo.m_blocksColor;
		}
		Rotate ();
	}

	public void CustomUpdate () {
		m_timerMoveLeftRight += Time.deltaTime * 10f;
		m_timer += Time.deltaTime;
		if(!m_isHoldDown) m_timerMoveDown += Time.deltaTime * DataManager.m_speed;
		else m_timerMoveDown += Time.deltaTime * 15f;
	
		//Move Left & Right
		//Single
		if (Input.GetButtonDown ("RightDir") && RightCheck()) 
		{
			transform.position = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
			m_timerMoveLeftRight = 0f;
		}
		if (Input.GetButtonDown ("LeftDir") && LeftCheck()) 
		{
			transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
			m_timerMoveLeftRight = 0f;
		}
		//Holding down
		if ((Input.GetButton ("RightDir") && RightCheck() && m_timerMoveLeftRight > 1) || (UIManager.m_UImngr.m_isRightHold && RightCheck() && m_timerMoveLeftRight > 1))
		{
			transform.position = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
		}
		if ((Input.GetButton ("LeftDir") && LeftCheck() && m_timerMoveLeftRight > 1) || (UIManager.m_UImngr.m_isLeftHold && LeftCheck() && m_timerMoveLeftRight > 1)) 
		{
			transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
		}

		//MoveDown
		//Auto
		if (m_timerMoveDown >= 1 && BottomCheck()) 
		{
			transform.position = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
			m_timerMoveDown = 0f;
		}
		//Forced
		if ((Input.GetAxisRaw ("Vertical") < 0 && BottomCheck ()) || (UIManager.m_UImngr.m_isDownHold && BottomCheck())) {
			//transform.position = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
			m_isHoldDown = true;
		} 
		else if (Input.GetAxisRaw ("Vertical") == 0) 
		{
			m_isHoldDown = false;
		}

		//Rotation
		if (Input.GetButtonDown ("Jump")) 
		{
			Rotate ();
		}

		//Refreshing timers
		if (m_timer > 1f) 
		{
			m_timer = 0f;
		}

		if (m_timerMoveLeftRight > 1f) 
		{
			m_timerMoveLeftRight = 0f;
		}
	}

	public void Rotate() 
	{
        if(RotationCheck())
        {
            for (int i = 0; i < m_positions.Count; i++)
            {
                m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                            m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                            m_positions[i].localPosition.z);
            }
            if(m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
            else m_nextRotationState = 0;
        }
        //інакше якщо !перевіка на оберт і позитивна перевірка на відскок то відскочити в залежності від сторони на необхідну відстань і повернутися
        else if(!RotationCheck() && BounceCheck())
        {
            if(m_figureID == 1)
            {
                if (m_rotationAxisBlock.position.x == 0)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    //regular rotation
                    #region
                    for (int i = 0; i < m_positions.Count; i++)
                    {
                        m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                                    m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                                    m_positions[i].localPosition.z);
                    }
                    if (m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
                    else m_nextRotationState = 0;
                    #endregion
                }
                else if (m_rotationAxisBlock.position.x == 8)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    //regular rotation
                    #region
                    for (int i = 0; i < m_positions.Count; i++)
                    {
                        m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                                    m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                                    m_positions[i].localPosition.z);
                    }
                    if (m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
                    else m_nextRotationState = 0;
                    #endregion
                }
                else if (m_rotationAxisBlock.position.x == 9)
                {
                    transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                    //regular rotation
                    #region
                    for (int i = 0; i < m_positions.Count; i++)
                    {
                        m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                                    m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                                    m_positions[i].localPosition.z);
                    }
                    if (m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
                    else m_nextRotationState = 0;
                    #endregion
                }
            }
            //Для решти фігур
            else
            {
                if(m_rotationAxisBlock.position.x == 0)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    //regular rotation
                    #region
                    for (int i = 0; i < m_positions.Count; i++)
                    {
                        m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                                    m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                                    m_positions[i].localPosition.z);
                    }
                    if (m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
                    else m_nextRotationState = 0;
                    #endregion
                }
                else if (m_rotationAxisBlock.position.x == 9)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    //regular rotation
                    #region
                    for (int i = 0; i < m_positions.Count; i++)
                    {
                        m_positions[i].localPosition = new Vector3(m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX,
                                                                    m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY,
                                                                    m_positions[i].localPosition.z);
                    }
                    if (m_nextRotationState + 1 < m_figureInfo.m_rotationInfos.Count) m_nextRotationState++;
                    else m_nextRotationState = 0;
                    #endregion
                }
            }
        }
	}

	bool LeftCheck() 
	{
		for (int i = 0; i < m_positions.Count; i++) 
		{
			if (m_positions [i].position.x == 0) 
			{
				return false;
			}
			if (GameManagerNew.m_gmMngr.m_gameField [(int)m_positions [i].position.x - 1, (int)m_positions [i].position.y] > 0) 
			{
				return false;
			}
		}
		return true;
	}

	bool RightCheck() 
	{
		for (int i = 0; i < m_positions.Count; i++) 
		{
			if (m_positions [i].position.x == 9) 
			{
				return false;
			}
			if (GameManagerNew.m_gmMngr.m_gameField [(int)m_positions [i].position.x + 1, (int)m_positions [i].position.y] > 0) 
			{
				return false;
			}
		}
		return true;
	}

	bool BottomCheck() 
	{
		for (int i = 0; i < m_positions.Count; i++) 
		{
            if (m_figureID != 9)
            {
                if (m_positions[i].position.y == 0)
                {
                    StopBlock();
                }
                if (m_positions[i].position.y != 0)
                {
                    if (GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y - 1] > 0)
                    {
                        StopBlock();
                    }
                }
            }
            else if(m_figureID == 9)
            {
                if (m_positions[0].position.y == 0 && GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y] == 0) StopBlock();
                else if (m_positions[0].position.y == 0 && GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y] > 0) StopBlockWithoutAddingToField();
                else if (m_positions[0].position.y != 0 && !m_isEnteredInBlocks && !m_isLeftBlocks && GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y] > 0)
                {
                    m_isEnteredInBlocks = true;
                }
                else if (m_positions[0].position.y != 0 && m_isEnteredInBlocks && !m_isLeftBlocks && GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y] == 0)
                {
                    m_isLeftBlocks = true;
                    if (GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y - 1] > 0 || m_positions[0].position.y - 1 == 0) StopBlock();
                }
                else if(m_positions[0].position.y != 0 && m_isEnteredInBlocks && m_isLeftBlocks)
                {
                    if (GameManagerNew.m_gmMngr.m_gameField[(int)m_positions[i].position.x, (int)m_positions[i].position.y - 1] > 0)
                    {
                        StopBlock();
                    }
                }
            }
		}
		return true;
	}

	bool RotationCheck() 
	{
		//Робимо виключення для фігури І, бо вона має асиметрію повороту на клітину більше вправо
		if (m_figureID == 1) {
			if (m_rotationAxisBlock.position.x == 0 || m_rotationAxisBlock.position.x == 9 || 
				m_rotationAxisBlock.position.x == 9 - 1 || m_rotationAxisBlock.position.y == 0) 
			{
				return false;
			}
		}
		//Для всіх інших однаково
		else 
		{
			if (m_rotationAxisBlock.position.x == 0 || m_rotationAxisBlock.position.x == 9 || m_rotationAxisBlock.position.y == 0) 
			{
				return false;
			}
		}

		if (GameManagerNew.m_gmMngr.m_gameField [(int)m_rotationAxisBlock.position.x - m_figureInfo.m_numLeftRotCheck, (int)m_rotationAxisBlock.position.y] > 0 || 
			GameManagerNew.m_gmMngr.m_gameField [(int)m_rotationAxisBlock.position.x + m_figureInfo.m_numRightRotCheck, (int)m_rotationAxisBlock.position.y] > 0 || 
			GameManagerNew.m_gmMngr.m_gameField [(int)m_rotationAxisBlock.position.x, (int)m_rotationAxisBlock.position.y - 1] > 0) 
			{
				return false;
			}

		return true;
	}

    bool BounceCheck()
    {
        Vector3 currentWorldPos = gameObject.transform.position;

        if (m_figureID == 1)
        {
            if (m_rotationAxisBlock.position.x == 0 && m_rotationAxisBlock.position.y > 1)
            {
                for (int i = 0; i < m_positions.Count; i++)
                {
                    int nextFigX = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX + (int)currentWorldPos.x + 1;
                    int nextFigY = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY + (int)currentWorldPos.y;

                    if (GameManagerNew.m_gmMngr.m_gameField[nextFigX, nextFigY] == 0) continue;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (m_rotationAxisBlock.position.x == 8 && m_rotationAxisBlock.position.y > 1)
            {
                for (int i = 0; i < m_positions.Count; i++)
                {
                    int nextFigX = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX + (int)currentWorldPos.x - 1;
                    int nextFigY = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY + (int)currentWorldPos.y;

                    if (GameManagerNew.m_gmMngr.m_gameField[nextFigX, nextFigY] == 0) continue;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (m_rotationAxisBlock.position.x == 9 && m_rotationAxisBlock.position.y > 1)
            {
                for (int i = 0; i < m_positions.Count; i++)
                {
                    int nextFigX = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX + (int)currentWorldPos.x - 2;
                    int nextFigY = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY + (int)currentWorldPos.y;

                    if (GameManagerNew.m_gmMngr.m_gameField[nextFigX, nextFigY] == 0) continue;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }
        else if(m_figureID != 4 && m_figureID != 8)
        {
            if (m_rotationAxisBlock.position.x == 0 && m_rotationAxisBlock.position.y > 1)
            {
                for (int i = 0; i < m_positions.Count; i++)
                {
                    int nextFigX = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX + (int)currentWorldPos.x + 1;
                    int nextFigY = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY + (int)currentWorldPos.y;

                    if (GameManagerNew.m_gmMngr.m_gameField[nextFigX, nextFigY] == 0) continue;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (m_rotationAxisBlock.position.x == 9 && m_rotationAxisBlock.position.y > 1)
            {
                for (int i = 0; i < m_positions.Count; i++)
                {
                    int nextFigX = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localX + (int)currentWorldPos.x - 1;
                    int nextFigY = m_figureInfo.m_rotationInfos[m_nextRotationState][i].m_localY + (int)currentWorldPos.y;

                    if (GameManagerNew.m_gmMngr.m_gameField[nextFigX, nextFigY] == 0) continue;
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        return false;
    }

	void StopBlock () 
	{
		GameManagerNew.m_gmMngr.m_isFigureExist = false;

		foreach (Transform block in m_positions) 
		{
			GameManagerNew.m_gmMngr.m_gameField [(int)block.position.x, (int)block.position.y] = m_figureID;
		}

		Destroy (gameObject);
	}

    void StopBlockWithoutAddingToField()
    {
        GameManagerNew.m_gmMngr.m_isFigureExist = false;

        Destroy(gameObject);
    }

    void CreateBlocks(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject block = Instantiate(m_blockPrefab, transform.position, Quaternion.identity); 
            if(m_figureID == 9)
            {
                block.GetComponent<BlockManager>().m_isSuperPoint = true;
            }
            block.transform.parent = gameObject.transform;
        }
    }

	void OnDestroy()
	{
		DataManager.m_scores += 1 * DataManager.m_difficulty;
	}
}
