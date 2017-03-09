using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour {

    #region VARIABLES
    private GameManager c_gameManager;
    private List<Button> c_buttons;
    private Image c_characterButtonImage;
    [SerializeField]
    private List<Sprite> c_characterImages;
    private GameObject c_characterSelector;
    public delegate void OnCharacterSelectedDelegate(Character p_character);
    public event OnCharacterSelectedDelegate OnCharacterSelectedEvent;
    public enum Character
    {
        Leonardo, 
        Luca,
        Salai
    }
    #endregion

    void Start () {
        c_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(c_gameManager);

        c_characterButtonImage = GameObject.Find("CharacterButton").GetComponent<Image>();
        Assert.IsNotNull(c_characterButtonImage);

        c_characterSelector = GameObject.Find("CharactersHolder");
        Assert.IsNotNull(c_characterSelector);

        c_buttons = new List<Button>();
        foreach (Button t_button in c_characterSelector.GetComponentsInChildren<Button>())
            c_buttons.Add(t_button);

        DeactivateOtherButtons("Leonardo");
        c_characterSelector.SetActive(false);
    }
	
	public void OnIconClick()
    {
        c_characterSelector.SetActive(!c_characterSelector.activeSelf);
        if (c_characterSelector.activeSelf)
            c_gameManager.SetActiveCharacterMovement(false);
        else
            c_gameManager.SetActiveCharacterMovement(true);
    }

    public void OnCharacterSelected(string p_character)
    {
        switch (p_character)
        {
            case "Leonardo":
                OnCharacterSelectedEvent(Character.Leonardo);
                c_characterButtonImage.sprite = c_characterImages[0];
                DeactivateOtherButtons("Leonardo");
                break;
            case "Luca":
                OnCharacterSelectedEvent(Character.Luca);
                c_characterButtonImage.sprite = c_characterImages[1];
                DeactivateOtherButtons("Luca");
                break;
            case "Salai":
                OnCharacterSelectedEvent(Character.Salai);
                c_characterButtonImage.sprite = c_characterImages[2];
                DeactivateOtherButtons("Salai");
                break;
        }
    }

    private void DeactivateOtherButtons(string p_buttonName)
    {
        for (int t_index = 0; t_index < c_buttons.Count; t_index++)
        {
            if (p_buttonName != c_buttons[t_index].GetComponent<Image>().sprite.name)
                c_buttons[t_index].interactable = true;
            else
                c_buttons[t_index].interactable = false;
        }
    }
}
