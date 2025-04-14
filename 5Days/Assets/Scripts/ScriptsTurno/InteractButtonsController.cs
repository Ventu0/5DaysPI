using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;

public class InteractButtonsController : MonoBehaviour
{
    [Header("Essential")]
    [SerializeField] GameObject menu;
    [SerializeField] Animator attackMenuAnim;
    [SerializeField] Button attackButton;
    [SerializeField] TextMeshProUGUI[] attacksText;
    [SerializeField] Button[] attackButtons;
    public List<BasicAttack> ataques;

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
        attacksText = attackMenuAnim.GetComponentsInChildren<TextMeshProUGUI>();
        attackButtons = attackMenuAnim.GetComponentsInChildren<Button>();
        attackMenuAnim.gameObject.SetActive(false);
            //for(int i = 0; i < attackButtons.Length; i++)
            //{
            //    attackButtons[i].onClick.AddListener(() => SetMove(i));
            //}
        attackButton.onClick.AddListener(OpenMenu);
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
            {
                attacksText[i].text = ataques[i].name;
            }
            else
            {
                attacksText[i].text = "------";
            }
        }
    }

    public void SetMove(int whatMove)
    {
        print("chora caetano: " + whatMove);
        ataques[whatMove].ExecutarAtaque(TurnModeManager.instance.EncontrarAlvo());
    }
}
