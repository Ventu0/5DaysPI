using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
public class NPC : MonoBehaviour
{
    [Header("Configura��es de Fala")]
    [SerializeField] string[] falas;
    [SerializeField] int falaAtual = -1;
    [SerializeField] Sprite[] charactersFace;
    [SerializeField] bool isHealer = false;
    int falasMaximas;
    
    void Start()
    {
        falasMaximas = falas.Length;
    }
    void Update()
    {

    }
    public void Falar()
    {
            falaAtual++;
        if (falaAtual < falasMaximas)
        {
            print("falando");

            ChatController.instance.StartDialogue(charactersFace[falaAtual], falas[falaAtual]);
        }
        if(falaAtual > falasMaximas)
        {
            if (isHealer)
            {
                PlayerPartyController.instance.CurarTodos();
                ChatController.instance.CloseDialogue();
                falaAtual = -1;
            }
            else
            {
                ChatController.instance.CloseDialogue();
                falaAtual = -1;
            }
        }
    }
}
