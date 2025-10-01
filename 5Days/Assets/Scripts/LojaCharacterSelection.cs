using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaCharacterSelection : MonoBehaviour
{
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] PersonagensNaLoja[] charactersInfo;
    [SerializeField] int selected;

    void Start()
    {
        Setup();
    }
    void Setup()
    {
        for(int i = 0; i < charactersInfo.Length; i++)
        {
            characters[i].Setup(charactersInfo[i]);
        }
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
        if(0 >= selected && sentido == -1 || selected == characters.Length - 1 && sentido == 1)
        {
            print("nao mexe");
            return;
        }

        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);
        sentido = -sentido;
        
        for (int i = 0; i < characters.Length; i++)
        {
            int next = i + sentido;
            print(i);
            if (next < 0)
            {
                if (sentido == 1) sentido = sentido * -1;
                Vector2 offset = new Vector3(100 * -sentido, 0);
                LojaCharacterInstance character = characters[i];
                RectTransform characterRect = character.GetComponent<RectTransform>();
                StartCoroutine(character.Mexer(characterRect.anchoredPosition + offset, character.originalScale, 0.5f));
                continue;
            }else if(next >= characters.Length)
            {
                if (sentido == -1)
                {
                    print("invertendo sentido do max"); //bug acontecendo aqui e ali em cima
                    sentido = sentido * 1;
                }
                Vector2 offset = new Vector3(100 * sentido, 0);
                LojaCharacterInstance character = characters[i];
                RectTransform characterRect = character.GetComponent<RectTransform>();
                StartCoroutine(character.Mexer(characterRect.anchoredPosition + offset, character.originalScale, 0.5f));
                break;
            }

            RectTransform rectTransform = characters[i + sentido].GetComponent<RectTransform>();
            StartCoroutine(characters[i].Mexer(rectTransform.anchoredPosition, rectTransform.localScale, 0.5f));
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
