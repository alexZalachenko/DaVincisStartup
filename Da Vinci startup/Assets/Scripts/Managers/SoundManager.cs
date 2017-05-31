using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager> {

    public enum Volume
    {
        Disabled,
        Medium,
        High
    };
    public enum Scenes
    {
        Lodge,
        Palace,
        Harbour,
        Streets,
        Workshop
    };

    public Volume MusicVolume
    {
        private set;
        get;
    }
    public Volume SoundsVolume
    {
        private set;
        get;
    }
    [SerializeField]
    private AudioSource c_audioSource;
    [SerializeField]
    private AudioClip[] c_audioClips;
    [SerializeField]
    private float c_volumeChangeFrecuency = 0.1f;//is the fadeOut/In smooth or stepped?
    [SerializeField]
    private float c_volumeChangeTime = 2;//total time it will take to fadeOut/In the clip
    AudioClip c_newClip;

    private void Start()
    {
        MusicVolume = Volume.High;
        SoundsVolume = Volume.High;
    }

    public void ChangeMusicVolume()
    {
        --MusicVolume;
        if (MusicVolume < 0)
            MusicVolume = Volume.High;
        switch (MusicVolume)
        {
            case Volume.Disabled:
                c_audioSource.volume = 0;
                break;
            case Volume.Medium:
                c_audioSource.volume = 0.5f;
                break;
            case Volume.High:
                c_audioSource.volume = 1;
                break;
        }
        
    }

    public void ChangeSoundsVolume()
    {
        --SoundsVolume;
        if (SoundsVolume < 0)
            SoundsVolume = Volume.High;

        
    }

    public void OnSceneLoaded(Scenes p_scene)
    {
        switch (p_scene)
        {
            case Scenes.Lodge:
                c_newClip = c_audioClips[0];
                break;
            case Scenes.Palace:
                c_newClip = c_audioClips[1];
                break;
            case Scenes.Harbour:
                c_newClip = c_audioClips[2];
                break;
            case Scenes.Streets:
                c_newClip = c_audioClips[3];
                break;
            case Scenes.Workshop:
                c_newClip = c_audioClips[4];
                break;
        }
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float t_decrease = c_audioSource.volume / (c_volumeChangeTime / c_volumeChangeFrecuency);
        while (c_audioSource.volume > 0)
        {
            c_audioSource.volume -= t_decrease;
            yield return new WaitForSeconds(c_volumeChangeFrecuency);
        }

        c_audioSource.volume = 0;
        c_audioSource.clip = c_newClip;
        c_audioSource.Play();
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float t_newVolume = 1;
        switch (MusicVolume)
        {
            case Volume.Disabled:
                t_newVolume = 0;
                break;
            case Volume.Medium:
                t_newVolume = 0.5f;
                break;
            case Volume.High:
                t_newVolume = 1;
                break;
        }
        float t_increase = t_newVolume / (c_volumeChangeTime / c_volumeChangeFrecuency);
        while (c_audioSource.volume < t_newVolume)
        {
            c_audioSource.volume += t_increase;
            yield return new WaitForSeconds(c_volumeChangeFrecuency);
        }
        c_audioSource.volume = t_newVolume;
    }
}
