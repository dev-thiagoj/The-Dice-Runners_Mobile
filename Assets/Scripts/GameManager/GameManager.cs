using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Singleton;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    #region === VARIABLES ===

    [Header("References")]
    [SerializeField] UIManager uiManager;
    [SerializeField] StarsCalculate starsCalculate;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject uiContainer;
    [SerializeField] GameObject virtualJoysticks;
    [SerializeField] CrossfadeLevelLoader crossfadeLevelLoader;

    [Header("Buttons Animation")]
    [SerializeField] GameObject btnContainer;
    [SerializeField] Ease ease;
    [SerializeField] float timeBtnAnim;

    [Header("Level Complete")]
    [SerializeField] GameObject levelCompleteScreen;

    [Header("GameOver Screen")]
    [SerializeField] GameObject gameOverScreen;

    [Header("Pause Game")]
    [SerializeField] GameObject pauseScreen;
    private bool _isGameStarted;

    [Header("Restart Game")]
    public int isRestart; //padrão binário, 0 = não e 1 = sim.

    [Header("Win Level Animation")]
    public Animator winLevelAnim;

    [Header("UI Level")]
    [SerializeField] TextMeshProUGUI showUILevel;

    [Header("Mini Map")]
    [SerializeField] Transform miniMap;
    #endregion

    private void OnEnable()
    {
        Actions.findEndLevelAnim += FindEndAnimInScene;
        Actions.onFinishLine += LevelComplete;
    }

    private void OnDisable()
    {
        Actions.findEndLevelAnim -= FindEndAnimInScene;
        Actions.onFinishLine -= LevelComplete;
    }

    protected override void Awake()
    {
        base.Awake();

        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (starsCalculate == null) starsCalculate = GameObject.Find("UIManager").GetComponent<StarsCalculate>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        uiContainer.SetActive(false);
        virtualJoysticks.SetActive(false);
        miniMap.gameObject.SetActive(false);

        if (isRestart == 0)
        {
            mainMenu.SetActive(true);
            AnimationButtons();
            _isGameStarted = false;
        }
        else StartRun();
    }

    private void Update()
    {
        if (_isGameStarted && Input.GetKeyUp(KeyCode.Escape)) PauseGame();
    }

    public void AnimationButtons()
    {
        btnContainer.transform.DOScale(0, timeBtnAnim).SetEase(ease).From();
    }

    void FindEndAnimInScene()
    {
        InstantiatePlayerHelper.Instance.InstantiateEndLevelCharacter();
    }

    public void StartRun()
    {
        SFXPool.Instance.CreatePool();
        _isGameStarted = true;
        Actions.onGameStarted.Invoke();
        Cursor.visible = false;
        Invoke(nameof(ShowInGameUI), 6);
    }

    public void ShowInGameUI()
    {
        uiContainer.SetActive(true);
        virtualJoysticks.SetActive(true);
        miniMap.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        miniMap.gameObject.SetActive(false);
        pauseScreen.SetActive(true);
        AudioListener.pause = true;
        Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        miniMap.gameObject.SetActive(true);
        AudioListener.pause = false;
        Cursor.visible = false;
    }

    public void EndGame()
    {
        uiContainer.SetActive(false);
        virtualJoysticks.SetActive(false);
        miniMap.gameObject.SetActive(false);
        Invoke(nameof(ShowGameOverScreen), 2);
    }

    public void LevelComplete()
    {
        uiContainer.SetActive(false);
        virtualJoysticks.SetActive(false);
        miniMap.gameObject.SetActive(false);
        UpdateUI();
        Invoke(nameof(ShowLevelCompleteScreen), 5);
    }

    public void ShowLevelCompleteScreen()
    {
        levelCompleteScreen.SetActive(true);
        Cursor.visible = true;
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("isRestart", 1);
        //SceneManager.LoadScene(1);
        crossfadeLevelLoader.StartCrossfadeAnim(1);
    }

    public void GoToMenu()
    {
        PlayerPrefs.SetInt("isRestart", 0);
        //SceneManager.LoadScene(1);
        crossfadeLevelLoader.StartCrossfadeAnim(1);
    }

    public void ExitApplication()
    {
        PlayerPrefs.SetInt("isRestart", 0);
        Application.Quit();
    }

    public void UpdateUI()
    {
        starsCalculate.UpdateUI();
        uiManager.UpdateUIScores();
    }
}
