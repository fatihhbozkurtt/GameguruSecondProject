using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] List<ParentBlockClass> spawnedBlocks;

    public void AddBlock(BlockMovementController spawnedBlock)
    {
        spawnedBlocks.Add(spawnedBlock);
    }

    public ParentBlockClass GetLastSpawned()
    {
        return spawnedBlocks[spawnedBlocks.Count - 1];
    }

    public List<ParentBlockClass> GetList()
    {
        return spawnedBlocks;
    }

    public ParentBlockClass GetBlockFromIndexNo(int targetIndex)
    {
        return spawnedBlocks[targetIndex];
    }

    public int GetListCount()
    {
        return spawnedBlocks.Count;
    }
}
