using UnityEngine;

public class ConstantRotater : MonoSingleton<ConstantRotater>
{
    [SerializeField] float speed;
    [SerializeField] Vector3 targetRotation;

    private void Update()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation += targetRotation * speed * Time.deltaTime; // Time.deltaTime ile zaman baðýmlý hýzý hesapla
        transform.localEulerAngles = currentRotation;
    }
}
