using System.Collections;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] GameObject[] doors;
    public static Doors instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    public IEnumerator InteractDoors(bool open)
    {
        yield return null;
        print("Quantidade portas: " + doors.Length);
        print("Interagindo com portas: " + open);
        for(int i = 0; i < doors.Length; i++)
        {
            Collider2D collider2D = doors[i].GetComponent<Collider2D>();
            if (collider2D != null)
            {
                print("Interagindo com a porta: " + doors[i].name + " - Estado: " + open);
                collider2D.enabled = open;
            }
        }
            
    }
    void Update()
    {
        
    }
}
