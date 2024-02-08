using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMover : MonoSingleton<CharacterMover>
{
    public event System.Action HorizontalMovementEndedEvent;

    [Header("Configuration")]
    [SerializeField] float speed;

    [Header("Debug")]
    [SerializeField] bool blockMovement;
    private void Start()
    {
        FrontColliderHandler.instance.PrepareToMoveNewBlockEvent += PrepareToMoveNewBlock; 
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
    }

    private void OnNextLevelStarted()
    {
        SetMovementStatus(block: false);
    }

    private void PrepareToMoveNewBlock(Transform _blockTr)
    {
        Vector3 newBlockPos = _blockTr.GetComponent<DivisionBehavior>().GetStandingPos();
        float xValue = newBlockPos.x;
        StartCoroutine(MoveRoutine(xValue));
    }

    private void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockMovement) return;

        transform.Translate(transform.forward * speed * Time.deltaTime);
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }

    private void OnArrivedToTheFinish()
    {
        SetMovementStatus(block: true);
    }
    IEnumerator MoveRoutine(float _xValue)
    {
        transform.DOMoveX(_xValue, .25f);
        yield return new WaitForSeconds(.75f);
        HorizontalMovementEndedEvent?.Invoke();
    }

    public void SetMovementStatus(bool block)
    {
        blockMovement = block;
    }
}
