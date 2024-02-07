using DG.Tweening;
using UnityEngine;

public class FinshLine : MonoBehaviour
{
    private void Awake()
    {
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
        transform.DOMove(transform.position, 1f).From(endPos);
    }
}
