using UnityEngine;
using UnityEngine.EventSystems;

public class Door : ClickableEntity, IPointerClickHandler
{
    [SerializeField]
    GameObject c_currentScenario;
    [SerializeField]
    GameObject c_nextScenario;
    [SerializeField]
    Transform c_nextSpawnpoint;
    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!IsClickValid())
            return;
        c_nextScenario.SetActive(true);
        c_currentScenario.SetActive(false);
        c_gameManager.SetActiveCharacterScenario(c_nextScenario, c_nextSpawnpoint.position);
        
    }
}
