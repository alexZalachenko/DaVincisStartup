using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class AchievementsManager : GenericSingleton<AchievementsManager> {

    public List<Achievement> Achievements
    {
        private set;
        get;
    }
    [SerializeField]
    string c_achievementsFile;
    [SerializeField]
    private GameObject c_popupPrefab;

    public override void Awake()
    {
        base.Awake();
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        JSONNode c_parsed = JSONNode.Parse(Resources.Load(c_achievementsFile).ToString())["achievements"];
        for (int t_achievement = 0; t_achievement < c_parsed.Count; t_achievement++)
            Achievements.Add(new Achievement(c_parsed[t_achievement]["text"], c_parsed[t_achievement]["imageSource"], c_parsed[t_achievement]["ID"]));
    }

    public void DeleteAllAchievements()
    {
        for (int t_index = 0; t_index < Achievements.Count; t_index++)
            Achievements[t_index].Unlocked = false;
    }

    public void UnlockAchievement(string p_achievementID)
    {
        for (int t_index = 0; t_index < Achievements.Count; t_index++)
        {
            if (Achievements[t_index].ID == p_achievementID)
            {
                Achievements[t_index].Unlocked = true;
                NotifyAchievementUnlocked(Achievements[t_index].Text);
            }
        }
    }

    private void NotifyAchievementUnlocked(string p_achievement)
    {
        (Instantiate(c_popupPrefab, GameObject.Find("Canvas").transform) as GameObject).GetComponent<PopUp>().SetText(p_achievement);
    }
}
