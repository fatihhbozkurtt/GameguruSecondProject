using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }

    private void OnArrivedToTheFinish()
    {
        TriggerDancing();
    }

    public void TriggerDancing()
    {
        animator.SetTrigger("Dance");
    }
}
