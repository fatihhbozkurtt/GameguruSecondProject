using UnityEngine;

public class FrontCollisionHandler : MonoSingleton<FrontCollisionHandler>
{
    public event System.Action<Transform> PrepareToMoveNewBlockEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ParentBlockClass parentClass))
        {
            if (!GameManager.instance.isLevelActive) return;

            ParentBlockClass currentBlock = GroundChecker.instance.GetCurrentBlock();
            BlockMovementController newBlock = parentClass.GetComponent<BlockMovementController>();

            if (currentBlock == parentClass) return;
            if (newBlock == null) return; // just parent calss detected such as startPlatform or Initial Block

            if (newBlock.IsBlockStopped())
            {
                PrepareToMoveNewBlockEvent?.Invoke(parentClass.transform);
            }
            else
            {

                Debug.Log("Fail, new block's still moving");
            }
        }
    }


}
