using UnityEngine;
using UnityEngine.UI;

public class LojaScript : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup layoutGroup; //to usando Layout que é mais facil de organizar e mexer tipo slider
    [SerializeField] RectTransform[] items;
    void Start()
    {
        
    }
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Horizontal"))
        {
            if (horizontal > 0)
            {
                layoutGroup.padding.left += Mathf.RoundToInt(horizontal * 157); //157 é o tamanho do item + espaçamento
            }
            else if (horizontal < 0)
            {
                layoutGroup.transform.position += new Vector3(-100, 0, 0);
            }
        }
    }
}
