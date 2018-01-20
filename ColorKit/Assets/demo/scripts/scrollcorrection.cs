using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollcorrection : MonoBehaviour {

    public GameObject MasterPanel;

    float scrollSpeed;

    void Awake()
    {
        scrollSpeed = 1f;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 newPos = MasterPanel.transform.position;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.PageUp))
            newPos.y -= scrollSpeed;

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.PageDown))
            newPos.y += scrollSpeed;

        MasterPanel.transform.position = newPos;
    }
}
