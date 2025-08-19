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
        int cutsceneEnded = PlayerPrefs.GetInt("CutsceneEnded");
        if (cutsceneEnded == 1) return;
        InteractDoors(false);
    }
    void Start()
    {
        
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
