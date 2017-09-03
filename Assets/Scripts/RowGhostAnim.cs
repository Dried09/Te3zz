using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowGhostAnim : MonoBehaviour {

	Transform m_transform;

	void Awake()
	{
		m_transform = transform;
	}

	// Update is called once per frame
	void Update () {
		m_transform.position = new Vector3 (m_transform.position.x + 0.16f, m_transform.position.y, m_transform.position.z);
	}
}
