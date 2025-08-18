using UnityEngine;
using System.Collections;
[System.Serializable]
public class YesOrNo
{
    public bool hasQuestion;
    public int question;

    public string[] yesText;
    public string[] noText;
}
public class NPC : MonoBehaviour
{
    [Header("Configurações de Fala")]
    [SerializeField] string[] dialogueLines;
    [SerializeField] int falaAtual = -1;
    [SerializeField] Sprite[] charactersFace;

    [Header("Opcionais")]
    [SerializeField] bool isHealer = false;
    [SerializeField] YesOrNo yesOrNo; //futuro: adicionar mais opções de fala

    [Header("Quest-Only")]
    [SerializeField] bool completeQuest = false;
    [SerializeField] string nextQuestName = "";
    bool alreadyTalked;
    bool canTalk;
    int falasMaximas;
    ChatController chatController;
    
    void Start()
    {
        chatController = ChatController.instance;
        falasMaximas = dialogueLines.Length;

        if (!yesOrNo.hasQuestion) yesOrNo = null;
    }
    public void Falar(string[] falas = null, Sprite[] icons = null)
    {
        if(falas == null && icons == null)
        {
            falas = dialogueLines;
            icons = charactersFace;
        }

        PauseMenuController pauseMenu = PauseMenuController.instance;
        if (chatController.falasRoutine == null)
            falaAtual++;

        if (falaAtual < falasMaximas && canTalk)
        {
            print("proximo dialogo");
            Player.instance.canMove = false;
            pauseMenu.canPause = false;
            chatController.StartDialogue(icons[falaAtual], falas[falaAtual]);
            CheckIfHasQuestion();
        }

        if (falaAtual > falasMaximas)
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
                if (completeQuest && !alreadyTalked)
                {
                    QuestController.instance.SetQuestWithAnimation(nextQuestName);
                    alreadyTalked = true;
                }
                falaAtual = -1;
            }
        }
    }
    void CheckIfHasQuestion()
    {
        if (yesOrNo != null && falaAtual == yesOrNo.question)
        {
            chatController.ShowYesOrNoButtons(true);
            canTalk = false;
        }
    }
    //void
}
