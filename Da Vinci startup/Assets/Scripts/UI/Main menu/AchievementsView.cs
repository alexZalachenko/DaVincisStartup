using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsView : MonoBehaviour {

    #region VARIABLES
    private List<Achievement> c_achievements = null;
    [SerializeField]
    private List<Sprite> c_achievementSprites;
    private int c_selectedAchievement = 1;
    [SerializeField]
    Text c_achievementText;
    [SerializeField]
    Text c_achievementUnlockedCount;
    [SerializeField]
    Image c_leftAchievement;
    [SerializeField]
    Image c_midAchievement;
    [SerializeField]
    Image c_rightAchievement;
    #endregion

    private void OnEnable()
    {
        LoadAchievements();
    }


    private void SetAchievementImage(Image p_image, int p_index)
    {
        p_image.sprite = c_achievementSprites[p_index];
        if (!c_achievements[p_index].Unlocked)
            p_image.color = new Color(0.2f, 0.2f, 0.2f, 1);
        else
            p_image.color = new Color(1, 1, 1, 1);
    }

    private void LoadAchievements()
    {
        c_achievements = AchievementsManager.Instance.Achievements;
        SetAchievementImage(c_leftAchievement, c_selectedAchievement - 1);
        SetAchievementImage(c_midAchievement, c_selectedAchievement);
        SetAchievementImage(c_rightAchievement, c_selectedAchievement + 1);

        c_achievementText.text = c_achievements[c_selectedAchievement].Text;

        int t_unlockedAchievements = 0;
        for (int t_index = 0; t_index < c_achievements.Count; t_index++)
        {
            if (c_achievements[t_index].Unlocked)
                ++t_unlockedAchievements;
        }
        c_achievementUnlockedCount.text = "Has desbloqueado " + t_unlockedAchievements + " de " + c_achievements.Count + " logros";
    }

    public void NavigateLeft()
    {
        --c_selectedAchievement;
        if (c_selectedAchievement == -1)
            c_selectedAchievement = 0;
        if (c_selectedAchievement == 0)
            c_leftAchievement.color = new Color(1, 1, 1, 0);
        else
        {
            SetAchievementImage(c_leftAchievement, c_selectedAchievement - 1);
            c_rightAchievement.color = new Color(1, 1, 1, 1);
        }
            
        SetAchievementImage(c_midAchievement, c_selectedAchievement);
        SetAchievementImage(c_rightAchievement, c_selectedAchievement + 1);
        c_achievementText.text = c_achievements[c_selectedAchievement].Text;
    }

    public void NavigateRight()
    {
        ++c_selectedAchievement;
        if (c_selectedAchievement == c_achievementSprites.Count)
            --c_selectedAchievement;
        SetAchievementImage(c_leftAchievement, c_selectedAchievement - 1);
        SetAchievementImage(c_midAchievement, c_selectedAchievement);
        if (c_selectedAchievement == c_achievementSprites.Count - 1)
            c_rightAchievement.color = new Color(1, 1, 1, 0);
        else
        {
            SetAchievementImage(c_rightAchievement, c_selectedAchievement + 1);
            c_leftAchievement.color = new Color(1, 1, 1, 1);
        }
        c_achievementText.text = c_achievements[c_selectedAchievement].Text;
    }
}
