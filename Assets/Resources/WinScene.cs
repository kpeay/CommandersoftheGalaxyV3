using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScene : MonoBehaviour {

    // Start game playing
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}
