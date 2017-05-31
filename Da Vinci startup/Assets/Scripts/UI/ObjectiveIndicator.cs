using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class ObjectiveIndicator : MonoBehaviour {

    #region  VARIABLES
    [SerializeField]
    private Text c_objectiveText;
    [SerializeField]
    private GameObject c_scrollView;
    private bool c_newObjective = false;
    [SerializeField]
    private RectTransform c_objectiveIndicator;
    [SerializeField]
    private GameManager c_gameManager;

    public class Objective
    {
        public Objective(string p_id, string p_text)
        {
            c_id = p_id;
            c_text = p_text;
        }
        public string c_id
        {
            private set;
            get;
        }
        public string c_text
        {
            private set;
            get;
        }
    }
    private List<Objective> c_objectives;
    [SerializeField]
    string c_objectivesFile;
    #endregion

    private void Awake()
    {
        LoadObjectives();
    }

    private void Start()
    {
        //set the first objective
        UnlockObjective("objective001");
    }

    public void OnIconClic()
    {
        c_scrollView.SetActive(!c_scrollView.activeSelf);
        c_newObjective = false;
        c_objectiveIndicator.eulerAngles = Vector3.zero;

        if (c_scrollView.activeSelf)
            c_gameManager.SetActiveCharacterMovement(false);
        else
            c_gameManager.SetActiveCharacterMovement(true);
    }

    public string UnlockObjective(string p_objective)
    {
        string t_objectiveText = "Objective not found";
        for (int t_index = 0; t_index < c_objectives.Count; t_index++)
        {
            if (c_objectives[t_index].c_id == p_objective)
                t_objectiveText = c_objectives[t_index].c_text;
        }
        c_newObjective = true;
        c_objectiveText.text = t_objectiveText + "\n";
        StartCoroutine(NotifyNewObjective());
        return t_objectiveText;
    }

    private System.Collections.IEnumerator NotifyNewObjective()
    {
        while (c_newObjective)
        {
            c_objectiveIndicator.eulerAngles += new Vector3(0, 0, 1);
            yield return null;
        }
        yield return null;
    }

    private void LoadObjectives()
    {
        c_objectives = new List<Objective>();
        JSONNode c_parsed = JSONNode.Parse(Resources.Load(c_objectivesFile).ToString())["objectives"];
        for (int t_objective = 0; t_objective < c_parsed.Count; t_objective++)
            c_objectives.Add(new Objective(c_parsed[t_objective]["ID"], c_parsed[t_objective]["text"]));
    }
}
