using UnityEngine;

[CreateAssetMenu(fileName = "DialogueOptions", menuName = "Scriptable Objects/DialogueOptions")]
public class DialogueOptions : ScriptableObject
{

   [TextArea] public string[] yesLines;
   public Sprite[] yesFaces;

   [TextArea] public string[] notEnoughMoneyText;
   public Sprite[] notEnoughMoneyFaces;

    [Header("Falas para caso o player diga não")]
    [TextArea] public string[] noLines;
    public Sprite[] noFaces;

    [Header("Configurações de opções")]
    public bool needMoney;
    public int moneyAmount;
    public bool canSleep;
}
