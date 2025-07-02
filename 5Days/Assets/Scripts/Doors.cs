using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] Collider2D[] doors;
    public static Doors instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    public void InteractDoors(bool open)
    {
        print("Interagindo com portas: " + open);
        foreach (Collider2D door in doors)
        {
            if (door != null)
            {
                door.enabled = open;
            }
        }
    }
    void Update()
    {
        
    }
}
