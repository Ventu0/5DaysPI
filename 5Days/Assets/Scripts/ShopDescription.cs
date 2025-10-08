using UnityEngine;
using TMPro;
public class ShopDescription : Selectable
{
    public GameObject descriptionMenu; 
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI ppText;

    void Start()
    {
        descriptionMenu.SetActive(false);
    }
    void Update()
    {
        
    }

    public override void AbrirMenu(int whichMove)
    {
        descriptionMenu.SetActive(true);
        Attack selectedAttack = LojaShowCharacterInfo.instance.characterStatus.ataques[whichMove];

        if (selectedAttack is GolpesNeutros neutral)
            damageText.text = neutral.buffType + selectedAttack.danoOuCura;
        else
            damageText.text = "Dano: " + selectedAttack.danoOuCura;

        descriptionText.text = selectedAttack.description;
        ppText.text = "Usos: " + selectedAttack.maxPP.ToString();
    }

    public override void Close() => descriptionMenu.SetActive(false);
}
