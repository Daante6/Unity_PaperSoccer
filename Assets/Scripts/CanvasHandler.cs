using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void restartLevel()
    {
		SceneManager.LoadScene(1);
    }
	public void backToMenu()
    {
		SceneManager.LoadScene(0);
    }
}
