using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;

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

    //variaveis não-mostraveis
    [SerializeField] Sprite[] originalSprites = new Sprite[4];
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
    void OnEnable()
    {
        
        ataques = TurnModeManager.instance.QuemEstaAtacando().GetComponent<Aliados>().ataques;
    }
    void Update()
    {
       
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(!attackMenuAnim.isActiveAndEnabled);
        ataques = TurnModeManager.instance.QuemEstaAtacando().GetComponent<Aliados>().ataques;
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
    }

    public void SetMove(int whatMove)
    {
        ataques[whatMove].ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo());
    }
}
