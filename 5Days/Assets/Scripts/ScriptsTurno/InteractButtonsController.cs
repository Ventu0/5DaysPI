using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InteractButtonsController : MonoBehaviour
{
    [Header("Essential")]
    public GameObject menu;
    public Animator attackMenuAnim;
    [SerializeField] Button attackButton;
    public Button runButton;
    [SerializeField] TextMeshProUGUI[] attacksText;
    public List<Attack> ataques;

    [Header("Configurable")]
    [SerializeField] Image[] attackIcons = new Image[4];
    [Tooltip("Cores de seleção dos botões de ataque, pode configurar com base na cor do icone do ataque")]
    [SerializeField] Button[] attackButtons = new Button[4];
    [SerializeField] Sprite[] originalSprites = new Sprite[4];
    [SerializeField] float menuDistance;
    [SerializeField] AudioClip errorSound;

    //variaveis não-mostraveis
     TurnModeManager turnModeManager;
    DescriptionMenu descriptionMenuScript;
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
        descriptionMenuScript = GetComponent<DescriptionMenu>();
        turnModeManager = TurnModeManager.instance;
        attackMenuAnim.gameObject.SetActive(false);
        attackButton.onClick.AddListener(OpenMenu);
        EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
    }
    #region MainButtons
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
        MainText mainText = MainText.instance;
        if (turnModeManager.escapeChance == 0)
        {
            mainText.SetText("Não é possível fugir!", Color.red);
            SFX.instance.PlaySFX(errorSound);
            runButton.enabled = false;
        }
        float random = Random.Range(0f, 1f);
        if(random <= turnModeManager.escapeChance)
        {
            ReturnScene.instance.StartCoroutine(ReturnScene.instance.RunAnimation(turnModeManager.aliadosPersonagens.ToArray()));
        }
        else
        {
            mainText.SetText("Não conseguiu fugir!", Color.red);
            runButton.enabled = false;
            runButton.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }
    }
    #endregion
    public void SetupMenu(Vector2 newPos)
    {
        if(turnModeManager.QuemEstaAtacando().characterStatus.ataques != null) 
            ataques = turnModeManager.QuemEstaAtacando().characterStatus.ataques;

        menu.transform.position = new Vector2(newPos.x + menuDistance, newPos.y);
        for (int i = 0; i < attacksText.Length; i++)
        {
            attackIcons[i].sprite = originalSprites[i];

            if (ataques[i] != null)
            {
                attacksText[i].text = ataques[i].nomeAtaque;
                attackButtons[i].interactable = true;
                if (ataques[i].iconeAtaque != null)
                {
                    attackIcons[i].sprite = ataques[i].iconeAtaque;

                    ColorBlock colors = attackButtons[i].colors;

                    colors.highlightedColor = ataques[i].iconMainColor;
                    colors.selectedColor = ataques[i].iconMainColor;

                    attackButtons[i].colors = colors;
                }
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

    public void SetMove(int whichMove)
    {
        Attack ataque = ataques[whichMove];
        MainText mainText = MainText.instance;
        SFX sfx = SFX.instance;
        if (ataque.currentPP <= 0)
        {
            mainText.SetText("Esse ataque não tem mais PP!", Color.red);
            sfx.PlaySFX(errorSound);
            return;
        }
        if(ataque.oneTime)
        {
            mainText.SetText("Esse ataque só pode ser usado uma vez por batalha!", Color.red);
            sfx.PlaySFX(errorSound);
            return;
        }
        if(ataque == null)
        {
            mainText.SetText("Ataque não existe!", Color.red);
            sfx.PlaySFX(errorSound);
            return;
        }

        descriptionMenuScript.descriptionMenu.SetActive(false);
        chosenAttack = whichMove;
        BasePersonagem alvo = turnModeManager.EncontrarAlvo();
        if (ataque.tipoDeAlvo == Alvo.Self)
        {
            alvo = turnModeManager.QuemEstaAtacando();
            Atacar(whichMove, alvo);
            return;
        }  

        if (alvo != null && ataque.tipoDeAlvo == Alvo.Inimigo)
            Atacar(whichMove, alvo); 
        else if(ataque.tipoDeAlvo != Alvo.Self && ataque.canUseSelectMenu)
        {
            SelectTarget selectTarget = SelectTarget.instance;
            List<BasePersonagem> target = ataques[whichMove].tipoDeAlvo == Alvo.Inimigo 
                ? new List<BasePersonagem>(turnModeManager.inimigosPersonagens)
                : new List<BasePersonagem>(turnModeManager.aliadosPersonagens);
            menu.SetActive(false);
            selectTarget.targets = target;

            selectTarget.StartSelecting();
        }
        else if (!ataque.canUseSelectMenu)
        {
            alvo = turnModeManager.QuemEstaAtacando();
            Atacar(whichMove, alvo);
        }
    }
    public void Atacar(int whatMove, BasePersonagem alvo)
    {
        Attack ataque = ataques[whatMove];
        ataque.ExecutarAtaque(alvo, ataque.attackEffect);
    }
}