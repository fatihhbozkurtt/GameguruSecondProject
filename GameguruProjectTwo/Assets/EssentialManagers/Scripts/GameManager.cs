using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector] public bool isLevelActive = false;
    [HideInInspector] public bool isLevelSuccessful = false;
    int _totalPlayedLevelCount = 1;

    public event System.Action NextLevelStartedEvent;
    public event System.Action LevelStartedEvent;
    public event System.Action LevelEndedEvent; // fired regardless of fail or success
    public event System.Action LevelSuccessEvent; // fired only on success
    public event System.Action LevelFailedEvent; // fired only on fail
    public event System.Action LevelAboutToChangeEvent; // fired just before next level load

    public void Start()
    {
        isLevelActive = true;
        LevelStartedEvent?.Invoke();
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }
    private void OnArrivedToTheFinish()
    {
        isLevelActive = false;
        EndGame(success: true);
        _totalPlayedLevelCount++;
    }
    public void EndGame(bool success)
    {
        isLevelActive = false;
        isLevelSuccessful = success;

        LevelEndedEvent?.Invoke();

        if (success) LevelSuccessEvent?.Invoke();
        else LevelFailedEvent?.Invoke();
    }
    public void NextStage()
    {
        isLevelActive = true;
        NextLevelStartedEvent?.Invoke();
    }
    public void RestartStage()
    {
        LevelAboutToChangeEvent?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _totalPlayedLevelCount = 0;
    }
    public int GetTotalPlayedLevelCOunt()
    {
        return _totalPlayedLevelCount;
    }
}