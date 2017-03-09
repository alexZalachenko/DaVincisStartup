public class SoundManager : GenericSingleton<SoundManager> {

    public enum Volume
    {
        Disabled,
        Medium,
        High
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

    private void Start()
    {
        MusicVolume = Volume.Medium;
        SoundsVolume = Volume.Medium;
    }

    public void ChangeMusicVolume()
    {
        --MusicVolume;
        if (MusicVolume < 0)
            MusicVolume = Volume.High;
    }

    public void ChangeSoundsVolume()
    {
        --SoundsVolume;
        if (SoundsVolume < 0)
            SoundsVolume = Volume.High;
    }
}
