using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

public class NPC : ClickableEntity, IPointerClickHandler
{
    #region VARIABLES
    public delegate void OnNPCInteraction(string p_conversationFile);
    public event OnNPCInteraction OnNPCInteractionEvent;

    [SerializeField]
    string c_filePath;
    private string c_conversationFileLeonardo;
    private string c_conversationFileLuca;
    private string c_conversationFileSalai;
    #endregion

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!IsClickValid())
            return;
        if (OnNPCInteractionEvent == null)
        {
            GameObject.Find("ConversationMenu").GetComponent<ConversationManager>().RegisterNPC(this);
            InitialiseFilePaths();
        }
        switch (c_gameManager.c_activeCharacterType)
        {
            case CharacterSelector.Character.Leonardo:
                OnNPCInteractionEvent(c_conversationFileLeonardo);
                break;
            case CharacterSelector.Character.Luca:
                OnNPCInteractionEvent(c_conversationFileLuca);
                break;
            case CharacterSelector.Character.Salai:
                OnNPCInteractionEvent(c_conversationFileSalai);
                break;
        }
    }

    private void InitialiseFilePaths()
    {
        StringBuilder t_conversationFile = new StringBuilder();
        c_conversationFileLeonardo = t_conversationFile.Append(c_filePath).Append("Leonardo").ToString();
        c_conversationFileLuca = t_conversationFile.Replace("Leonardo", "Luca").ToString();
        c_conversationFileSalai = t_conversationFile.Replace("Luca", "Salai").ToString();
    }
}
