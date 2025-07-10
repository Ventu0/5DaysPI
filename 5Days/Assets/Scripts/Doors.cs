using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Doors : MonoBehaviour
{
    [SerializeField] Collider2D[] doors;
    public static Doors instance;
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = this;
        }

        if (CutsceneStatus.cutsceneEnded) return;
        SceneManager.sceneLoaded += OnSceneLoaded;
        print("cena começou");
    }
    void Start()
    {
        
    }
    public void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        InteractDoors(false);
    }
    public void InteractDoors(bool open)
    {
        for(int i = 0; i < doors.Length; i++)
        {
            Collider2D collider2D = doors[i];
            if (collider2D != null)
            {   
                print("Interagindo com a porta: " + doors[i].name + " - Estado: " + open);
                collider2D.enabled = open;
            }
        }       
    }
}
