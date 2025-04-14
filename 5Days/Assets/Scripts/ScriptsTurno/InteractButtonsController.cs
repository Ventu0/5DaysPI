using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractButtonsController : MonoBehaviour
{
    [Header("Essential")]
    [SerializeField] GameObject menu;
    [SerializeField] Animator attackMenuAnim;
    [SerializeField] Button attackButton;
    [SerializeField] TextMeshProUGUI[] attacksText;
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
        
        attackMenuAnim.gameObject.SetActive(false);
        attackButton.onClick.AddListener(OpenMenu);
    }
    void Update()
    {
        
    }
    public void SetupMenu(Vector2 newPos, BasicAttack[] ataques)
    {
        menu.transform.position = new Vector2(newPos.x + menuDistance, newPos.y);
        for (int i = 0; i < attacksText.Length; i++)
        {
            if(attacksText.Length > ataques.Length)
            {

            }
                attacksText[i].text = ataques[i].name;
            
            attacksText[i].text = ataques[i].name;
        }
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(!attackMenuAnim.isActiveAndEnabled);
    }
}
