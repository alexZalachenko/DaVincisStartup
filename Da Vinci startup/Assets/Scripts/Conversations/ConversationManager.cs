using UnityEngine.Assertions;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    #region VARIABLES
    private UIManager c_UIManager;
    private JSONParser c_JSONParser;

    public delegate void OnNewConversation();
    public event OnNewConversation OnNewConversationEvent;

    public delegate void OnEndConversation();
    public event OnEndConversation OnEndConversationEvent;
    #endregion

    void Awake()
    {
        c_UIManager = gameObject.GetComponent<UIManager>();
        c_JSONParser = gameObject.GetComponent<JSONParser>();
        Assert.IsNotNull(c_UIManager, "failed to find UIManager on Reader");
        Assert.IsNotNull(c_JSONParser, "failed to find JSONParser on Reader");
    }

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RegisterNPC(NPC p_NPC)
    {
        p_NPC.OnNPCInteractionEvent += NewConversation;
    }

    private void UpdateUI()
    {
        if (c_JSONParser.ConversationEnded)
        {
            EndConversation();
            return;
        }
        c_UIManager.SetText(c_JSONParser.GetNodeText());
        c_UIManager.SetOptions(c_JSONParser.GetOptionsText());
    }

    public void OnButtonClick(string p_buttonClicked)
    {
        c_JSONParser.OnOptionClicked(p_buttonClicked);
        UpdateUI();
    }

    private void NewConversation(string p_conversationFile)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if (OnNewConversationEvent != null)
            OnNewConversationEvent();
        c_JSONParser.NewConversation(p_conversationFile);
        UpdateUI();
    }

    private void EndConversation()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        OnEndConversationEvent();
        c_JSONParser.EndConversation();
    }
}
