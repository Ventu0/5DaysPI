using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Doors : MonoBehaviour
{
    public Collider2D florestaCollider;
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
        int firstQuestEnded = PlayerPrefs.GetInt("FirstQuest");

        if(firstQuestEnded == 0)
        {
            florestaCollider.enabled = false;
        }
        else florestaCollider.enabled = true;

        if (cutsceneEnded == 1) return;
        InteractDoors(false);
    }
    void Start()
    {
        
    }
    public void InteractDoors(bool open)
    {
        florestaCollider.enabled = open;
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
