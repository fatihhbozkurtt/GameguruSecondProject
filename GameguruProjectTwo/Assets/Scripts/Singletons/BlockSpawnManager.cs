using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
{
    public event PreSpawnAdjustmentsAreDoneDelegate PreSpawnAdjustmentsAreDoneEvent;
    public delegate void PreSpawnAdjustmentsAreDoneDelegate(ParentBlockClass currentInitialBlock);

    [Header("Configuration")]
    [SerializeField] int maxBlockCount;

    [Header("References")]
    [SerializeField] BlockMovementController blockPrefab;
    [SerializeField] StackController stackPrefab;
    [SerializeField] GameObject finishLine;

    [Header("Debug")]
    [SerializeField] StackController currentStackController;
    [SerializeField] List<StackController> spawnedStacks;
    float _remainingXScale;
    const float _constVerticalPos = -0.5f;
    int _stackIndex;
    bool _isFinishAlreadySpawned;

    private void Start()
    {
        PreSpawnAdjustments();
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        SpawnBlock();
    }
    private void PreSpawnAdjustments()
    {
        _remainingXScale = 3;

        Vector3 charPos = CharacterMover.instance.transform.position;
        Vector3 spawnPos = new Vector3(charPos.x, 0, charPos.z - 1.5f); // minus character's initial Z pos / 2 (3)

        StackController _stack = Instantiate(stackPrefab, spawnPos, Quaternion.identity, transform);
        _stack.name = "Stack_" + _stackIndex;

        currentStackController = _stack;
        spawnedStacks.Add(currentStackController);
        currentStackController.SetIndex(_stackIndex);
        GetCurrentInitialBlock().SetColor(ColorManager.instance.GetColorFromIndex(0, stackIndex: _stackIndex));

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
        if (ArrivedToFinish())
        {
            if (!_isFinishAlreadySpawned)
            {
                _isFinishAlreadySpawned = true;
                SpawnFinishLine();
                return;
            }
            return;
        }

        ParentBlockClass lastSpawned = GetLastSpawnedBlock();
        Vector3 lastPos = lastSpawned.transform.position;

        float zPos = (lastSpawned.transform.localScale.z / 2) + lastPos.z + 1.5f;
        int index = (GetLastSpawnedBlock().GetIndex() + 1);
        int sign = (GetLastSpawnedBlock().GetIndex() + 1) % 2 == 0 ? 1 : -1;

        Vector3 spawnPos = new Vector3(3 * sign, _constVerticalPos, zPos);

        BlockMovementController newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, currentStackController.transform);
        newBlock.transform.localScale = new Vector3(_remainingXScale, 1, 3);// y and z scale values are constant never get modified

        currentStackController.AddBlock(newBlock);
        newBlock.Initialize(index);
    }

    void SpawnFinishLine()
    {
        ParentBlockClass lastSpawned = GetLastSpawnedBlock();
        Vector3 lastPos = lastSpawned.transform.position;
        Vector3 spawnPos = new Vector3(lastPos.x, 0, lastPos.z + 3);

        Instantiate(finishLine, spawnPos, Quaternion.identity, null);
    }
    bool ArrivedToFinish()
    {
        int lastSpawnedIndex = currentStackController.GetListCount();
        return lastSpawnedIndex == maxBlockCount;
    }
    #region Getters / Settters

    public ParentBlockClass GetLastSpawnedBlock()
    {
        return currentStackController.GetLastSpawned();
    }
    public ParentBlockClass GetBlockFromIndexNo(int targetIndex)
    {
        return currentStackController.GetBlockFromIndexNo(targetIndex);
    }
    private ParentBlockClass GetCurrentInitialBlock()
    {
        return currentStackController.GetBlockFromIndexNo(0);
    }

    public int GetMaxBlockCount()
    {
        return maxBlockCount;
    }

    public void SetRemainingXScale(float value)
    {
        _remainingXScale = value;
    }
    #endregion
}
