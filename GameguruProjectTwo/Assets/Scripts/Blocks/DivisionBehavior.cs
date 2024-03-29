using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
public enum Direction
{
    Left,
    Right,
}

public class DivisionBehavior : MonoBehaviour
{
    /// <summary>
    // Developer's Note:
    // I'm aware there are duplicate parts between lines 59 to 119, and this issue may violates SOLID rules slightly.
    // I had rewritten those parts but to provide readability I decided to leave the parts as they are now.
    /// </summary>

    [Header("References")]
    [SerializeField] GameObject fallingPrefab;

    [Header("Configuraiton")]
    [SerializeField] float lowestScaleThreshold;
    [SerializeField] float perfectMatchedTolerance;

    [Header("Debug")]
    BlockMovementController _blockMover;
    const float ConstYScale = 1;
    const float ConstZScale = 3;
    AudioManager.SoundTag soundTag = AudioManager.SoundTag.PerfectMatched;

    [HideInInspector] public GameObject instantiatedStandingBlock;
    private void Awake()
    {
        _blockMover = GetComponent<BlockMovementController>();
        _blockMover.BlockStoppedMovingEvent += OnBlockStopped;
    }

    private void OnBlockStopped()
    {
        Transform previousBlock = BlockSpawnManager.instance.GetBlockFromIndexNo(_blockMover.GetIndex() - 1).transform;
        Vector3 distance = transform.position - previousBlock.transform.position;

        float RemainBlockScaleX;
        float myXScale = transform.localScale.x;
        float previousXScale = previousBlock.transform.transform.localScale.x;
        bool perfectlyMatched = false;
        bool scaleBelowTreshold = false;

        #region Calculate Rightmost & Leftmost Points

        Vector3 previousRightmostPoint = previousBlock.transform.position + new Vector3(previousXScale / 2f, 0, 0);
        Vector3 previousLeftmostPoint = previousBlock.transform.position - new Vector3(previousXScale / 2f, 0, 0);
        Vector3 myRightmostPoint = transform.position + new Vector3(myXScale / 2f, 0, 0);
        Vector3 myLeftmostPoint = transform.position - new Vector3(myXScale / 2f, 0, 0);

        #endregion

        //--------

        if (GetDirection(distance) == Direction.Right)
        {
            RemainBlockScaleX = Mathf.Abs(previousRightmostPoint.x - myLeftmostPoint.x);

            if (myLeftmostPoint.x > previousRightmostPoint.x)
            {
                TriggerFail();
                return;
            }

            if (CheckIfScaleBelowThreshold(RemainBlockScaleX))
            {
                TriggerFail();
                scaleBelowTreshold = true;

            }
            if (IsPerfectlyMatched(distance.x))
            {
                if (!scaleBelowTreshold) // if the block is too smal no need to play audio
                {
                    RemainBlockScaleX = myXScale; // because blocks perfectly matched
                    transform.position = new Vector3(previousBlock.position.x, previousBlock.position.y, transform.position.z);
                    perfectlyMatched = true;
                }
            }

            if (!perfectlyMatched)
            {
                InstantiateFallingPiece(RemainBlockScaleX, previousRightmostPoint, myRightmostPoint);
                SetCalculatedPositionAndScale(RemainBlockScaleX, previousRightmostPoint, myLeftmostPoint);
            }
        }
        else
        {
            RemainBlockScaleX = Mathf.Abs(previousLeftmostPoint.x - myRightmostPoint.x);

            if (myRightmostPoint.x < previousLeftmostPoint.x) // block is stopped too far away from the previous one
            {
                TriggerFail();
                return;
            }

            if (CheckIfScaleBelowThreshold(RemainBlockScaleX))
            {
                TriggerFail();
                scaleBelowTreshold = true;
            }

            if (IsPerfectlyMatched(distance.x))
            {
                if (!scaleBelowTreshold) // if the block is too smal no need to play audio
                {
                    RemainBlockScaleX = myXScale; // because blocks perfectly matched
                    transform.position = new Vector3(previousBlock.position.x, previousBlock.position.y, transform.position.z);
                    perfectlyMatched = true;
                }
            }

            if (!perfectlyMatched)
            {
                InstantiateFallingPiece(RemainBlockScaleX, previousLeftmostPoint, myLeftmostPoint);
                SetCalculatedPositionAndScale(RemainBlockScaleX, previousLeftmostPoint, myRightmostPoint);
            }
        }

        HandleMatchedAudio(perfectlyMatched);


        if (scaleBelowTreshold) return; // standing piece still needs to be positioned adn then fall
                                        // but not triggering spawn should be blocked

        BlockSpawnManager.instance.SetRemainingXScale(RemainBlockScaleX);
        BlockSpawnManager.instance.SpawnBlock();
    }

    void SetCalculatedPositionAndScale(float remainingXScale, Vector3 previousPoint, Vector3 myPoint)
    {
        transform.position = new Vector3((previousPoint.x + myPoint.x) / 2f, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(remainingXScale, ConstYScale, ConstZScale);
    }
    void InstantiateFallingPiece(float remainingXScale, Vector3 previousPoint, Vector3 myPoint)
    {
        Vector3 fallingPos = new Vector3((myPoint.x + previousPoint.x) / 2, transform.position.y, transform.position.z);
        float fallingXScale = transform.localScale.x - remainingXScale;

        GameObject cloneFalling = Instantiate(fallingPrefab, fallingPos, Quaternion.identity);
        cloneFalling.transform.localScale = new Vector3(fallingXScale, ConstYScale, ConstZScale);
    }
    bool CheckIfScaleBelowThreshold(float remainingXScale)
    {
        return remainingXScale <= lowestScaleThreshold;
    }
    void TriggerFail()
    {
        GameManager.instance.EndGame(false);
        transform.AddComponent<Rigidbody>();

        IEnumerator FailRoutine()
        {
            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        }

        StartCoroutine(FailRoutine());
    }
    bool IsPerfectlyMatched(float distanceX)
    {
        if (Mathf.Abs(distanceX) <= perfectMatchedTolerance)
            return true;
        else
            return false;
    }
    void HandleMatchedAudio(bool perfectlyMatched)
    {
        AudioManager manager = AudioManager.instance;

        if (perfectlyMatched)
            manager.PlaySoundEffect(soundTag, manipulatePitch: true);
        else
            manager.OnComboEnded(soundTag);
    }
    Direction GetDirection(Vector3 distance)
    {
        Direction dir;
        if (distance.x > 0)
            dir = Direction.Right;
        else
            dir = Direction.Left;

        return dir;

    }
}