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
        Application.OpenURL("market://details?id=" + p_game);
    }
}
