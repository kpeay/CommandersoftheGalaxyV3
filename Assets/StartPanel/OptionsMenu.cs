using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Toggle playerTank1;
    public Toggle playerSoldier1;
    public Toggle playerRange1;

    // Human Race Selected
    public void HumansButtonCLicked()
    {
        
        Debug.Log("Humans Button Clicked");
    }

    // Gungan Race Selected
    public void GungansButtonCLicked () {
        Debug.Log("Gungans Button Clicked");
	}

    // Wookies Race Selected
    public void WookiesButtonCLicked()
    {
        Debug.Log("Wookies Button Clicked");
    }

}
