using UnityEngine;
using TMPro;
public class DescriptionMenu : MonoBehaviour
{
    public GameObject descriptionMenu; //publico para desativar o menu no script
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI PPText;
    InteractButtonsController interactButtonsController;
    public static DescriptionMenu instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        interactButtonsController = GetComponent<InteractButtonsController>();
        descriptionMenu.SetActive(false);
    }
    public void AbrirMenu(int whatMove) //numero de identificação do ataque selecionado de acordo com o botão
    {
        Attack attack = interactButtonsController.ataques[whatMove];
        descriptionMenu.SetActive(true);

        if(attack.description == null || attack.description == "")
            descriptionText.text = "Sem descrição.";
        else
            descriptionText.text = attack.description;

        PPText.text = "Usos: " + attack.currentPP.ToString() + "/" + attack.maxPP.ToString();

        if(attack.GetType() == typeof(GolpesNeutros))
        {
            damageText.text = "";
        }else
        damageText.text = "Dano: " + attack.danoOuCura * TurnModeManager.instance.QuemEstaAtacando().strengthFactor;
        //futuramente: customizar o menu de descrição para ter mais personalidade e coisas
    }
}