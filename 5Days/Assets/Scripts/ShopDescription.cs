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
        
    }
    void Update()
    {
        
    }

    public override void AbrirMenu(int whichMove)
    {
        descriptionMenu.SetActive(true);

    }

    public override void Close()
    {
        descriptionMenu.SetActive(false);
    }
}
