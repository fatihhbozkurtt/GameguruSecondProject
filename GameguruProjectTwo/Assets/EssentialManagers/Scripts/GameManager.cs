using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public static readonly string lastPlayedStageKey = "n_lastPlayedStage";
    public static readonly string randomizeStagesKey = "n_randomizeStages";
    public static readonly string cumulativeStagePlayedKey = "n_cumulativeStages";

    [HideInInspector] public bool isLevelActive = false;
    [HideInInspector] public bool isLevelSuccessful = false;

    public event System.Action NextLevelStartedEvent;
    public event System.Action LevelStartedEvent;
    public event System.Action LevelEndedEvent; // fired regardless of fail or success
    public event System.Action LevelSuccessEvent; // fired only on success
    public event System.Action LevelFailedEvent; // fired only on fail
    public event System.Action LevelAboutToChangeEvent; // fired just before next level load

    protected override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey(cumulativeStagePlayedKey)) 
            PlayerPrefs.SetInt(cumulativeStagePlayedKey, 1);
    }
    public void StartGame()
    {
        isLevelActive = true;
        LevelStartedEvent?.Invoke();
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }
    private void OnArrivedToTheFinish()
    {
        isLevelActive = false;
        EndGame(success: true);
    }
    public void EndGame(bool success)
    {
        isLevelActive = false;
        isLevelSuccessful = success;

        LevelEndedEvent?.Invoke();
        if (success)
        {
            LevelSuccessEvent?.Invoke();
        }
        else
        {
            LevelFailedEvent?.Invoke();
        }
    }
    public void NextStage()
    {
        PlayerPrefs.SetInt(cumulativeStagePlayedKey, PlayerPrefs.GetInt(cumulativeStagePlayedKey, 1) + 1);

        isLevelActive = true;
        NextLevelStartedEvent?.Invoke();
        LevelAboutToChangeEvent?.Invoke();
    }
    public void RestartStage()
    {
        LevelAboutToChangeEvent?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public int GetTotalStagePlayed()
    {
        return PlayerPrefs.GetInt(cumulativeStagePlayedKey, 1);
    }
}