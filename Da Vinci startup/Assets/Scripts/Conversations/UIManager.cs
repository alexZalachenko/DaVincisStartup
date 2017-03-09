using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text c_text;
    [SerializeField]
    private ButtonsPool c_buttonsPool;

    void Awake()
    {
        c_text = GameObject.Find("Text").GetComponent<Text>();
        c_buttonsPool = gameObject.GetComponent<ButtonsPool>();
        Assert.IsNotNull(c_text, "failed to find Text on UIManager");
        Assert.IsNotNull(c_buttonsPool, "failed to find ButtonsPool on UIManager");
    }

    public void SetText(string p_text)
    {
        c_text.text = p_text;
    }

    public void SetOptions(List<string> p_options)
    {
        List<Button> t_createdButtons = c_buttonsPool.GetButtons(p_options.Count);

        for (int t_option = 0; t_option < p_options.Count; t_option++)
        {
            t_createdButtons[t_option].GetComponentInChildren<Text>().text = p_options[t_option];
        }
    }
}
