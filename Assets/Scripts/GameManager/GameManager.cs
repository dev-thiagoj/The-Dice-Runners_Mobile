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
    public GameObject mainMenu;
    public GameObject uiContainer;
    public GameObject virtualJoysticks;
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;
    public TextMeshProUGUI maxScoreText = null;

    [Header("Buttons Animation")]
    public GameObject btnContainer;
    public Ease ease;
    public float timeBtnAnim;

    [Header("Level Complete")]
    public GameObject levelCompleteScreen;
    public int finalScore;
    public int turboScore;
    public int maxScore;
    public bool checkedEndLine = false;

    [Header("Final Stars")]
    int activeDices;
    int activeTurbos;
    int maxPossibleScore;
    int totalScore;
    public List<GameObject> fullStars;

    [Header("GameOver Screen")]
    public GameObject gameOverScreen;

    [Header("Pause Game")]
    public GameObject pauseScreen;
    private bool _isGameStarted;
    
    [Header("Restart Game")]
    public int isRestart; //padr�o bin�rio, 0 = n�o e 1 = sim.

    [Header("Tutorial")]
    public GameObject[] tutorialImages;
    public int _viewed = 0;

    [Header("Female Animation")]
    public Animator femaleAnim;

    [Header("UI Level")]
    public TextMeshProUGUI showUILevel;

    [Header("Mini Map")]
    public Transform miniMap;
    #endregion

    private void OnEnable()
    {
        Actions.startTutorial += StartTutorialCoroutine;
        Actions.findFemaleAnim += FindFemaleAnimInScene;
    }

    private void OnDisable()
    {
        Actions.startTutorial -= StartTutorialCoroutine;
        Actions.findFemaleAnim -= FindFemaleAnimInScene;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //cameraCanvas.SetActive(false);
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

        Invoke(nameof(ActiveCollectablesCount), 2);
    }

    private void Update()
    {
        if (_isGameStarted && Input.GetKeyUp(KeyCode.Escape)) PauseGame();
    }

    public void ActiveCollectablesCount()
    {
        activeDices = FindObjectsOfType(typeof(ItemCollectableCoin)).Length;
        activeTurbos = FindObjectsOfType(typeof(ItemCollectableTurbo)).Length;

        maxPossibleScore = activeDices * (activeTurbos + PlayerController.Instance.maxTurbos);
        showUILevel.text = "Level " + LevelManager.Instance.level;
    }

    void TurnAllStarsOff()
    {
        foreach (var gameObject in fullStars)
        {
            gameObject.SetActive(false);
        }
    }

    public void AnimationButtons()
    {
        btnContainer.transform.DOScale(0, timeBtnAnim).SetEase(ease).From();
    }

    void FindFemaleAnimInScene()
    {
        femaleAnim = GameObject.Find("CharacterPos").GetComponentInChildren<Animator>();
        InstantiatePlayerHelper.Instance.InstantiateEndLevelCharacter();
    }

    public void StartRun()
    {
        SFXPool.Instance.CreatePool();
        _isGameStarted = true;
        PlayerController.Instance.InvokeStartRun();
        RollDice.Instance.InvokeStartRoll();
        RollDice.Instance.CallDiceSFX();
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
        RollDice.Instance.canMove = false;
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
        RollDice.Instance.canMove = true;
        pauseScreen.SetActive(false);
        miniMap.gameObject.SetActive(true);                                                                 
        AudioListener.pause = false;
        Cursor.visible = false;
    }

    public void EndGame()
    {
        PlayerController.Instance.canRun = false;
        uiContainer.SetActive(false);
        virtualJoysticks.SetActive(false);
        miniMap.gameObject.SetActive(false);
        Invoke(nameof(ShowGameOverScreen), 2);
    }

    public void LevelComplete()
    {
        PlayerController.Instance.canRun = false;
        uiContainer.SetActive(false);
        virtualJoysticks.SetActive(false);
        miniMap.gameObject.SetActive(false);
        femaleAnim.SetTrigger("FemaleWin");
        PlayerController.Instance.animator.SetTrigger("EndGame");
        PiecesManager.Instance.AddIndex();
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
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        PlayerPrefs.SetInt("isRestart", 0);
        SceneManager.LoadScene(1);
    }

    public void ExitApplication()
    {
        PlayerPrefs.SetInt("viewedTutorial", 0);
        PlayerPrefs.SetInt("isRestart", 0);
        Application.Quit();
    }

    public void UpdateUI()
    {
        TurnTurboInPoints();
        totalScore = ItemManager.Instance.dice * turboScore;
        //if (checkedEndLine) totalScore += 300;
        SaveMaxScore();
        StarsCalculate();
        scoreText.text = "Score: " + totalScore.ToString("000");
        diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");
    }

    void TurnTurboInPoints()
    {
        turboScore = ItemManager.Instance.turbo;

        if (turboScore == 0) turboScore = 1;
    }

    void SaveMaxScore()
    {
        if (totalScore > maxScore)
        {
            maxScore = totalScore;

            maxScoreText.text = ("NEW  " + maxScore).ToString();
            maxScoreText.color = Color.green;

            PlayerPrefs.SetInt("maxScore", maxScore);
        }
        else
        {
            maxScoreText.text = maxScore.ToString();
            maxScoreText.color = Color.yellow;
        }
    }

    void StarsCalculate()
    {
        if (totalScore > (maxPossibleScore * 0.2f) && totalScore < (maxPossibleScore * 0.4f))
        {
            fullStars[0].SetActive(true);
        }
        else if (totalScore >= maxPossibleScore * 0.4f && totalScore < maxPossibleScore * 0.7f)
        {
            fullStars[0].SetActive(true);
            fullStars[1].SetActive(true);
        }
        else if (totalScore >= maxPossibleScore * 0.7f)
        {
            foreach (var star in fullStars)
            {
                star.SetActive(true);
            }
        }
    }

    public void StartTutorialCoroutine()
    {
        if (_viewed == 0) StartCoroutine(TutorialCoroutine());
        else return;
    }

    public IEnumerator TutorialCoroutine()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(true);
            yield return new WaitForSeconds(3);
            tutorialImages[i].SetActive(false);
            yield return new WaitForSeconds(1);
        }

        _viewed = 1;
        PlayerPrefs.SetInt("viewedTutorial", 1);
    }
}
