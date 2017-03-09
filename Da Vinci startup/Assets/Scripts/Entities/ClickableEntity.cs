using UnityEngine;

public class ClickableEntity : MonoBehaviour {

    private float c_maxInteractionDistance = 2;
    protected GameManager c_gameManager = null;

    protected bool IsClickValid()
    {
        if (c_gameManager == null)
            c_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (Mathf.Abs(c_gameManager.GetActiveCharacter().transform.position.x - transform.position.x) > c_maxInteractionDistance)
            return false;
        else
            return true;
    }
}
