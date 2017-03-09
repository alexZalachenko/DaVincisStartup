using UnityEngine;
using UnityEngine.EventSystems;

public class MovementManager : MonoBehaviour {

    #region VARIABLES   
    [SerializeField]
    private bool c_mustMove = false;
    [SerializeField]
    private float c_movementThreshold = 0.5f;
    [SerializeField]
    private float c_movementSpeed = 1.0f;
    private float c_movementObjectivePosition;
    [SerializeField]
    private float c_dontGetCloserToInteractableThan = 0.5f;
    private Vector3 c_movementRight;
    private Vector3 c_movementLeft;
    private float c_scenarioLeftBound;
    private float c_scenarioRightBound;

    private float c_cameraToPlayerDistance;
    private bool c_canMove = true;
    [SerializeField]
    LayerMask c_raycastMask;
    private Camera c_camera;
    #endregion

    void Start ()
    {
        c_movementObjectivePosition = transform.position.x;
        c_cameraToPlayerDistance = transform.position.z - Camera.main.transform.position.z;
        ConversationManager t_conversationMgr = GameObject.Find("ConversationMenu").GetComponent<ConversationManager>();
        t_conversationMgr.OnNewConversationEvent += OnConversationStart;
        t_conversationMgr.OnEndConversationEvent += OnConversationEnd;

        c_movementRight = Vector3.right * c_movementSpeed;
        c_movementLeft = Vector3.left * c_movementSpeed;
        c_camera = transform.GetChild(0).GetComponent<Camera>();
        UpdateBounds();
    }

    void Update()
    {
        if (c_canMove)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //raycast to see if an interactable entity has been clicked
                Vector3 t_origin = c_camera.ScreenToWorldPoint(
                    new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, c_cameraToPlayerDistance));
                t_origin.z -= 1;
                RaycastHit t_hitInfo;
                if (Physics.Raycast(t_origin, Vector3.forward, out t_hitInfo, 2, c_raycastMask))
                {
                    Vector3 t_hitPosition = t_hitInfo.transform.position;
                    //if the player is close enought don't do anything
                    if (Mathf.Abs(t_hitPosition.x - transform.position.x) > c_dontGetCloserToInteractableThan)
                    {
                        //the player is at the left of the raycasted entity
                        if (t_hitPosition.x < transform.position.x) 
                            c_movementObjectivePosition = t_hitPosition.x + c_dontGetCloserToInteractableThan;
                        //the player is at the right of the raycasted entity
                        else
                            c_movementObjectivePosition = t_hitPosition.x - c_dontGetCloserToInteractableThan;
                        c_mustMove = true;
                    }
                    return;
                }

                //if touching the UI don't move the player
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Debug.Log("touched the UI");//tocado cualquier elemento en realidad. No entiendo como furula
                    return;
                }

                //check if the touch is further the scene bounds
                if      (c_scenarioLeftBound > t_origin.x)
                {
                    c_movementObjectivePosition = c_scenarioLeftBound;
                    c_mustMove = true;
                    return;
                }
                else if (c_scenarioRightBound < t_origin.x)
                {
                    c_movementObjectivePosition = c_scenarioRightBound;
                    c_mustMove = true;
                    return;
                }

                //otherwise store the touched position and set it as the objectivePosition
                c_movementObjectivePosition = t_origin.x;
                if (Mathf.Abs(c_movementObjectivePosition - transform.position.x) > c_movementThreshold)
                    c_mustMove = true;
            }
            //if movement is allowed and the player has an objectivePosition move him
            if (c_mustMove)
            {
                if (c_movementObjectivePosition > transform.position.x)
                    transform.position += c_movementRight * Time.deltaTime;
                else
                    transform.position += c_movementLeft * Time.deltaTime;
                if (Mathf.Abs(c_movementObjectivePosition - transform.position.x) < c_movementThreshold)
                    c_mustMove = false;
            }
        }
    }

    private void OnConversationStart()
    {
        c_canMove = false;
        c_mustMove = false;
    }

    private void OnConversationEnd()
    {
        //activate movement after a short delay, so the player won't move with the tap on the response
        StartCoroutine(ActivateMovement());
    }

    private System.Collections.IEnumerator ActivateMovement()
    {
        yield return new WaitForEndOfFrame();// WaitForSeconds(0.5f);
        c_canMove = true;
    }

    public void SetMovementAllowed(bool p_movementState)
    {
        if (!p_movementState)
        {
            c_canMove = false;
            c_mustMove = false;
        }
        else
            c_canMove = true;

        //if (!p_movementState)
        //    OnConversationStart();
        //else
        //    OnConversationEnd();
    }

    public void UpdateBounds()
    {
        Transform t_bounds = GameObject.Find("SceneBounds").transform;
        c_scenarioLeftBound = t_bounds.GetChild(0).transform.position.x;
        c_scenarioRightBound = t_bounds.GetChild(1).transform.position.x;
    }
}
