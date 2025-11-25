using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class YesOrNo
{
    public bool hasQuestion;
    public bool alreadyAnswered = false;
    public bool conditionMet = false;
    public int question;

    public bool activateYesTextEndOnCondition = false;
    [Space(10)]
    public bool yesTextActivated = false;
    public UnityEvent OnYesTextEnd;
    public DialogueOptions options;

    public bool hasCondition()
    {
        return options != null;
    }
}
public class NPC : MonoBehaviour, IInteractable
{
    [Header("Configurações de Fala")]
    public string[] dialogueLines;
    [SerializeField] int falaAtual = -1;
    public Sprite[] charactersFace;

    [Header("Opcionais")]
    [SerializeField] bool talkOnce = true;
    public string idToSave;
    [SerializeField] bool isHealer = false;
    public YesOrNo yesOrNo; //futuro: adicionar mais opções de fala
    [SerializeField] UnityEvent onTextEnd;

    [Header("Quest-Only")]
    [SerializeField] bool completeQuest = false;
    [SerializeField] string nextQuestName = "";

    [Header("Read-Only")]
    [SerializeField] string[] activeLines;
    [SerializeField] Sprite[] activeIcons;
    public bool canBeInteracted = true;
    [SerializeField] bool isChoosing = false;

    public bool alreadyRecievedQuest;
    public bool alreadyTalked;
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
        SaveNPCs.instance.AddNPC(this);
        Load();
    }
    public void Interact()
    {
        Falar();
    }
    public void Falar(string[] falas = null, Sprite[] icons = null)
    {
        if (alreadyTalked) return;
        if (!canBeInteracted) return;

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

        if (falaAtual < activeLines.Length && canBeInteracted) 
        {

            player.canMove = false;

            if(pauseMenu != null)
            pauseMenu.canPause = false;

            if(dayAndNight != null)
            dayAndNight.isPaused = true;

            chatController.StartDialogue(activeIcons[falaAtual], activeLines[falaAtual]);
            CheckIfHasQuestion();
        }

        if (falaAtual > activeLines.Length && !alreadyTalked)
        {
            ResetNPC();
            if(yesOrNo == null && talkOnce)
                alreadyTalked = true;
        }
    }
    public void ChangeOriginalDialogue(DialogueArrays dialogueArray)
    {
        dialogueLines = dialogueArray.lines;
        charactersFace = dialogueArray.faces;
    }
    public void ResetNPC()
    {
        if(player != null)
        player.canMove = true;
        
        if(pauseMenu != null)
        pauseMenu.canPause = true;

        if(dayAndNight != null)
        dayAndNight.isPaused = false;

        onTextEnd?.Invoke();
        chatController.CloseDialogue();
        print("fechando dialogo");
        activeLines = dialogueLines;
        activeIcons = charactersFace;
        if (yesOrNo != null && yesOrNo.hasQuestion)
        {
            yesOrNo.alreadyAnswered = false;
            if (yesOrNo.activateYesTextEndOnCondition)
            {
                if (yesOrNo.conditionMet)
                    yesOrNo.OnYesTextEnd?.Invoke();
            }
            else
                yesOrNo.OnYesTextEnd?.Invoke();
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
    void Load()
    {
        SaveNPCs savedNPCs = SaveNPCs.instance;
        if(savedNPCs == null)
        {
            Debug.LogWarning("Não tem script de salvar npcs");
            return;
        }
        if (!savedNPCs.HasData()) return;

        NPCSaveData data = savedNPCs.GetNPCData(idToSave);
        DialogueOptions options = yesOrNo?.options;
        if (options != null)
        {
            if (options.needMoney && yesOrNo != null) //se adicionar mais condições, adicionar aqui
                yesOrNo.conditionMet = data.alreadyPayedMoney;
            if(yesOrNo.conditionMet)
            {
                ChangeOriginalDialogue(options.alreadyPayedDialogue);
                activeLines = dialogueLines;
                activeIcons = charactersFace;
                print("trocando dialogo");
            }
        }
        
        alreadyRecievedQuest = data.alreadyRecievedQuest;
        if(talkOnce) alreadyTalked = data.alreadyAnswered;

    }
}