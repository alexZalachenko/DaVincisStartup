using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesView : MonoBehaviour {


    public void OnGameButtonClicked(string p_game)
    {
        OpenGooglePlay(p_game);
    }

    private void OpenGooglePlay(string p_game)
    {
        //"market://details?q=pname:com.myCompany.myAppName/"
        Application.OpenURL("market://details?id=com.example.android");
    }
}
