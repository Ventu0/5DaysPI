using UnityEngine;

[System.Serializable]
public class DialogueArrays
{
    [TextArea] public string[] lines;
    public Sprite[] faces;
}

[CreateAssetMenu(fileName = "DialogueOptions", menuName = "Scriptable Objects/DialogueOptions")]
public class DialogueOptions : ScriptableObject
{
    public DialogueArrays yesDialogue;
    public DialogueArrays noDialogue;
    [Space(10)]
    public bool alreadyPayed = false;
    public DialogueArrays notEnoughMoneyDialogues;
    public DialogueArrays alreadyPayedDialogue;
    public DialogueArrays yesPayedDialogue;

    [Header("Configurações de opções")]
    public bool needsSomething = true;
    public bool needToPayAgain;
    public bool needMoney;
    public int moneyAmount;
    public bool needSleep;
}
