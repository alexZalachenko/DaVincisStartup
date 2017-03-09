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
    private float c_initialCameraPosition = -10;
    private float c_finalCameraPosition = -2;

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
        StartCoroutine(MoveCamera(c_finalCameraPosition));
    }

    private void OnConversationEnd()
    {
        StartCoroutine(MoveCamera(c_initialCameraPosition));
    }

    private IEnumerator MoveCamera(float p_destination)
    {
        float t_remainingDistance = Mathf.Abs(c_playerCamera.transform.position.z - p_destination);
        
        Vector3 t_endPosition = c_playerCamera.transform.position;
        t_endPosition.z = p_destination;

        while (t_remainingDistance > float.Epsilon)
        {
            c_playerCamera.transform.position = Vector3.MoveTowards(c_playerCamera.transform.position, t_endPosition, c_inverseTimeToMoveCamera * Time.deltaTime);
            t_remainingDistance = Mathf.Abs(c_playerCamera.transform.position.z - p_destination);
            yield return null;
        }
    }
}
