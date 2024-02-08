using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] List<BlockMover> spawnedBlocks;

    public void AddBlock(BlockMover spawnedBlock)
    {
        spawnedBlocks.Add(spawnedBlock);
    }

    public BlockMover GetLastSpawned()
    {
        return spawnedBlocks[spawnedBlocks.Count - 1];
    }

    public List<BlockMover> GetList()
    {
        return spawnedBlocks;
    }

    public BlockMover GetBlockFromIndexNo(int targetIndex)
    {
        return spawnedBlocks[targetIndex];
    }

    public int GetListCount()
    {
        return spawnedBlocks.Count;
    }
}
