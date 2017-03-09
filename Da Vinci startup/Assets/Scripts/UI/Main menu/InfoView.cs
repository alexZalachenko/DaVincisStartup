using UnityEngine;

public class InfoView : MonoBehaviour {

    [SerializeField]
    private GameObject c_popUp;
    

	public void OpenBrowserLink(string p_url)
    {
        Application.OpenURL(p_url);
    }

    public void ShowCreditsPopUp()
    {
        RectTransform t_popUp = (Instantiate(c_popUp, transform) as GameObject).GetComponent<RectTransform>();
        t_popUp.offsetMin = new Vector2(0, 0);
        t_popUp.offsetMax = new Vector2(0, 0);
    }
}
