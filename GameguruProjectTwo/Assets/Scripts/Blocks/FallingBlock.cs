using System.Collections;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
