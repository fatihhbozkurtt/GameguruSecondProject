using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockMovementController : ParentBlockClass
{
    public event Action BlockStoppedMovingEvent;

    [Header("Configuration")]
    [SerializeField] float delta;  // Amount to move left and right from the start point
    [SerializeField] float speed;
    [SerializeField] bool moveSmoothly;

    [Header("Debug")]
    [SerializeField] bool blockMovement;
    bool _isForward;
    const int _consXScale = 3;
    public void Initialize(int index)
    {
        InputManager.instance.TouchOccuredEvent += OnTouchOccured;

        _index = index;
        gameObject.name = "Block_" + _index.ToString();
    }
    private void OnTouchOccured()
    {
        if (blockMovement) return;

        Stop();

        BlockStoppedMovingEvent?.Invoke();
    }

    public void Stop()
    {
        blockMovement = true;
    }

    void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockMovement) return;

        if (moveSmoothly)
        {
            float xPos = delta * Mathf.Sin(Time.time * speed);
            transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            #region Harsh Movement
            Vector3 position = transform.position;
            int direction = _isForward ? 1 : -1;
            float move = speed * Time.deltaTime * direction;
            position.x += move;

            if (position.x < -_consXScale || position.x > _consXScale)
            {
                position.x = Mathf.Clamp(position.x, -_consXScale, _consXScale);
                _isForward = !_isForward;
            }

            transform.position = position;
            #endregion
        }
    }

    public bool IsBlockStopped()
    {
        return blockMovement;
    }
}

