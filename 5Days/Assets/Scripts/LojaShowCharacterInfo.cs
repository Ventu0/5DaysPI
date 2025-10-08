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
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] AttackInstance[] attackInstance = new AttackInstance[4];

    [Header("Readonly")]
    public CharacterStatusGeneric characterStatus;

    public static LojaShowCharacterInfo instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach (AttackInstance attackInstance in attackInstance)
        {
            attackInstance.button = attackInstance.attackText.GetComponent<Button>();
        }
        gameObject.SetActive(false);
    }
    public void ApplyInfo(PersonagensNaLoja character)
    {
        if (character == null) return;
        characterStatus = character.personagemOriginal;

        nameText.text = character.nomePersonagem;
        costText.text = character.custo.ToString();
        lifeText.text = characterStatus.vidaMaxima.ToString();

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
                colors.selectedColor = ataque.iconMainColor;

                attackSlot.button.colors = colors;
            }
        }
        
    }
    public void ChangeAlpha(float a)
    {
        CanvasGroup img = GetComponent<CanvasGroup>();
        img.alpha = a;
    }
}
