using UnityEngine.SceneManagement;
using UnityEngine;

public class Misc : MonoBehaviour {

    [SerializeField]
    GameObject[] c_gameobjectsToDisable;
    [SerializeField]
    GameObject c_loadingIcon;

	public void StartGame()
    {
        foreach (GameObject t_gameObject in c_gameobjectsToDisable)
            t_gameObject.SetActive(false);
        c_loadingIcon.SetActive(true);
        SceneManager.LoadScene("Game");
    }
}
