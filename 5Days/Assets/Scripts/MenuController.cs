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
    [SerializeField]
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
    void Start()
    {
        DeleteSave delete = DeleteSave.instance;
        objectImage = characterTransform.GetComponent<Image>();
        originalSprite = objectImage.sprite;
        hardcorePanel.SetActive(false);
        loadGameButton.onClick.AddListener(LoadGameButton);
        loadGameButton.enabled = delete.ChecarSePossuiSave();

        deleteSaveButton.onClick.AddListener(DeleteSaveBTN);
        deleteSaveButton.gameObject.SetActive(false);
        fiveDaysLogo.onClick.AddListener(ShowDeleteBTN);
        if (!delete.ChecarSePossuiSave())
        {
            return;
        }
        else
        {
            Color deactivateColor = new Color(255, 255, 255, 110);
            loadGameButton.image.color = deactivateColor;
            loadGameButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = deactivateColor;
        }
    }
    #region MenuButtons
    public void NewGameButton()
    {
        DeleteSave.instance.Deletar();
        SceneManager.LoadScene("CasaDianas");
    }
    public void LoadGameButton()
    {
        Loader.instance.DeletarTudoDoDontDestroy();
        Loader.instance.Carregar();
    }
    public void DeleteSaveBTN()
    {
        DeleteSave.instance.Deletar();
        loadGameButton.enabled = false;
        Color deactivateColor = new Color(255, 255, 255, 110);
        loadGameButton.image.color = deactivateColor;
        loadGameButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = deactivateColor;
    }
    public void ShowDeleteBTN()
    {
        deleteSaveButton.gameObject.SetActive(true);
    }
    public void OptionsButton()
    {

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
