using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class CoisasParaSalvar
{
    public Vector2 playerPos;
    public Vector2 playerLastSavedPos;
    public string activeScene;
}
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button SaveButton;
    [SerializeField] bool isPaused;

    public CoisasParaSalvar coisasSalvar;
    public static PauseMenuController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        coisasSalvar = new CoisasParaSalvar();
    }
    private void Start()
    {
        SaveButton.onClick.AddListener(Salvar);
        menu.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pausar(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Pausar(false);
        }
    }
    #region Buttons
    public void Pausar(bool PauseDespause)
    {
        if (PauseDespause)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
    public void Salvar()
    {
        InimigosController.instance.Salvar();
        coisasSalvar.playerPos = Player.instance.transform.position;
        coisasSalvar.playerLastSavedPos = Player.instance.lastSavedPosition;
        coisasSalvar.activeScene = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(coisasSalvar, true);
        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", json);
        print("Salvando, Caminho: " + Application.persistentDataPath + "/PlayerData.json");
    }
    public void Sair()
    {
        Salvar();
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
