using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;


public class MenuController : MonoBehaviour
{
    [SerializeField] Button loadGameButton;

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
        loadGameButton.onClick.AddListener(LoadGameButton);
        loadGameButton.enabled = delete.ChecarSePossuiSave();
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
    
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion
    public void InteractObject()
    {
        StartCoroutine(BounceEffect.instance.Bounce(characterTransform, characterTransform.anchoredPosition, bobbingDuration, jumpQuantity, OnEndBounce));
        objectImage.sprite = clickSprite;
    }
    void OnEndBounce()
    {
        objectImage.sprite = originalSprite;
    }
}
