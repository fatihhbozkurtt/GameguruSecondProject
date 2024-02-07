using DG.Tweening;
using UnityEngine;

public class CharacterMover : MonoSingleton<CharacterMover>
{
    [Header("Configuration")]
    [SerializeField] float speed;

    [Header("Debug")]
    [SerializeField] bool blockMovement;

    private void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockMovement) return;

        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    public void AssignHorizontalCenter(float xValue)
    {
        GroundChecker.instance.SetBlockRay(status: true);
        transform.DOMoveX(xValue, .5f).OnComplete(() =>
        {
            GroundChecker.instance.SetBlockRay(status: false);

        });
    }

    public void SetMovementStatus(bool block)
    {
        blockMovement = block;
    }
}
