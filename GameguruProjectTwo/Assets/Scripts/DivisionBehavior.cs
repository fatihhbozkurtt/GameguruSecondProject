using System;
using Unity.VisualScripting;
using UnityEngine;
public enum Direction
{
    Left,
    Right,
}

public class DivisionBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject fallingPrefab;

    [Header("Configuraiton")]
    [SerializeField] float lowestScaleTreshold;
    [SerializeField] float xTolerance;

    [Header("Debug")]
    BlockMover _blockMover;
    const float ConstYScale = 1;
    const float ConstZScale = 3;

    [HideInInspector] public GameObject instantiatedStandingBlock;
    private void Awake()
    {
        _blockMover = GetComponent<BlockMover>();
        _blockMover.BlockStoppedMovingEvent += OnBlockStopped;
    }

    private void OnBlockStopped()
    {
        Transform previousBlock = BlockSpawnManager.instance.GetBlockFromIndexNo(_blockMover.GetIndex() - 1).transform;
        float previousXScale = previousBlock.transform.transform.localScale.x;
        float myXScale = transform.localScale.x;

        float RemainBlockScaleX;
        bool isFailed = false;
        bool isPerfectMatched = false;
        //----------
        Vector3 distance = transform.position - previousBlock.transform.position;

        if (IsPerfectlyMatched(distance.x))
        {
            // Play audio
            isPerfectMatched = true;
        }

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
                Debug.LogError("FAIL");
                isFailed = true;
                transform.AddComponent<Rigidbody>();
                return;
            }

            Debug.Log ("X scale is: " + RemainBlockScaleX);
            if (CheckIfScaleBelowTreshold(RemainBlockScaleX))
            {
                Debug.LogError("Block is too small");
                transform.AddComponent<Rigidbody>();
                return;
            }
            InstantiateFallingPiece(RemainBlockScaleX, previousRightmostPoint, myRightmostPoint);
            SetCalculatedPosition(RemainBlockScaleX, previousRightmostPoint, myLeftmostPoint);


        }
        else
        {
            RemainBlockScaleX = Mathf.Abs(previousLeftmostPoint.x - myRightmostPoint.x);

            if (myRightmostPoint.x < previousLeftmostPoint.x)
            {
                Debug.LogError("FAIL");
                isFailed = true;
                transform.AddComponent<Rigidbody>();
                return;
            }
            Debug.Log("X scale is: " + RemainBlockScaleX);
            if (CheckIfScaleBelowTreshold(RemainBlockScaleX))
            {
                Debug.LogError("Block is too small");
                transform.AddComponent<Rigidbody>();
                return;
            }
            InstantiateFallingPiece(RemainBlockScaleX, previousLeftmostPoint, myLeftmostPoint);
            SetCalculatedPosition(RemainBlockScaleX, previousLeftmostPoint, myRightmostPoint);
        }

        //  if (isPerfectMatched) transform.position = new Vector3(previousBlock.position.x, transform.position.y, transform.position.z);


        BlockSpawnManager.instance.SetMaxXScale(RemainBlockScaleX);
        BlockSpawnManager.instance.SpawnBlock();
    }

    void SetCalculatedPosition(float remainingXScale, Vector3 previousPoint, Vector3 myPoint)
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


    bool CheckIfScaleBelowTreshold(float remainingXScale)
    {
        return remainingXScale <= lowestScaleTreshold;
    }


    bool IsPerfectlyMatched(float distanceX)
    {
        if (Mathf.Abs(distanceX) <= xTolerance)
        {
            Debug.LogWarning("Perfect matched");
            return true;
        }
        else
            return false;
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