using UnityEngine;

public class ConstantRotater : MonoSingleton<ConstantRotater>
{
    [SerializeField] float speed;
    [SerializeField] Vector3 targetRotation;
    bool canPerform;

    private void Start()
    {
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnArrivedToTheEnd;
    }

    private void OnArrivedToTheEnd()
    {
        canPerform = true;
    }

    private void OnNextLevelStarted()
    {
        canPerform = false;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if (!canPerform) return;
        
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation += targetRotation * speed * Time.deltaTime; // Time.deltaTime ile zaman baðýmlý hýzý hesapla
        transform.localEulerAngles = currentRotation;
    }
}
