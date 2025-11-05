using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class YesOrNo
{
    public bool hasQuestion;
    public bool alreadyAnswered = false;
    public int question;

    public bool yesTextActivated = false;
    public UnityEvent OnYesTextEnd;
    public DialogueOptions options;

    public bool hasCondition()
    {
        return options != null;
    }
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
    [SerializeField] UnityEvent onTextEnd;

    [Header("Quest-Only")]
    [SerializeField] bool completeQuest = false;
    [SerializeField] string nextQuestName = "";

    [Header("Read-Only")]
    [SerializeField] string[] activeLines;
    [SerializeField] Sprite[] activeIcons;
    public bool canBeInteracted = true;
    [SerializeField] bool isChoosing = false;

    bool alreadyRecievedQuest;
    NPConditions conditions;
    ChatController chatController;

    PauseMenuController pauseMenu;
    DiaENoite dayAndNight;
    Player player;
    void Start()
    {
        player = Player.instance;
        dayAndNight = DiaENoite.instance;
        pauseMenu = PauseMenuController.instance;

        conditions = NPConditions.instance;
        chatController = ChatController.instance;
        activeLines = dialogueLines;
        activeIcons = charactersFace;

        if (!yesOrNo.hasQuestion) yesOrNo = null;
    }
    public void Falar(string[] falas = null, Sprite[] icons = null)
    {
        if(!canBeInteracted) return;

        if (isChoosing)
        {
        print("to escolhendo");
        return;
        }

        
        
        if(falas != null && icons != null)
        {
            activeLines = falas;
            activeIcons = icons;
        }
        
        if (chatController.falasRoutine != null && chatController.isWritingText)
        {
            chatController.ResetText();
            return;
        }

        falaAtual++;

        if (falaAtual < activeLines.Length && canBeInteracted) //erro aqui
        {

            player.canMove = false;

            if(pauseMenu != null)
            pauseMenu.canPause = false;

            if(dayAndNight != null)
            dayAndNight.isPaused = true;

            chatController.StartDialogue(activeIcons[falaAtual], activeLines[falaAtual]);
            CheckIfHasQuestion();
        }

        if (falaAtual > activeLines.Length)
        {
            ResetNPC();
        }
    }
    public void ResetNPC()
    {
        if(player != null)
        player.canMove = true;
        
        if(pauseMenu != null)
        pauseMenu.canPause = true;

        if(dayAndNight != null)
        dayAndNight.isPaused = false;

        chatController.CloseDialogue();
        print("fechando dialogo");
        activeLines = dialogueLines;
        activeIcons = charactersFace;
        onTextEnd?.Invoke();
        if (yesOrNo != null)
        {
            yesOrNo.alreadyAnswered = false;
            if (yesOrNo.yesTextActivated)
            {
                yesOrNo.OnYesTextEnd?.Invoke();
                yesOrNo.yesTextActivated = false;
            }
        }

        if (isHealer)
        {
            PlayerPartyController.instance.CurarTodos();
        }
        else
        {
            if (completeQuest && !alreadyRecievedQuest)
            {
                QuestController.instance.SetQuestWithAnimation(nextQuestName);
                alreadyRecievedQuest = true;
            }
        }
        falaAtual = -1;
    }
    void CheckIfHasQuestion()
    {
        if (yesOrNo == null) return;
        if(yesOrNo.hasQuestion == false || yesOrNo.alreadyAnswered) return;

        if (falaAtual == yesOrNo.question)
        {
            chatController.SetYesNoFunctions(this);
            chatController.ShowYesOrNoButtons(true);

            Player.instance.canTalk = false;
            canBeInteracted = false;
        }
    }
    public void ChooseQuestion(bool yesOrNoButton)
    {
        falaAtual = -1;
        chatController.ShowYesOrNoButtons(false);
        canBeInteracted = true;
        isChoosing = true;
        yesOrNo.alreadyAnswered = true;
        chatController.ResetText();

        if (yesOrNoButton)
        {
            isChoosing = false;
            yesOrNo.yesTextActivated = true;
            if (yesOrNo.hasCondition())
                conditions.DoAction(this, yesOrNo.options);
        }
        else
        {
            isChoosing = false;
            Falar(yesOrNo.options.noDialogue.lines, yesOrNo.options.noDialogue.faces);
        }
            
        Player.instance.canTalk = true;
    }
}