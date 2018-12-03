using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour {

    // Start game playing
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}
