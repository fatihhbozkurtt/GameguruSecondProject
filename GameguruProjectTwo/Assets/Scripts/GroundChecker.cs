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
        //currentBlock = BlockSpawnManager.instance.GetCurrentInitialBlock();

        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        FrontColliderHandler.instance.PrepareToMoveNewBlockEvent += OnMoveToNewBlock;
        CharacterMover.instance.HorizontalMovementEndedEvent += OnHorizontalMovementEnded;
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheFinish;
        BlockSpawnManager.instance.PreSpawnAdjustmentsAreDoneEvent += OnPreSpawnAdjustmentsDone;
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
            Debug.LogError("There is no block under the character, current block is null");

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
        SetBlockRay(block: false);
    }

    private void OnPreSpawnAdjustmentsDone(BlockMover currentInitialBlock)
    {
        currentBlock = currentInitialBlock;
    }
    #endregion

}
