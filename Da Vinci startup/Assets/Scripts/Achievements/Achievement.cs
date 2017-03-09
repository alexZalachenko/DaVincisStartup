public class Achievement{

    public bool Unlocked
    {
        set;
        get;
    }
    public string Text
    {
        private set;
        get;
    }
    public string ImageSource
    {
        private set;
        get;
    }
    public string ID
    {
        private set;
        get;
    }

    public Achievement(string p_text, string p_imageSource, string p_ID)
    {
        Text = p_text;
        ImageSource = p_imageSource;
        Unlocked = false;
        ID = p_ID;
    }
}
