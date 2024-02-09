using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CharacterInteractionController : MonoSingleton<CharacterInteractionController>
{
    public event Action ArrivedToTheFinishEvent;
    bool blockCollision;

    private void Start()
    {
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
    }

    private void OnNextLevelStarted()
    {
        //IEnumerator ActivationRoutine()
        //{
        //    yield return new WaitForSeconds(.75f);
        //   
        //}

        //StartCoroutine(ActivationRoutine());

        blockCollision = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FinishLine finish))
        {
            if (blockCollision) return;
            blockCollision = true;

            ArrivedToTheFinishEvent?.Invoke();
            transform.DOMove(finish.transform.position, 1);
        }
    }
}
