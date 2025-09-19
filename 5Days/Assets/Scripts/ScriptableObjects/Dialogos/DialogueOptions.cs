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
    public DialogueArrays notEnoughMoneyDialogues;

    [Header("Configurações de opções")]
    public bool needMoney;
    public int moneyAmount;
    public bool needSleep;
}
