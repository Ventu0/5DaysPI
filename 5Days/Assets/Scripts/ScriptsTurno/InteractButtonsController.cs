using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class InteractButtonsController : MonoBehaviour
{
    [Header("Essential")]
    public GameObject menu;
    public Animator attackMenuAnim;
    [SerializeField] Button attackButton;
    [SerializeField] TextMeshProUGUI[] attacksText;
    public List<Attack> ataques;

    [Header("Configurable")]
    [SerializeField] Image[] attackIcons = new Image[4];
    [SerializeField] Color[] attackColors = new Color[4];
    [Tooltip("Cores de seleção dos botões de ataque, pode configurar com base na cor do icone do ataque")]
    [SerializeField] Sprite[] originalSprites = new Sprite[4];
    [SerializeField] float menuDistance;
    //variaveis não-mostraveis
    [SerializeField] TurnModeManager turnModeManager;
    [HideInInspector] public int chosenAttack;
    public static InteractButtonsController instance;
    private void Awake()
    {
        for (int i = 0; i < attacksText.Length; i++)
        {
            originalSprites[i] = attackIcons[i].sprite;
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        turnModeManager = TurnModeManager.instance;
        attackMenuAnim.gameObject.SetActive(false);
        attackButton.onClick.AddListener(OpenMenu);
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(!attackMenuAnim.isActiveAndEnabled);
        print(turnModeManager.QuemEstaAtacando());
        ataques = turnModeManager.QuemEstaAtacando().characterStatus.ataques;
        EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
    }
    public void Defend()
    {
        Aliados aliado = turnModeManager.QuemEstaAtacando().GetComponent<Aliados>();
        aliado.isDefending = true;
        
        aliado.shield.SetActive(true);
        turnModeManager.QuemEstaAtacando().turnEnded = true;
        turnModeManager.CheckIfAllCharactersAttacked();
    }
    public void Run()
    {

    }
    public void SetupMenu(Vector2 newPos)
    {
        if(turnModeManager.QuemEstaAtacando().characterStatus.ataques != null) ataques = turnModeManager.QuemEstaAtacando().characterStatus.ataques;
        menu.transform.position = new Vector2(newPos.x + menuDistance, newPos.y);
        for (int i = 0; i < attacksText.Length; i++)
        {
            attackIcons[i].sprite = originalSprites[i];

            if (ataques[i] != null)
            {
                attacksText[i].text = ataques[i].name;
                if(ataques[i].iconeAtaque != null) attackIcons[i].sprite = ataques[i].iconeAtaque;
            }
            else
            {
                attacksText[i].text = "------";
            }
        }
    }
    public void NextPlayer()
    {
        menu.SetActive(true);
        attackMenuAnim.gameObject.SetActive(false);
        SetupMenu(turnModeManager.QuemEstaAtacando().transform.position);
        EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
    }

    public void SetMove(int whatMove)
    {
        Attack ataque = ataques[whatMove];
        if(ataque == null) return;

        chosenAttack = whatMove;
        BasePersonagem alvo = turnModeManager.EncontrarAlvo();
        if (ataque.tipoDeAlvo == Alvo.Self)
        {
            alvo = turnModeManager.QuemEstaAtacando();
            Atacar(whatMove, alvo);
            return;
        }
            

        if (alvo != null)
            Atacar(whatMove, alvo); 
        else
        {
            SelectTarget selectTarget = SelectTarget.instance;
            List<BasePersonagem> target = ataques[whatMove].tipoDeAlvo == Alvo.Inimigo 
                ? new List<BasePersonagem>(turnModeManager.inimigosPersonagens)
                : new List<BasePersonagem>(turnModeManager.aliadosPersonagens);
            menu.SetActive(false);
            selectTarget.targets = target;

            selectTarget.StartSelecting();
        }
    }
    public void Atacar(int whatMove, BasePersonagem alvo)
    {
        Attack ataque = ataques[whatMove];
        ataque.ExecutarAtaque(alvo, ataque.attackEffect);
    }
}
