using UnityEngine;
using UnityEngine.UI;

public class InteractButtonsController : MonoBehaviour
{
    [SerializeField] Button attackButton;
    [SerializeField] Animator attackMenuAnim;
    void Start()
    {
        attackButton.onClick.AddListener(OpenMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenMenu()
    {
        attackMenuAnim.gameObject.SetActive(true);
    }
}
