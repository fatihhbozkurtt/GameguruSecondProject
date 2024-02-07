using System;
using UnityEngine;

public class CharacterInteractionController : MonoSingleton<CharacterInteractionController>
{
    public event Action ArrivedToTheFinishEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FinishLine finish))
        {
            
            ArrivedToTheFinishEvent?.Invoke();
        }
    }
}
