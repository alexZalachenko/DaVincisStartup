using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

    [SerializeField]
    private RectTransform c_rectTransform;
    [SerializeField]
    private Text c_text;

    private void Start()
    {
        c_rectTransform.offsetMin = new Vector2(0, 0);
        c_rectTransform.offsetMax = new Vector2(0, 0);
    }

    public void ClosePopUp()
    {
        Destroy(transform.gameObject);
    }

    public void SetText(string p_text)
    {
        c_text.text = p_text;
    }
}
