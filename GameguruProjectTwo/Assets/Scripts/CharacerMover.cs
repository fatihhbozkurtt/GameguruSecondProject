using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacerMover : MonoSingleton<CharacerMover>
{
    [Header("Configuration")]
    [SerializeField] float speed;

    [Header("Debug")]
    [SerializeField] bool blockMovement;

    private void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockMovement) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    public void SetMovementStatus(bool block)
    {
        blockMovement = block;
    }
}
