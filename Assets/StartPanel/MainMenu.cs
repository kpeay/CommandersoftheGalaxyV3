using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Start game playing
	public void PlayButtonClicked () {
        SceneManager.LoadScene("Test");
	}

    // Stop Game playing
    public void QuitButtonClicked () {
        Debug.Log("Application Quit Button Clicked");
        Application.Quit();
	}
}
