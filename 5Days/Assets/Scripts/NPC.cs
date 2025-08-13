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
    public void Falar()
    {
        PauseMenuController pauseMenu = PauseMenuController.instance;
        if (chatController.falasRoutine == null)
            falaAtual++;

        if (falaAtual < falasMaximas)
        {
            print("proximo dialogo");
            Player.instance.canMove = false;
            pauseMenu.canPause = false;
            chatController.StartDialogue(charactersFace[falaAtual], falas[falaAtual]);
        }
        if(falaAtual > falasMaximas)
        {
            if (isHealer)
            {
                Player.instance.canMove = true;
                pauseMenu.canPause = true;
                PlayerPartyController.instance.CurarTodos();
                chatController.CloseDialogue();
                falaAtual = -1;
            }
            else
            {
                Player.instance.canMove = true;
                pauseMenu.canPause = true;
                chatController.CloseDialogue();
                if (completeQuest && !jaFalou)
                {
                    QuestController.instance.SetQuestWithAnimation(nextQuestName);
                    jaFalou = true;
                }
                falaAtual = -1;
            }
        }
    }
}
