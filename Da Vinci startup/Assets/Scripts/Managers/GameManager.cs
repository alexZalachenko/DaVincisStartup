using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> c_characters;
    [SerializeField]
    private GameObject c_activeCharacter;
    public CharacterSelector.Character c_activeCharacterType
    {
        private set;
        get;
    }

    private void Awake()
    {
        Assert.IsNotNull(c_activeCharacter);
        c_activeCharacterType = CharacterSelector.Character.Leonardo;
        Assert.AreEqual(c_characters.Count, 3, "Found only" + c_characters.Count + "characters in GameManager");
        DeactivateUnselectedCharacters();

        GameObject.Find("CharacterSelector").GetComponent<CharacterSelector>().OnCharacterSelectedEvent += OnCharacterSelected;
    }

    private void OnCharacterSelected(CharacterSelector.Character p_character)
    {
        GameObject t_oldCharacter = c_activeCharacter;
        SetCharacterState(c_activeCharacter, false);
        for (int t_character = 0; t_character < c_characters.Count; t_character++)
        {
            if (c_characters[t_character].GetComponent<Player>().Character == p_character)
                c_activeCharacter = c_characters[t_character];
        }

        if (t_oldCharacter.transform.parent != c_activeCharacter)
            ChangeScenario(t_oldCharacter.transform.parent.gameObject, c_activeCharacter.transform.parent.gameObject);

        SetCharacterState(c_activeCharacter, true);
        SetActiveCharacterMovement(false);
        c_activeCharacterType = c_activeCharacter.GetComponent<Player>().Character;
    }

    private void DeactivateUnselectedCharacters()
    {
        for (int t_character = 0; t_character < c_characters.Count; t_character++)
        {
            if (c_characters[t_character] != c_activeCharacter)
                SetCharacterState(c_characters[t_character], false);
        }
    }

    private void SetCharacterState(GameObject p_character, bool p_state)
    {
        p_character.transform.GetChild(0).gameObject.SetActive(p_state);//activate or deactivate camera
        p_character.GetComponent<Player>().enabled = p_state;
        p_character.GetComponent<MovementManager>().enabled = p_state;
    }

    private void ChangeScenario(GameObject p_oldScenario, GameObject p_newScenario)
    {
        p_oldScenario.SetActive(false);
        p_newScenario.SetActive(true);
    }

    public void SetActiveCharacterMovement(bool p_movementState)
    {
        c_activeCharacter.GetComponent<MovementManager>().SetMovementAllowed(p_movementState);
    }

    public void SetActiveCharacterScenario(GameObject p_newScenario, Vector3 p_newPosition)
    {
        c_activeCharacter.transform.parent = p_newScenario.transform;
        c_activeCharacter.transform.position = p_newPosition;
        c_activeCharacter.GetComponent<MovementManager>().UpdateBounds();
    }

    public GameObject GetActiveCharacter()
    {
        return c_activeCharacter;
    }
}
