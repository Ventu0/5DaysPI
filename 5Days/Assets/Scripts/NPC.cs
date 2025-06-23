using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class NPC : MonoBehaviour
{
    [Header("Configurações de Fala")]
    [SerializeField] string[] falas;
    [SerializeField] int falaAtual = -1;
    [SerializeField] Sprite[] charactersFace;

    [Header("Opcionais")]
    [SerializeField] bool isHealer = false;

    [Header("Quest-Only")]
    [SerializeField] bool completeQuest = false;
    [SerializeField] string nextQuestName = "";
    bool jaFalou;
    int falasMaximas;
    ChatController chatController;
    
    void Start()
    {
        chatController = ChatController.instance;
        falasMaximas = falas.Length;
    }
    void Update()
    {

    }
    public void Falar()
    {
        if(chatController.falasRoutine == null)
            falaAtual++;
        if (falaAtual < falasMaximas)
        {
            print("falando");

            chatController.StartDialogue(charactersFace[falaAtual], falas[falaAtual]);
        }
        if(falaAtual > falasMaximas)
        {
            if (isHealer)
            {
                PlayerPartyController.instance.CurarTodos();
                chatController.CloseDialogue();
                falaAtual = -1;
            }
            else
            {
                chatController.CloseDialogue();
                if (completeQuest && !jaFalou)
                {
                    QuestController.instance.SetQuestText(nextQuestName);
                    jaFalou = true;
                }
                falaAtual = -1;
            }
        }
    }
}
