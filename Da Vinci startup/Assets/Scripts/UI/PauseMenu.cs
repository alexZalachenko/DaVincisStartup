using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private GameObject c_pauseMenu;
    [SerializeField]
    private List<GameObject> c_pausableObjects;

	
    public void ChangePauseMenuState()
    {
        c_pauseMenu.SetActive(!c_pauseMenu.activeSelf);
        if (c_pauseMenu.activeSelf)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        for (int t_index = 0; t_index < c_pausableObjects.Count; t_index++)
            c_pausableObjects[t_index].SetActive(!c_pauseMenu.activeSelf);
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }	
}
