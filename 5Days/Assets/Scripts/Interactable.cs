using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent onInteract;
    ChatController chatController;
    void Start()
    {
        chatController = ChatController.instance;
    }
    public void OnReachRange()
    {
        chatController.interactBTN.gameObject.SetActive(true);
        chatController.interactBTN.onClick.AddListener(() => onInteract?.Invoke());
    }
    public void OnExitRange()
    {
        chatController.interactBTN.gameObject.SetActive(false);
        chatController.interactBTN.onClick.RemoveAllListeners();
    }

}
