using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;
  
/*
 * Inklwritter https://writer.inklestudios.com/ puts markups on the nodes. Here we want to put markups on the choice itself
 * so remember to manually change it. Add a flag to every choice node which you want to be an exit node. The same applies for
 * all the other markups
 *        {
            "ifConditions": false,
            "linkPath": "queTeLargues",
            "option": "Venga, adios",
            "notIfConditions": false,
            "flagName": exit
          }
 * also remember to replace every null with false
 * Anyways, information can be added to the node instead of the choice. Simply add an element to the content object
 */

public class JSONParser : MonoBehaviour
{
    #region VARIABLES
    private class Conversation
    {
        public string c_conversationFile
        {
            get;
            set;
        }
        public JSONNode c_currentNode
        {
            get;
            set;
        }
        public JSONNode c_stitches
        {
            get;
            set;
        }

        public Conversation(string p_conversationFile, JSONNode p_currentNode, JSONNode p_stitches)
        {
            c_conversationFile = p_conversationFile;
            c_currentNode = p_currentNode;
            c_stitches = p_stitches;
        }
    }
    private List<Conversation> c_savedConversations;
    private List<string> c_collectedMarkups;
    private string c_conversationFile = null;
    private JSONNode c_currentStitch;
    private JSONNode c_stitches;
    [SerializeField]
    private ObjectiveIndicator c_objectiveIndicator;
    private AchievementsManager c_achievementsManager;
    #endregion

    public bool ConversationEnded
    {
        set;
        get;
    }

    private void Start()
    {
        c_collectedMarkups = new List<string>();
        c_savedConversations = new List<Conversation>();
        c_achievementsManager = AchievementsManager.Instance;
    }

    private void OpenFile(string p_file)
    {
        c_stitches = JSONNode.Parse(Resources.Load(p_file).ToString());
        
        string t_fistNode = c_stitches["data"]["initial"];
        c_currentStitch = c_stitches["data"]["stitches"][t_fistNode];
        c_stitches = c_stitches["data"]["stitches"];
        c_conversationFile = p_file;
    }

    public string GetNodeText()
    {
        string r_text = c_currentStitch["content"][0];

        while (c_currentStitch["content"][1].Keys.Contains("divert")/* .ElementAt(0) == "divert"*/)
        {
            c_currentStitch = c_stitches[c_currentStitch["content"][1]["divert"]];
            r_text += c_currentStitch["content"][0];
        }
        return r_text;
    }

    public List<string> GetOptionsText()
    {
        List<string> r_options = new List<string>();
        bool t_conditionNotMet;
        for (int t_index = 1; t_index < c_currentStitch["content"].Count; t_index++)
        {
            t_conditionNotMet = false;
            //found an ifCondition
            if (c_currentStitch["content"][t_index]["ifConditions"] != null && c_currentStitch["content"][t_index]["ifConditions"].ToString() != "\"false\"")
            {
                //loop throught all the conditions
                for (int t_index2 = 0; t_index2 < c_currentStitch["content"][t_index]["ifConditions"].Count; t_index2++)
                {
                    if (!c_collectedMarkups.Contains(c_currentStitch["content"][t_index]["ifConditions"][t_index2]["ifCondition"]))
                        t_conditionNotMet = true;
                }
                if (t_conditionNotMet)
                    continue;
            }

            //found an notIfCondition
            if (c_currentStitch["content"][t_index]["notIfConditions"] != null && c_currentStitch["content"][t_index]["notIfConditions"].ToString() != "\"false\"")
            {
                if (c_collectedMarkups.Contains(c_currentStitch["content"][t_index]["notIfConditions"]))
                    continue;
            }

            //found an option
            if (c_currentStitch["content"][t_index]["option"] != null)
                r_options.Add(c_currentStitch["content"][t_index]["option"]);

            //look for markups, objectives and achievements only on nodes with length 1 in order to avoid taking choice ones
            if (c_currentStitch["content"][t_index].Count == 1)
            {
                //if markup objective and achievement are read from here it means that it is been taken from the node itself and not from the choice
                //found an objective on the node
                if (c_currentStitch["content"][t_index]["objective"] != null)
                    c_objectiveIndicator.UnlockObjective(c_currentStitch["content"][t_index]["objective"]);

                //found an achievement on the node
                if (c_currentStitch["content"][t_index]["achievement"] != null)
                    ObjectiveAchieved(c_currentStitch["content"][t_index]["achievement"]);

                //found a markup on the node. Store it if it isn't exit markup
                if (c_currentStitch["content"][t_index]["flagName"] != null)
                {
                    if (c_currentStitch["content"][t_index]["flagName"].ToString() != "\"exit\"")
                        c_collectedMarkups.Add(c_currentStitch["content"][t_index]["flagName"]);
                }
            }
        }
        return r_options;
    }

    public void OnOptionClicked(string p_optionText)
    {
        for (int t_index = 1; t_index < c_currentStitch["content"].Count; t_index++)
        {
            string t_optionText = c_currentStitch["content"][t_index]["option"];
            if (t_optionText != null && t_optionText == p_optionText)
            {
                //is it a empty linkpath?
                if (!c_currentStitch["content"][t_index]["linkPath"].AsBool)
                    ConversationEnded = true;
                else
                    c_currentStitch = c_stitches[c_currentStitch["content"][t_index]["linkPath"]];
                //found an objective on the taken choice
                if (c_currentStitch["content"][t_index]["objective"] != null)
                    ObjectiveAchieved(c_currentStitch["content"][t_index]["objective"]);
                //found an achievement on the taken choice
                if (c_currentStitch["content"][t_index]["achievement"] != null)
                    c_achievementsManager.UnlockAchievement(c_currentStitch["content"][t_index]["achievement"]);
                //search for exit flag on the taken choice
                if (c_currentStitch["content"][t_index]["flagName"] != null &&
                    c_currentStitch["content"][t_index]["flagName"].ToString() == "\"exit\"")
                    ConversationEnded = true;
                break;
            }
        }
    }

    public void NewConversation(string p_conversationFile)
    {
        ConversationEnded = false;
        for (int t_index = 0; t_index < c_savedConversations.Count; t_index++)
        {
            if (c_savedConversations[t_index].c_conversationFile == p_conversationFile)
            {
                c_conversationFile = c_savedConversations[t_index].c_conversationFile;
                c_currentStitch = c_savedConversations[t_index].c_currentNode;
                c_stitches = c_savedConversations[t_index].c_stitches;
                break;
            }
        }
        if (c_conversationFile == null)
            OpenFile(p_conversationFile);
    }

    public void EndConversation()
    {
        for (int t_index = 0; t_index < c_savedConversations.Count; t_index++)
        {
            if (c_savedConversations[t_index].c_conversationFile == c_conversationFile)
            {
                c_savedConversations[t_index].c_conversationFile = c_conversationFile;
                c_savedConversations[t_index].c_currentNode = c_currentStitch;
                c_savedConversations[t_index].c_stitches = c_stitches;
                break;
            }
        }
        c_conversationFile = null;
    }

    private void ObjectiveAchieved(string p_objective)
    {
        string t_achievedObjective = c_objectiveIndicator.UnlockObjective(p_objective);
        //check if there are any stitch with the same name as the unlocked objective. If so, set it as current stitch
        for (int t_index = 0; t_index < c_savedConversations.Count; t_index++)
        {
            for (int t_index2 = 0; t_index2 < c_savedConversations[t_index].c_stitches.Count; t_index2++)
            {
                if (c_savedConversations[t_index].c_stitches[t_index2] == t_achievedObjective)
                {
                    c_savedConversations[t_index].c_currentNode = c_savedConversations[t_index].c_stitches[t_index2];
                    return;
                }
            }
        }
    }
}
