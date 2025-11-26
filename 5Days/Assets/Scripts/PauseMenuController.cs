using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class PlayerData
{
    public int money;

    public Vector2 playerPos;
    public Vector2 playerLastSavedPos;

    public Vector2 playerLastBedPos;
    public string playerLastBedScene;
    public bool resetPlayerPosOnSleep;

    public string activeQuest;
    public string activeScene;
}
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button SaveButton;
    public bool canPause = true;
    [SerializeField] bool isPaused;

    [Header("Salvar")]
    [SerializeField] string caminho;
    public delegate void Save();
    public Save onSave;


    public PlayerData coisasSalvar;
    public static PauseMenuController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        coisasSalvar = new PlayerData();
        caminho = Application.persistentDataPath;
    }
    private void Start()
    {
        SaveButton.onClick.AddListener(Salvar);
        menu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire2")) //alt
        {
            Salvar();
        }
        if (!canPause) return;
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pausar(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
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

        coisasSalvar.money = PlayerMoney.money;
        coisasSalvar.playerPos = player.transform.position;
        coisasSalvar.playerLastSavedPos = player.lastSavedPosition;

        coisasSalvar.playerLastBedPos = player.lastSavedBedPos;
        coisasSalvar.playerLastBedScene = player.lastSavedBedScene;

        if (Sleep.instance != null)
            coisasSalvar.resetPlayerPosOnSleep = Sleep.instance.resetPlayerPosOnSleep;

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
        DeleteSave.instance.DeletarTudoDoDontDestroy(false);
        Time.timeScale = 1;
        Destroy(gameObject); //para não pausar no menu
    }
    #endregion
}
