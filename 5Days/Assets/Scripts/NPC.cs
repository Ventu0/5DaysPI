using UnityEngine;
using System.Collections;
[System.Serializable]
public class YesOrNo
{
    public bool hasQuestion;
    public int question;

    [Header("Configurações Sim")]
    public string[] yesText;
    public Sprite[] yesIcons;

    [Header("Configurações Nao")]
    public string[] noText;
    public Sprite[] noIcons;
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

    [Header("Read-Only")]
    [SerializeField] string[] activeLines;
    [SerializeField] Sprite[] activeIcons;

    bool alreadyTalked;
    bool canTalk = true;
    ChatController chatController;
    
    void Start()
    {
        chatController = ChatController.instance;

        if (!yesOrNo.hasQuestion) yesOrNo = null;
    }
    public void Falar(string[] falas = null, Sprite[] icons = null)
    {
        if(falas != activeLines && icons != activeIcons)
        {
            activeLines = falas;
            activeIcons = icons;
        }

        PauseMenuController pauseMenu = PauseMenuController.instance;
        if (chatController.falasRoutine == null)
            falaAtual++;

        if (falaAtual < activeLines.Length && canTalk)
        {
            print("proximo dialogo");
            Player.instance.canMove = false;
            pauseMenu.canPause = false;
            chatController.StartDialogue(activeIcons[falaAtual], activeLines[falaAtual]);
            CheckIfHasQuestion();
        }

        if (falaAtual > activeLines.Length)
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
    public void ChooseQuestion(bool yesOrNoButton)
    {
        if (yesOrNoButton)
        {
            falaAtual = -1;
            chatController.ShowYesOrNoButtons(false);
            Falar(yesOrNo.yesText, yesOrNo.yesIcons);
        }
        else
        {
            falaAtual = -1;
            chatController.ShowYesOrNoButtons(false);
            Falar(yesOrNo.noText, yesOrNo.noIcons);
        }
    }
}