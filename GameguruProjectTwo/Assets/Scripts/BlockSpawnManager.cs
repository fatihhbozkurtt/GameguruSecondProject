using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
{
 //   public event Action<BlockController> BlockStoppedEvent;

    [Header("Configuration")]
    [SerializeField] int maxStackCount;

    [Header("References")]
    [SerializeField] BlockController blockPrefab;
    [SerializeField] Transform stackPrefab;
    [SerializeField] GameObject finishLine;

    [Header("Debug")]
    [SerializeField] List<BlockController> spawnedBlocks;
    [HideInInspector] public BlockController initialBlock;
    protected override void Awake()
    {
        base.Awake();

        Transform initStack = Instantiate(stackPrefab, transform);
        initialBlock = initStack.GetChild(0).GetComponent<BlockController>();

        spawnedBlocks.Add(initialBlock);
    }

    private void Start()
    {
       // GroundChecker.instance.MovedToNewBlockEvent += OnMovedToNewBlock;
      InputManager.instance.TouchOccuredEvent += OnTouchOccured;
        SpawnBlock();
    }

    private void OnMovedToNewBlock(BlockController obj)
    {
        SpawnBlock();
    }

    private void OnTouchOccured()
    {
        SpawnBlock();
       
    }

    public void SpawnBlock()
    {
        if (HasComeToFinish())
        {
            SpawnFinish();
            return;
        }
        if (!CanSpawnMore())
        {
            Debug.Log("cant spawn");
            return;
        }

        BlockController lastSpawned = GetLastSpawnedBlock();

        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(0, -.5f, lastPos.z + 3);

        BlockController newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, transform.GetChild(0).transform);

        spawnedBlocks.Add(newBlock);
        int index = spawnedBlocks.IndexOf(newBlock);
        newBlock.Initialize(index);
    }

    void SpawnFinish()
    {
        BlockController lastSpawned = GetLastSpawnedBlock();

        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(0, -.5f, lastPos.z + 3);
        Instantiate(finishLine, spawnPos, Quaternion.identity, null);
    }

    public BlockController GetLastSpawnedBlock()
    {
        return spawnedBlocks[spawnedBlocks.Count - 1];
    }

    bool HasComeToFinish()
    {
        int lastSpawnedIndex = spawnedBlocks.Count - 1;

        return lastSpawnedIndex == maxStackCount;
    }

    bool CanSpawnMore()
    {
        int lastSpawnedIndex = spawnedBlocks.Count - 1;
        if (lastSpawnedIndex == 0) return true;


        BlockController block = GroundChecker.instance.GetCurrentBlock();

        int currentBlockIndex = /*block == null ? lastSpawnedIndex :*/ block.GetIndex();
        int diff = lastSpawnedIndex - currentBlockIndex;
        bool status = diff > 2 ? false : true;

        return status;
    }
}
