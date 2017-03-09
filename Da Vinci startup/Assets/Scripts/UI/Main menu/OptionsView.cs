using UnityEngine;
using UnityEngine.UI;

public class OptionsView : MonoBehaviour {

    [SerializeField]
    private Image c_musicIcon;
    [SerializeField]
    private Image c_soundIcon;

    [SerializeField]
    private Sprite[] c_volumeImages;

    public void OnMusicIconClic()
    {
        SoundManager.Instance.ChangeMusicVolume();
        c_musicIcon.sprite = c_volumeImages[(int)SoundManager.Instance.MusicVolume];
    }

    public void OnSoundsIconClic()
    {
        SoundManager.Instance.ChangeSoundsVolume();
        c_soundIcon.sprite = c_volumeImages[(int)SoundManager.Instance.SoundsVolume];
    }

    public void OnDeleteAchievementClic()
    {
        AchievementsManager.Instance.DeleteAllAchievements();
    }
}
