using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GroundChecker : MonoSingleton<GroundChecker>
{
    [Header("References")]
    [SerializeField] Transform rayOrigin;
    [SerializeField] Transform startPlatform;

    [Header("Configuration")]
    [SerializeField] float rayDistance;

    [Header("Debug")]
    [SerializeField] ParentBlockClass currentBlock;
    [SerializeField] bool blockRay;

    private void Start()
    {
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        FrontCollisionHandler.instance.PrepareToMoveNewBlockEvent += OnMoveToNewBlock;
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
            if (hit.collider.TryGetComponent(out ParentBlockClass block))
            {
                if (currentBlock != block)
                {
                    currentBlock = block;
                }
            }
        }
        else
        {
            TriggerFail();
        }
    }

    void TriggerFail()
    {
        blockRay = true;
        GameManager.instance.EndGame(success: false);

        IEnumerator FailRoutine()
        {
            Rigidbody rigi = GetComponent<Rigidbody>();
            rigi.useGravity = true;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        StartCoroutine(FailRoutine());
    }

    void SetBlockRay(bool block)
    {
        blockRay = block;
    }
    public ParentBlockClass GetCurrentBlock()
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
        IEnumerator Routine()
        {
            yield return new WaitForSeconds(.75f);
            SetBlockRay(block: false);
        }

        StartCoroutine(Routine());
    }

    private void OnPreSpawnAdjustmentsDone(ParentBlockClass currentInitialBlock)
    {
        currentBlock = currentInitialBlock;
    }
    #endregion

}
