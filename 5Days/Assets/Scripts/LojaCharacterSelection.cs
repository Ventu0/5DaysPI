using System.Collections;
using UnityEngine;

public class LojaCharacterSelection : MonoBehaviour
{
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] CharacterStatusGeneric[] charactersInfo;
    [SerializeField] int selected;

    void Start()
    {
        
    }
    void Update()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Horizontal"))
        {
            MexerTodos(horizontal);
        }
    }
    void MexerTodos(int sentido)
    {
        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].Mexer(characters[i + sentido].GetComponent<RectTransform>(), 0.5f);
        }
    }
    bool ChecarSeONumeroEstaNoIntervalo(int i)
    {
        if(i <= selected + 2 && i >= selected - 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
