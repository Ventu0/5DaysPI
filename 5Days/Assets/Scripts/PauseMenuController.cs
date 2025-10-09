using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class CoisasParaSalvar
{
    public int money;

    public Vector2 playerPos;
    public Vector2 playerLastSavedPos;

    public string activeQuest;
    public string activeScene;
}
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button SaveButton;
    public bool canPause = true;
    [SerializeField] bool isPaused;

    public delegate void Save();
    public Save onSave;
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
        if (!canPause) return;
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
        Player player = Player.instance;
        onSave?.Invoke();

        print("money: " + PlayerMoney.money);
        coisasSalvar.money = PlayerMoney.money;
        coisasSalvar.playerPos = player.transform.position;
        coisasSalvar.playerLastSavedPos = player.lastSavedPosition;

        coisasSalvar.activeQuest = QuestController.instance?.GetActiveQuest();
        coisasSalvar.activeScene = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(coisasSalvar, true);
        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", json);
        print("Salvando, Caminho: " + Application.persistentDataPath + "/PlayerData.json");
    }
    public void Sair()
    {
        Salvar();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        Destroy(gameObject); //para não pausar no menu
    }
    #endregion
}
