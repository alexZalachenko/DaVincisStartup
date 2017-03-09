using System.Collections.Generic;
using UnityEngine;

public class UINavigationScript : MonoBehaviour {

    [SerializeField]
    private MenuView c_currentView;
    private float c_touchPosition;
    [SerializeField]
    private float c_minMovementToSwipe = 1f;
    
	void Update () {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //to avoid taps
                if (Mathf.Abs(Input.GetTouch(0).position.x - c_touchPosition) > c_minMovementToSwipe)
                {
                    //swipe to the right
                    if (Input.GetTouch(0).position.x - c_touchPosition > 0)
                        c_currentView = c_currentView.Swiped(MenuView.Direction.left);
                    else
                        c_currentView = c_currentView.Swiped(MenuView.Direction.right);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began)
                c_touchPosition = Input.GetTouch(0).position.x;
        }
	}
}
