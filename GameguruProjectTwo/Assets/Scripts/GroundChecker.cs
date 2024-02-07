using UnityEngine;
using System;

public class GroundChecker : MonoSingleton<GroundChecker>
{
    public event Action<BlockController> MovedToNewBlockEvent;

    [Header("References")]
    [SerializeField] Transform rayOrigin;

    [Header("Configuration")]
    [SerializeField] float rayDistance;

    [Header("Debug")]
    [SerializeField] BlockController currentBlock;
    [SerializeField] bool blockRay;

    private void Start()
    {
        currentBlock = BlockSpawnManager.instance.initialBlock;
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }
    private void OnArrivedToTheFinish()
    {
        SetBlockRay(status: true);
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockRay) return;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, transform.TransformDirection(Vector3.down), out hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out BlockController block))
            {
                if (currentBlock != block)
                {
                    currentBlock = block;
                    MovedToNewBlockEvent?.Invoke(currentBlock);
                }
            }
        }
        else
        {
            blockRay = true;
            Rigidbody rigi = GetComponent<Rigidbody>();
            rigi.useGravity = blockRay;
        }
    }

    public void SetBlockRay(bool status)
    {
        blockRay = status;
    }
    public BlockController GetCurrentBlock()
    {
        return currentBlock;
    }
}
