using UnityEngine;

public class ParentBlockClass : MonoBehaviour
{
    [Header("Debug")]
    protected int _index;

    public int GetIndex()
    {
        return _index;
    }

    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
