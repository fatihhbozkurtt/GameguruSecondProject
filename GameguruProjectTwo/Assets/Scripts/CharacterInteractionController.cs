using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionController : MonoBehaviour
{
    CharacterAnimator charAnimator;

    private void Awake()
    {
        charAnimator = GetComponent<CharacterAnimator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.collider.TryGetComponent(out FinishLine finish))
        //{

        //}
    }
}
