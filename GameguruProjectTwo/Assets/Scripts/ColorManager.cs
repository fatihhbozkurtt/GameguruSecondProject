using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoSingleton<ColorManager>
{
    [SerializeField] List<ColorData> colorData;
    [SerializeField] int maxBlockCount;

    private IEnumerator Start()
    {
        yield return null;

    }

    public Color GetColorFromIndex(int index, int stackIndex = 0)
    {
        if (maxBlockCount == 0)
            maxBlockCount = BlockSpawnManager.instance.GetMaxBlockCount();
        if (stackIndex >= colorData.Count) stackIndex -= colorData.Count;

        float t = (float)index / maxBlockCount;  // Calculate t value based on block index

        ColorData data = GetStartEndColors(stackIndex);
        Color lerpedColor = Color.Lerp(data.startColor, data.endColor, t); // Calculate lerped color based on t value

        return lerpedColor;
    }

    ColorData GetStartEndColors(int stackIndex = 0)
    {
        return colorData[stackIndex];
    }
}
[System.Serializable]
public class ColorData
{
    public Color startColor;
    public Color endColor;
}
