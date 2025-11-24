using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent onInteract;
    public void Interact()
    {
        onInteract?.Invoke();
    }

    void Start()
    {
        
    }
}
