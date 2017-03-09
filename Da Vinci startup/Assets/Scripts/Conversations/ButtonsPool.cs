using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ButtonsPool : MonoBehaviour
{

    #region VARIABLES
    private List<Button> c_buttons;
    private GameObject c_canvas;
    [SerializeField]
    private GameObject c_buttonPrefab;
    //parameters about buttons setup
    [SerializeField]
    private float c_buttonTopBound = 0.25f;
    [SerializeField]
    private float c_buttonBottomBound = 0.05f;
    [SerializeField]
    private float c_buttonSpacing = 0.05f;
    [SerializeField]
    private float c_buttonMaxHeight = 0.1f;
    #endregion

    void Awake()
    {
        c_buttons = new List<Button>();
        c_canvas = GameObject.Find("Buttons");
        Assert.IsNotNull(c_canvas, "failed to find Buttons holder on buttonsPool");
    }

    public List<Button> GetButtons(int p_amount)
    {
        List<Button> r_returnedButtons = new List<Button>();
        foreach (Button t_button in c_buttons)
        {
            if (r_returnedButtons.Count < p_amount)
            {
                if (!t_button.gameObject.activeSelf)
                    t_button.gameObject.SetActive(true);
                r_returnedButtons.Add(t_button);
            }
            else
            {
                if (t_button.gameObject.activeSelf)
                    t_button.gameObject.SetActive(false);
                else
                    break;
            }
        }

        int t_buttonsToCreate = p_amount - r_returnedButtons.Count;
        if (r_returnedButtons.Count < p_amount)
        {
            for (int t_createdButtons = 0; t_createdButtons < t_buttonsToCreate; t_createdButtons++)
            {
                Button t_createdButton = Instantiate(c_buttonPrefab, c_canvas.transform).GetComponent<Button>();
                c_buttons.Add(t_createdButton);
                r_returnedButtons.Add(t_createdButton);
                t_createdButton.onClick.AddListener(() => GameObject.Find("ConversationMenu").GetComponent<ConversationManager>().OnButtonClick(t_createdButton.gameObject.GetComponentInChildren<Text>().text));
            }
        }

        SetupButtons(r_returnedButtons);

        return r_returnedButtons;
    }

    private void SetupButtons(List<Button> p_buttons)
    {
        if (p_buttons.Count == 0)
            return;
        float t_offset = c_buttonTopBound;


        float t_buttonHeight = (c_buttonTopBound - c_buttonBottomBound - (p_buttons.Count - 1) * c_buttonSpacing) / p_buttons.Count;
        if (t_buttonHeight > c_buttonMaxHeight)
            t_buttonHeight = c_buttonMaxHeight;

        foreach (Button t_button in p_buttons)
        {
            RectTransform t_buttonRectTransform = t_button.GetComponent<RectTransform>();
            t_buttonRectTransform.anchorMax = new Vector2(0.9f, t_offset);
            t_offset -= t_buttonHeight;
            t_buttonRectTransform.anchorMin = new Vector2(0.1f, t_offset);
            t_buttonRectTransform.offsetMin = new Vector2(0, 0);
            t_buttonRectTransform.offsetMax = new Vector2(1, 1);
            t_offset -= c_buttonSpacing;
        }
    }

}
