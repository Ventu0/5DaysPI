using System.Collections;
using UnityEngine;

public class LojaCharacterSelection : MonoBehaviour
{
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] PersonagensNaLoja[] charactersInfo;
    [SerializeField] int selected;

    void Start()
    {
        
    }
    void Update()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Horizontal"))
        {
            if(selected == 0 && horizontal == -1)
            {
                print("nao pode ir mais pra esquerda");
                return;
            }else if(selected == characters.Length - 1 && horizontal == 1)
            {
                print("nao pode ir mais pra direita");
                return;
            }   
                MexerTodos(horizontal);
        }
    }
    void MexerTodos(int sentido)
    {
        selected = Mathf.Clamp(selected + sentido, 1, characters.Length - 2);
        sentido = -sentido;
        for (int i = 0; i < characters.Length; i++)
        {
            print(i);
            int next = i + sentido;
            if (next < 0 || next >= characters.Length)
            {
                Vector3 offset = new Vector3(50 * sentido, 0, 0);
                StartCoroutine(characters[i].Mexer(offset, characters[i].GetComponent<RectTransform>().localScale, 0.5f));
                continue;
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
