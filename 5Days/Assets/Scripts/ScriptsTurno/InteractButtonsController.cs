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
    [SerializeField] Button[] attackButtons;
    public List<Attack> ataques;

    [Header("Configurable")]
    [SerializeField] float menuDistance;
    
    //variaveis não-mostraveis
    public static InteractButtonsController instance;
    private void Awake()
    {
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
        attackButtons = attackMenuAnim.GetComponentsInChildren<Button>();
        attackMenuAnim.gameObject.SetActive(false);
        attackButton.onClick.AddListener(OpenMenu);
    }
    void OnEnable()
    {
        ataques = TurnModeManager.instance.aliados[TurnModeManager.instance.turnoDeQualJogador].ataques;
    }
    void Update()
    {
       
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(!attackMenuAnim.isActiveAndEnabled);
    }
    public void SetupMenu(Vector2 newPos)
    {
        menu.transform.position = new Vector2(newPos.x + menuDistance, newPos.y);

        for (int i = 0; i < attacksText.Length; i++)
        {
            if (ataques[i] != null)
                attacksText[i].text = ataques[i].name;
            else
                attacksText[i].text = "------";
        }
    }

    public void SetMove(int whatMove)
    {
        ataques[whatMove].ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo());
    }
}
