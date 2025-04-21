using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciarLuta : MonoBehaviour
{
    [SerializeField] string cenaEscolhida;
    [SerializeField] bool vitoriaOuDerrota;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (vitoriaOuDerrota)
        {
            SceneManager.LoadScene(cenaEscolhida, LoadSceneMode.Additive);
            TurnModeManager.instance.aliadosPersonagens[2].vida = 0;
        }
        else
        {
            SceneManager.LoadScene(cenaEscolhida);
        }
        
    }
}
