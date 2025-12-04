using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;


public class MenuController : MonoBehaviour
{
    [SerializeField] Button loadGameButton;
    [SerializeField] Button deleteSaveButton;
    [SerializeField] Button fiveDaysLogo;

    [Header("Hardcore Mode")]
    [SerializeField] GameObject hardcorePanel;
    [SerializeField] Toggle hardcoreToggle;   
    [SerializeField] Image toggleImage;
    [SerializeField] Sprite[] toggleSprites;

    [Header("Config")]
    [SerializeField] int clickCountToActivate = 5;
    [SerializeField] int clicks = 0;
    bool hardmode;
    [Header("Interação com o menu (animação)")]
    [SerializeField] Sprite clickSprite;
    [SerializeField] RectTransform characterTransform;
    [SerializeField] float jumpQuantity = 10f;
    [SerializeField] float bobbingDuration = 0.5f;
    Image objectImage;
    Sprite originalSprite;
    bool oneTime = false;
    public static MenuController instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DeleteSave delete = DeleteSave.instance;
        objectImage = characterTransform.GetComponent<Image>();
        originalSprite = objectImage.sprite;
        
        loadGameButton.onClick.AddListener(LoadGameButton);
        fiveDaysLogo.onClick.AddListener(ShowDeleteBTN);
        deleteSaveButton.onClick.AddListener(DeleteSaveBTN);

        ToggleHardmode(hardcoreToggle.isOn);
        hardcoreToggle.onValueChanged.AddListener(ToggleHardmode);
        hardcorePanel.SetActive(false);
        deleteSaveButton.gameObject.SetActive(false);

        if (!delete.ChecarSePossuiSave())
            DeactivateContinueButton();
            
    }
    #region MenuButtons
    public void NewGameButton()
    {
        DeleteSave.instance.Deletar(true);
        if (hardmode)
        {
            PlayerPrefs.SetInt("HardcoreMode", 1);
        }
        
        SceneManager.LoadScene("CasaDianas");
    }
    public void LoadGameButton()
    {
        DeleteSave.instance.DeletarTudoDoDontDestroy(false);
        if (!oneTime)
        {
            Loader.instance.Carregar();
            oneTime = true;
        }
    }
    public void DeleteSaveBTN()
    {
        DeleteSave.instance.Deletar();
        DeactivateContinueButton();
    }
    public void ShowDeleteBTN()
    {
        deleteSaveButton.gameObject.SetActive(true);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void ToggleHardmode(bool toggle)
    {
        hardmode = toggle;
        toggleImage.sprite = hardmode ? toggleSprites[1] : toggleSprites[0];
    }
    #endregion
    public void DeactivateContinueButton()
    {
        loadGameButton.enabled = false;
        Color deactivateColor = new Color(255, 255, 255, 110);
        loadGameButton.image.color = deactivateColor;
        loadGameButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = deactivateColor;
    }
    public void InteractObject()
    {
        StartCoroutine(BounceEffect.instance.Bounce(characterTransform, characterTransform.anchoredPosition, bobbingDuration, jumpQuantity, OnEndBounce));
        objectImage.sprite = clickSprite;
        if(clicks >= clickCountToActivate)
        {
            if(!DeleteSave.instance.ChecarSePossuiSave())
                hardcorePanel.SetActive(true);
        }
    }
    void OnEndBounce()
    {
        clicks++;
        objectImage.sprite = originalSprite;
    }
}
