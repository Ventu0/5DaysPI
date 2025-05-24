using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

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
    [SerializeField] float menuDistance;
    [SerializeField] Sprite[] originalSprites = new Sprite[4];
    //variaveis não-mostraveis
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
        attackMenuAnim.gameObject.SetActive(false);
        attackButton.onClick.AddListener(OpenMenu);
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(!attackMenuAnim.isActiveAndEnabled);
        print(TurnModeManager.instance.QuemEstaAtacando());
        ataques = TurnModeManager.instance.QuemEstaAtacando().GetComponent<Aliados>().ataques;
    }
    public void Defend()
    {
        Aliados aliado = TurnModeManager.instance.QuemEstaAtacando().GetComponent<Aliados>();
        aliado.isDefending = true;
        
        aliado.shield.SetActive(true);
        TurnModeManager.instance.QuemEstaAtacando().turnEnded = true;
        TurnModeManager.instance.CheckIfAllCharactersAttacked();
    }
    public void Run()
    {

    }
    public void SetupMenu(Vector2 newPos)
    {
        menu.transform.position = new Vector2(newPos.x + menuDistance, newPos.y);
        for (int i = 0; i < attacksText.Length; i++)
        {
            attackIcons[i].sprite = originalSprites[i];

            if (ataques[i] != null)
            {
                attacksText[i].text = ataques[i].name;
                attackIcons[i].sprite = ataques[i].iconeAtaque;
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
        SetupMenu(TurnModeManager.instance.QuemEstaAtacando().transform.position);
        EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
    }

    public void SetMove(int whatMove)
    {
        chosenAttack = whatMove;
        BasePersonagem alvo = TurnModeManager.instance.EncontrarAlvo();
        if (alvo != null)
            Atacar(whatMove, alvo); 
        else
        {
            SelectTarget selectTarget = SelectTarget.instance;
            selectTarget.targets = new List<BasePersonagem>(TurnModeManager.instance.inimigosPersonagens);
            InteractButtonsController.instance.menu.SetActive(false);

            selectTarget.StartSelecting();
        }
    }
    public void Atacar(int whatMove, BasePersonagem alvo)
    {
        Attack ataque = ataques[whatMove];
        ataque.ExecutarAtaque(alvo, ataque.attackEffect);
    }
}
