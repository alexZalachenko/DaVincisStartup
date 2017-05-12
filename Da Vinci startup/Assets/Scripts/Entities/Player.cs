using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Player : MonoBehaviour {

    #region VARIABLES
    private Transform c_playerCamera;
    [SerializeField]
    //used when starting conversation to move the camera closer to the player
    private float c_timeToMoveCamera = 1;//in seconds
    private float c_inverseTimeToMoveCamera;
    private Vector3 c_initialPosition;
    [SerializeField]
    private float c_cameraMovementZ = -4;
    [SerializeField]
    private float c_cameraMovementY = 1;
    [SerializeField]
    private float c_cameraMovementX = 1; //it depends on the direction the player is looking

    [SerializeField]
    private CharacterSelector.Character c_character;
    public CharacterSelector.Character Character
    {
        private set
        {
            c_character = value;
        }
        get
        {
            return c_character;
        }
    }
    #endregion

    void Start () {
        c_playerCamera = transform.FindChild("Camera");
        Assert.IsNotNull(c_playerCamera);
        c_inverseTimeToMoveCamera = 1f / (c_timeToMoveCamera * 0.1f);

        ConversationManager t_conversationMgr = GameObject.Find("ConversationMenu").GetComponent<ConversationManager>();
        t_conversationMgr.OnNewConversationEvent += OnConversationStart;
        t_conversationMgr.OnEndConversationEvent += OnConversationEnd;
    }
	
	private void OnConversationStart()
    {
        if (c_playerCamera.gameObject.activeSelf)
        {
            c_initialPosition = c_playerCamera.position;
            Vector3 t_finalPosition = c_playerCamera.position + new Vector3(c_cameraMovementX, c_cameraMovementY, c_cameraMovementZ);
            StartCoroutine(MoveCamera(t_finalPosition));
        }
    }

    private void OnConversationEnd()
    { 
        if (c_playerCamera.gameObject.activeSelf)
            StartCoroutine(MoveCamera(c_initialPosition)); 
    }

    private IEnumerator MoveCamera(Vector3 p_destination)
    {
        
        float t_remainingDistance = (c_playerCamera.transform.position - p_destination).sqrMagnitude;
        while (t_remainingDistance > float.Epsilon)
        {
            Vector3 t_newPosition = Vector3.MoveTowards(c_playerCamera.transform.position, p_destination, c_inverseTimeToMoveCamera * Time.deltaTime);
            c_playerCamera.transform.position = t_newPosition;
            t_remainingDistance = (c_playerCamera.transform.position - p_destination).sqrMagnitude;
            yield return null;
        }
    }
}
