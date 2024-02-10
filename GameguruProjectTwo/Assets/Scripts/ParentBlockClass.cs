using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentBlockClass : MonoBehaviour
{
    [Header("Debug")]
    protected int _index;

    public int GetIndex()
    {
        return _index;
    }

}
