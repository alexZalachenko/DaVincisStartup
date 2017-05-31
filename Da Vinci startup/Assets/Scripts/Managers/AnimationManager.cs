using UnityEngine;

public class AnimationManager : MonoBehaviour {

    [SerializeField]
    private Transform c_meshTransform;
    [SerializeField]
    private float c_rotateMesh = 130f;
    public bool c_lookingRight = true;

    public void OnMovement(float p_targetPositionX)
    {
        if (c_meshTransform.position.x < p_targetPositionX)
        {
            if (!c_lookingRight)
            {
                c_meshTransform.eulerAngles += new Vector3(0, -c_rotateMesh, 0);
                c_lookingRight = true;
            }
                
        }
        else
        {
            if (c_lookingRight)
            {
                c_meshTransform.eulerAngles += new Vector3(0, c_rotateMesh, 0);
                c_lookingRight = false;
            }
        }
        
    }
}
