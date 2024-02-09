using UnityEngine;

public class GroundChecker : MonoSingleton<GroundChecker>
{
    [Header("References")]
    [SerializeField] Transform rayOrigin;

    [Header("Configuration")]
    [SerializeField] float rayDistance;

    [Header("Debug")]
    [SerializeField] BlockMover currentBlock;
    [SerializeField] bool blockRay;

    private void Start()
    {
        currentBlock = BlockSpawnManager.instance.GetCurrentInitialBlock();

        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        FrontColliderHandler.instance.PrepareToMoveNewBlockEvent += OnMoveToNewBlock;
        CharacterMover.instance.HorizontalMovementEndedEvent += OnHorizontalMovementEnded;
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockRay) return;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, transform.TransformDirection(Vector3.down), out hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out BlockMover block))
            {
                if (currentBlock != block)
                {
                    currentBlock = block;
                }
            }
        }
        else
        {
            //blockRay = true;
            //Rigidbody rigi = GetComponent<Rigidbody>();
            //rigi.useGravity = blockRay;
        }
    }

    void SetBlockRay(bool block)
    {
        blockRay = block;
    }
    public BlockMover GetCurrentBlock()
    {
        return currentBlock;
    }

    #region Events Subscribers
    private void OnMoveToNewBlock(Transform _)
    {
        SetBlockRay(block: true);
    }

    private void OnHorizontalMovementEnded()
    {
        SetBlockRay(block: false);
    }

    private void OnArrivedToTheFinish()
    {
        SetBlockRay(block: true);
    }

    private void OnNextLevelStarted()
    {
        currentBlock = BlockSpawnManager.instance.GetCurrentInitialBlock();
        SetBlockRay(block: false);
    }
    #endregion

}
