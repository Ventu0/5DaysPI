using UnityEngine;

public class AtivarCollider : MonoBehaviour
{
    private void Awake()
    {
        int firstQuest = PlayerPrefs.GetInt("FirstQuest");

        if (firstQuest == 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    public void Ativar()
    {
        PlayerPrefs.SetInt("FirstQuest", 1);
        Destroy(gameObject);
    }
}
