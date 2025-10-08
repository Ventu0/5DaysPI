using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
class AttackInstance
{
    public TextMeshProUGUI attackText;
    public Image attackSprite;
}
public class LojaShowCharacterInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] AttackInstance[] attacks = new AttackInstance[4];

    [Header("Readonly")]
    [SerializeField] CharacterStatusGeneric characterStatus;
    public static LojaShowCharacterInfo instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }
    public void ApplyInfo(PersonagensNaLoja character)
    {
        characterStatus = character.personagemOriginal;

        nameText.text = character.nomePersonagem;
        costText.text = character.custo.ToString();
        lifeText.text = characterStatus.vidaMaxima.ToString();

        for(int i = 0; i < attacks.Length; i++)
        {
            Attack ataque = characterStatus.ataques[i];
            if(ataque == null)
            {
                attacks[i].attackText.text = "------";
            }
            else
            {
                attacks[i].attackText.text = ataque.nomeAtaque;
                if (ataque.iconeAtaque != null)
                    attacks[i].attackSprite.sprite = ataque.iconeAtaque;
            }
        }
        
    }
    public void ChangeAlpha(float a)
    {
        Image img = GetComponent<Image>();
        Color c = img.color;
        c.a = a;
        img.color = c;
    }
}
