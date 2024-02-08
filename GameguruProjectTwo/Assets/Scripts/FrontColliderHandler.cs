using UnityEngine;

public class FrontColliderHandler : MonoSingleton<FrontColliderHandler>
{
    public event System.Action<Transform> PrepareToMoveNewBlockEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlockMover _newBlockMover))
        {
            BlockMover currenBlock = GroundChecker.instance.GetCurrentBlock();

            if (currenBlock == _newBlockMover) return;

            if (_newBlockMover.IsBlockStopped())
            {
                PrepareToMoveNewBlockEvent?.Invoke(_newBlockMover.transform);
            }
            else
                Debug.LogWarning("Fail, new block s still moving");
        }
    }
}
