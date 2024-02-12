using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] List<ParentBlockClass> spawnedBlocks;
    int index;

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

    public void SetIndex (int _index)
    {
        index = _index; 
    }

    public int GetIndex()
    {
        return index;
    }
}
