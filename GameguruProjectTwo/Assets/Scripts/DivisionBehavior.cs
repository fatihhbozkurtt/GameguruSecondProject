using System;
using UnityEngine;
public enum Direction
{
    Left,
    Right,
}

public class DivisionBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform referenceBlockPrefab;
    [SerializeField] GameObject fallingPrefab;
    [SerializeField] GameObject standPrefab;

    [Header("Debug")]
    MeshRenderer referenceMesh;
    BlockMover _blockMover;
    Vector3 standingPos;

    private void Awake()
    {
        _blockMover = GetComponent<BlockMover>();
        _blockMover.BlockStoppedMovingEvent += OnBlockStopped;
        referenceMesh = referenceBlockPrefab.GetComponent<MeshRenderer>();
    }

    private void OnBlockStopped()
    {
        GetComponent<MeshRenderer>().enabled = false;
        BlockMover previousBlock = BlockSpawnManager.instance.GetBlockFromIndexNo(_blockMover.GetIndex() - 1);
        Vector3 distance = previousBlock.transform.position - transform.position;
        DivideBlock(value: distance.x);
    }

    private void DivideBlock(float value)
    {
        bool isFirstFalling = value > 0;

        Transform falling = Instantiate(fallingPrefab).transform;
        Transform stand = Instantiate(standPrefab).transform;

        //Size
        Vector3 fallingSize = referenceBlockPrefab.localScale;
        fallingSize.x = Math.Abs(value);
        falling.localScale = fallingSize;

        Vector3 standSize = referenceBlockPrefab.localScale;
        standSize.x = referenceBlockPrefab.localScale.x - Math.Abs(value);
        stand.localScale = standSize;

        //Position
        Vector3 fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? Direction.Left : Direction.Right);
        int fallingMultiply = (isFirstFalling ? 1 : -1);
        fallingPosition.x += (fallingSize.x / 2) * fallingMultiply;
        falling.position = fallingPosition;

        Vector3 standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? Direction.Left : Direction.Right);
        int standMultiply = (!isFirstFalling ? 1 : -1);
        standPosition.x += (standSize.x / 2) * standMultiply;
        stand.position = standPosition;

        standingPos = standPosition;
    }

    private Vector3 GetPositionEdge(MeshRenderer mesh, Direction direction)
    {
        Vector3 extents = mesh.bounds.extents;
        Vector3 position = mesh.transform.position;

        switch (direction)
        {
            case Direction.Left:
                position.x += -extents.x;
                break;
            case Direction.Right:
                position.x += extents.x;
                break;
        }

        return position;
    }

    public Vector3 GetStandingPos()
    {
        return standingPos;
    }
}