using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        GameManager.instance.LevelFailedEvent += OnLevelFailed;
    }
    private void OnLevelFailed()
    {
        _animator.enabled = false;
    }

    private void OnNextLevelStarted()
    {
        TriggerAnimation(animationName: "Run");
    }

    private void OnArrivedToTheFinish()
    {
        TriggerAnimation(animationName: "Dance");
    }

    public void TriggerAnimation(string animationName)
    {
        _animator.SetTrigger(animationName);
    }
}
