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

    [Header("Debug")]
    BlockMover _blockMover;
    Direction _direction;
    float minValue = .1f;
    float xTolerance = 0.5f;

    [HideInInspector] public GameObject instantiatedStandingBlock;
    private void Awake()
    {
        _blockMover = GetComponent<BlockMover>();
        _blockMover.BlockStoppedMovingEvent += OnBlockStopped;
    }

    private void OnBlockStopped()
    {
        Transform previousBlock = BlockSpawnManager.instance.GetBlockFromIndexNo(_blockMover.GetIndex() - 1).transform;
        float prevXScale = previousBlock.transform.transform.localScale.x; // default block scale
        float RemainBlockScaleX = 0;
        bool isFailed = false;
        bool isPerfectMatched = false;
        //----------
        Vector3 distance = transform.position - previousBlock.transform.position;

        if (distance.x > 0)
            _direction = Direction.Right;
        else
            _direction = Direction.Left;


        if (Mathf.Abs(distance.x) <= xTolerance)
        {
            // Play audio
            isPerfectMatched = true;
            Debug.LogWarning("Perfect matched");
        }



        //--------

        if (_direction == Direction.Right)
        {
            Vector3 PrewRightPos = previousBlock.transform.position + new Vector3(prevXScale / 2f, 0, 0);
            Vector3 CurrentLeftPos = transform.position - new Vector3(transform.localScale.x / 2f, 0, 0);
            Vector3 CurrentRightPos = transform.position + new Vector3(transform.localScale.x / 2f, 0, 0);

            RemainBlockScaleX = Mathf.Abs(PrewRightPos.x - CurrentLeftPos.x);

            if (CurrentLeftPos.x > PrewRightPos.x)
            {
                Debug.LogError("FAIL");
                isFailed = true;
            }


            //------ Falling Instantiate
            Vector3 fallingPos = new Vector3((CurrentRightPos.x + PrewRightPos.x) / 2, transform.position.y, transform.position.z);
            float fallingXScale = transform.localScale.x - RemainBlockScaleX;

            GameObject cloneFalling = Instantiate(fallingPrefab, fallingPos, Quaternion.identity);
            cloneFalling.transform.localScale = new Vector3(fallingXScale, 1, 3);
            //------------------


            transform.position = new Vector3((PrewRightPos.x + CurrentLeftPos.x) / 2f, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(RemainBlockScaleX, 1, 3);
            if (RemainBlockScaleX <= minValue)
            {
                transform.AddComponent<Rigidbody>();
                return;
            }
        }
        else
        {
            Vector3 PrewLeftPos = previousBlock.transform.position - new Vector3(prevXScale / 2f, 0, 0);
            Vector3 CurrentRightPos = transform.position + new Vector3(transform.localScale.x / 2f, 0, 0);
            Vector3 CurrentLeftPos = transform.position - new Vector3(transform.localScale.x / 2f, 0, 0);
            RemainBlockScaleX = Mathf.Abs(PrewLeftPos.x - CurrentRightPos.x);

            if (CurrentRightPos.x < PrewLeftPos.x)
            {
                Debug.LogError("FAIL");
                isFailed = true;
            }

            //------ Falling Instantiate
            Vector3 fallingPos = new Vector3((CurrentLeftPos.x + PrewLeftPos.x) / 2, transform.position.y, transform.position.z);
            float fallingXScale = transform.localScale.x - RemainBlockScaleX;

            GameObject cloneFalling = Instantiate(fallingPrefab, fallingPos, Quaternion.identity);
            cloneFalling.transform.localScale = new Vector3(fallingXScale, 1, 3);
            //------------------------

            transform.position = new Vector3((PrewLeftPos.x + CurrentRightPos.x) / 2f, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(RemainBlockScaleX, 1, 3);
            if (RemainBlockScaleX <= minValue)
            {
                transform.AddComponent<Rigidbody>();
                return;
            }

        }

        //  if (isPerfectMatched) transform.position = new Vector3(previousBlock.position.x, transform.position.y, transform.position.z);


        BlockSpawnManager.instance.SetMaxXScale(RemainBlockScaleX);
        BlockSpawnManager.instance.SpawnBlock();
        //if (isFailed) return;
    }
    public Vector3 GetStandingPos()
    {
        return transform.position;
    }
}