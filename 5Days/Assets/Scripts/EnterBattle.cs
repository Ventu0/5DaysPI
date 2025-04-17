using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBattle : MonoBehaviour, IEnterBattle
{
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void EnterInBattle()
    {
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);

    }
}
