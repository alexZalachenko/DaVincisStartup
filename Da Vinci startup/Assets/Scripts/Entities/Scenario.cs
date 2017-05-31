using UnityEngine;

public class Scenario : MonoBehaviour {

    [SerializeField]
    SoundManager.Scenes c_scenario;

    private void OnEnable()
    {
        SoundManager.Instance.OnSceneLoaded(c_scenario);
    }
}
