using UnityEngine;

public class CurarTime : MonoBehaviour
{
    [SerializeField] NPC NPC;
    void Start()
    {
        NPC = GetComponent<NPC>();
    }

    void Heal()
    {
        PlayerPartyController.instance.CurarTodos();
    }
}
