using System.Collections.Generic;
using System;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    public event Action BlockStoppedMovingEvent;

    [Header("Configuration")]
    [SerializeField] float delta;  // Amount to move left and right from the start point
    [SerializeField] float speed;
    [SerializeField] bool blockMovement;
    public bool initialBlock;

    [Header("Debug")]
    [SerializeField] int _index;
    float initXPos;

    public void Initialize(int index)
    {
        InputManager.instance.TouchOccuredEvent += OnTouchOccured;

        _index = index;
        gameObject.name = "Block_" + _index.ToString();
        initXPos = transform.position.x;
    }
    private void OnTouchOccured()
    {
        if (blockMovement) return;
        if (initialBlock) return;

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

        float xPos = delta * Mathf.Sin(Time.time * speed);
        transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);

    }
    public int GetIndex()
    {
        return _index;
    }

    public bool IsBlockStopped()
    {
        return blockMovement;
    }
}

