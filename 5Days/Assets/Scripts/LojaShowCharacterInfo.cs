using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
class AttackInstance
{
    public TextMeshProUGUI attackText;
    public Image attackSprite;
    public Button button;
}
public class LojaShowCharacterInfo : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI buyTextMesh;
    [Header("Custom Text")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI costText;

    [Header("AttackTexts")]
    [SerializeField] AttackInstance[] attackInstance = new AttackInstance[4];

    [Header("Readonly")]
    [SerializeField] PersonagensNaLoja currentCharacter;
    public CharacterStatusGeneric characterStatus;

    public static LojaShowCharacterInfo instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        buyTextMesh = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        foreach (AttackInstance attackInstance in attackInstance)
        {
            attackInstance.button = attackInstance.attackText.GetComponent<Button>();
        }
        gameObject.SetActive(false);
    }
    public void ApplyInfo(PersonagensNaLoja character)
    {
        if (character == null) return;
        currentCharacter = character;
        characterStatus = character.personagemOriginal;

        nameText.text = character.nomePersonagem;
        costText.text = character.custo.ToString();
        lifeText.text = characterStatus.vidaMaxima.ToString();

        Image btnImg = buyButton.GetComponent<Image>();
        bool condition = PlayerMoney.money >= character.custo;

        btnImg.color = condition ? Color.green : Color.red;
        buyTextMesh.color = condition ? Color.green : Color.red;

        buyButton.interactable = condition;

        for(int i = 0; i < attackInstance.Length; i++)
        {
            Attack ataque = characterStatus.ataques[i];
            AttackInstance attackSlot = attackInstance[i];
            if (ataque == null)
            {
                attackSlot.attackText.text = "------";
            }
            else
            {
                attackSlot.attackText.text = ataque.nomeAtaque;

                if (ataque.iconeAtaque != null)
                    attackSlot.attackSprite.sprite = ataque.iconeAtaque;

                ColorBlock colors = attackSlot.button.colors;

                colors.highlightedColor = ataque.iconMainColor;

                attackSlot.button.colors = colors;
            }
        }
    }
    public void BuyBTN()
    {
        print("adicionando character");
        PlayerMoney.instance.AddMoneyNoAnimation(-currentCharacter.custo);
        PlayerPartyController.instance.AddCharacter(characterStatus);
    }
    public void ChangeAlpha(float a)
    {
        CanvasGroup img = GetComponent<CanvasGroup>();
        img.alpha = a;
    }
}
