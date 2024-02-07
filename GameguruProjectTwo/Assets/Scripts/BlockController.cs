using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float delta;  // Amount to move left and right from the start point
    [SerializeField] float speed;
    [SerializeField] bool blockMovement;
    [SerializeField] bool initialBlock;
    [Header("Debug")]
    [SerializeField] int _index;

    public void Initialize(int index)
    {
        InputManager.instance.TouchOccuredEvent += OnTouchOccured;

        _index = index; 
        gameObject.name = "Block_" + _index.ToString();
    }

    private void OnTouchOccured()
    {
        if (blockMovement) return;
        if (initialBlock) return;

        Stop();
        float x = transform.position.x;
        CharacterMover.instance.AssignHorizontalCenter(xValue: x );
    }

    public void Stop()
    {
        blockMovement = true;
    }

    void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (blockMovement) return;

        transform.position = new Vector3(delta * Mathf.Sin(Time.time * speed), transform.position.y, transform.position.z);
    }

    public int GetIndex()
    {
        return _index;
    }
}
