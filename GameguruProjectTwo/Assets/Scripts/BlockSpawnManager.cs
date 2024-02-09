using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
{
    public event PreSpawnAdjustmentsAreDoneDelegate PreSpawnAdjustmentsAreDoneEvent;
    public delegate void PreSpawnAdjustmentsAreDoneDelegate(BlockMover currentInitialBlock);

    [Header("Configuration")]
    [SerializeField] int maxStackCount;

    [Header("References")]
    [SerializeField] BlockMover blockPrefab;
    [SerializeField] StackController stackPrefab;
    [SerializeField] GameObject finishLine;

    [Header("Debug")]
    [SerializeField] StackController currentStackController;
    [SerializeField] List<StackController> spawnedStacks;
    float maxXScale;
    int _stackIndex;
    bool _isFinishAlreadySpawned;

    protected override void Awake()
    {
        base.Awake();
        PreSpawnAdjustments();
    }
    private void Start()
    {
        //InputManager.instance.TouchOccuredEvent += SpawnBlock;
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        SpawnBlock();
    }
    private void PreSpawnAdjustments()
    {
        maxXScale = 3;

        Vector3 charPos = CharacterMover.instance.transform.position;
        Vector3 spawnPos = new Vector3(charPos.x, 0, charPos.z);

        StackController _stack = Instantiate(stackPrefab, spawnPos, Quaternion.identity, transform);
        _stack.name = "Stack_" + _stackIndex;

        currentStackController = _stack;
        spawnedStacks.Add(currentStackController);
        _stackIndex++;
        _isFinishAlreadySpawned = false;


        PreSpawnAdjustmentsAreDoneEvent?.Invoke(GetCurrentInitialBlock());
    }

    private void OnNextLevelStarted()
    {
        PreSpawnAdjustments();
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        if (!GameManager.instance.isLevelActive) return;

        if (HasComeToFinish())
        {
            if (!_isFinishAlreadySpawned)
            {
                _isFinishAlreadySpawned = true;
                SpawnFinishLine();
                return;
            }
            return;
        }

        BlockMover lastSpawned = GetLastSpawnedBlock();

        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(lastPos.x, -.5f, lastPos.z + 3);

        BlockMover newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, currentStackController.transform);
        newBlock.transform.localScale = new Vector3(maxXScale, 1, 3);

        currentStackController.AddBlock(newBlock);
        int index = currentStackController.GetList().IndexOf(newBlock);
        newBlock.Initialize(index);
    }

    void SpawnFinishLine()
    {
        BlockMover lastSpawned = GetLastSpawnedBlock();
        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(lastPos.x, 0, lastPos.z + 3);

        Instantiate(finishLine, spawnPos, Quaternion.identity, null);
    }
    bool HasComeToFinish()
    {
        int lastSpawnedIndex = currentStackController.GetListCount();
        return lastSpawnedIndex == maxStackCount;
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
    private BlockMover GetCurrentInitialBlock()
    {
        return currentStackController.GetBlockFromIndexNo(0);
    }

    public void SetMaxXScale(float value)
    {
        maxXScale = value;
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
