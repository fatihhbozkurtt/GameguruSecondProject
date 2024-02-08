using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
{
    [Header("Configuration")]
    [SerializeField] int maxStackCount;

    [Header("References")]
    [SerializeField] BlockMover blockPrefab;
    [SerializeField] StackController stackPrefab;
    [SerializeField] GameObject finishLine;

    [Header("Debug")]
    [SerializeField] StackController currentStackController;
    [SerializeField] List<StackController> spawnedStacks;
    int stackIndex;
    bool canFinishLineSpawn;

    protected override void Awake()
    {
        base.Awake();
        PreSpawnAdjustments();
    }
    private void Start()
    {
        InputManager.instance.TouchOccuredEvent += SpawnBlock;
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        SpawnBlock();
    }
    private void PreSpawnAdjustments()
    {
        Vector3 charPos = CharacterMover.instance.transform.position;
        Vector3 spawnPos = new Vector3(charPos.x, 0, charPos.z);

        StackController _stack = Instantiate(stackPrefab, spawnPos, Quaternion.identity, transform);
        _stack.name = "Stack_" + stackIndex;

        currentStackController = _stack;
        spawnedStacks.Add(currentStackController);
        stackIndex++;
        canFinishLineSpawn = true;
    }

    private void OnNextLevelStarted()
    {
        PreSpawnAdjustments();
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        if (HasComeToFinish() && canFinishLineSpawn)
        {
            SpawnFinishLine();
            return;
        }

        BlockMover lastSpawned = GetLastSpawnedBlock();

        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(lastPos.x, -.5f, lastPos.z + lastSpawned.transform.localScale.x);

        BlockMover newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, transform.GetChild(0).transform);
        currentStackController.AddBlock(newBlock);
        int index = currentStackController.GetList().IndexOf(newBlock);
        newBlock.Initialize(index);
    }

    void SpawnFinishLine()
    {
        if (!canFinishLineSpawn) return;

        canFinishLineSpawn = false;
        BlockMover lastSpawned = GetLastSpawnedBlock();
        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(lastPos.x, 0, lastPos.z + 3);

        Instantiate(finishLine, spawnPos, Quaternion.identity, null);
    }
    bool HasComeToFinish()
    {
        int lastSpawnedIndex = currentStackController.GetListCount();
        canFinishLineSpawn = lastSpawnedIndex == maxStackCount;
        return canFinishLineSpawn;
    }
    #region Getters

    public BlockMover GetLastSpawnedBlock()
    {
        return currentStackController.GetLastSpawned();
    }
    public BlockMover GetBlockFromIndexNo(int targetIndex)
    {
        return currentStackController.GetBlockFromIndexNo(targetIndex);
    }

    public BlockMover GetCurrentInitialBlock()
    {
        return currentStackController.GetBlockFromIndexNo(0);
    }
    #endregion

    //bool CanSpawnMore()
    //{
    //    int lastSpawnedIndex = spawnedBlocks.Count - 1;
    //    if (lastSpawnedIndex == 0) return true;


    //    BlockMover block = GroundChecker.instance.GetCurrentBlock();

    //    int currentBlockIndex = block.GetIndex();
    //    int diff = lastSpawnedIndex - currentBlockIndex;
    //    bool status = diff > 1 ? false : true;

    //    return status;
    //}
}
