using UnityEngine;

public class MenuView : MonoBehaviour {

    public enum Direction
    {
        right,
        left
    }

    [SerializeField]
    private MenuView c_leftView;
    [SerializeField]
    private MenuView c_rightView;

    [SerializeField]
    private GameObject c_blackImage;

    public MenuView Swiped(Direction p_direction)
    {
        if (p_direction == Direction.left)
        {
            if (c_leftView == null)
                return this;
            ActiveMenuView(c_leftView);
            return c_leftView;
        }
        else
        {
            if (c_rightView == null)
                return this;
            ActiveMenuView(c_rightView);
            return c_rightView;
        }
    }

    private void ActiveMenuView(MenuView p_menuView)
    {
        c_blackImage.SetActive(false);
        p_menuView.OnActiveMenuView();
        gameObject.SetActive(false);
    }

    public void OnActiveMenuView()
    {
        gameObject.SetActive(true);
        c_blackImage.SetActive(true);
    }
}
