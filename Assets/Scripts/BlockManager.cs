using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

	Color m_colorVisible;
	Color m_colorUnvisible;
	float m_timer;
	SpriteRenderer m_sprite;
    public bool m_isSuperPoint;

    void Awake()
    {
    }

	void Start () {
		m_sprite = GetComponent<SpriteRenderer> ();
		m_colorVisible = m_sprite.color;
		m_colorUnvisible = new Color (m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0);
		m_sprite.color = m_colorUnvisible;
        if (m_isSuperPoint) m_sprite.sortingOrder = 2;
    }
	

	void Update () {
		m_timer += Time.deltaTime;

        if(m_isSuperPoint)
        {
            if (m_timer >= 0.25f && transform.position.y < 18)
            {
                if (m_sprite.color == m_colorUnvisible) m_sprite.color = m_colorVisible;
                else if (m_sprite.color == m_colorVisible) m_sprite.color = m_colorUnvisible;

                m_timer = 0f;
            }
            else if(transform.position.y > 18)
            {
                m_sprite.color = m_colorUnvisible;
            }
        }
        else
        {
            if (transform.position.y > 18)
            {
                m_sprite.color = m_colorUnvisible;
            }
            else
            {
                m_sprite.color = m_colorVisible;
            }
        }   
    }
}
