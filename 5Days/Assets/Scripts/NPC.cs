using UnityEngine;
using UnityEngine.Events;
using System.Collections;
[System.Serializable]
public class YesOrNo
{
    public UnityEvent OnYesTextEnd; 
    public bool hasQuestion;
    public int question;
    public bool alreadyAnswered = false;

    [Header("Configurações Sim")]
    public string[] yesText;
    public Sprite[] yesIcons;
    public bool yesTextActivated = false;

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
        activeLines = dialogueLines;
        activeIcons = charactersFace;

        if (!yesOrNo.hasQuestion) yesOrNo = null;
    }
    public void Falar(string[] falas = null, Sprite[] icons = null)
    {
        PauseMenuController pauseMenu = PauseMenuController.instance;
        if(falas != null && icons != null)
        {
            activeLines = falas;
            activeIcons = icons;
        }
        
        if (chatController.falasRoutine == null)
            falaAtual++;

        if (falaAtual < activeLines.Length && canTalk) //erro aqui
        {
            print("proximo dialogo");
            Player.instance.canMove = false;
            pauseMenu.canPause = false;
            DiaENoite.instance.isPaused = true;
            chatController.StartDialogue(activeIcons[falaAtual], activeLines[falaAtual]);
            CheckIfHasQuestion();
        }

        if (falaAtual > activeLines.Length)
        {
            Player.instance.canMove = true;
            pauseMenu.canPause = true;
            DiaENoite.instance.isPaused = false;
            chatController.CloseDialogue();
            yesOrNo.alreadyAnswered = false;
            activeLines = dialogueLines; 
            activeIcons = charactersFace;
            if (yesOrNo.yesTextActivated)
            {
                yesOrNo.OnYesTextEnd?.Invoke();
                yesOrNo.yesTextActivated = false;
            }

            if (isHealer)
            {
                PlayerPartyController.instance.CurarTodos();
            }
            else
            {  
                if (completeQuest && !alreadyTalked)
                {
                    QuestController.instance.SetQuestWithAnimation(nextQuestName);
                    alreadyTalked = true;
                }
            }
            falaAtual = -1;
        }
    }
    void CheckIfHasQuestion()
    {
        if(yesOrNo == null || yesOrNo.alreadyAnswered) return;

        if (falaAtual == yesOrNo.question)
        {
            chatController.SetYesNoFunctions(this);
            chatController.ShowYesOrNoButtons(true);
            canTalk = false;
        }
    }
    public void ChooseQuestion(bool yesOrNoButton)
    {
        falaAtual = -1;
        chatController.ShowYesOrNoButtons(false);
        canTalk = true;
        yesOrNo.alreadyAnswered = true;

        if (yesOrNoButton)
        {
            yesOrNo.yesTextActivated = true;
            Falar(yesOrNo.yesText, yesOrNo.yesIcons);
        }
        else
            Falar(yesOrNo.noText, yesOrNo.noIcons);
    }
}