using System.Reflection;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public event System.Action TouchOccuredEvent;
    public event System.Action TouchEndEvent;

    public void OnPointerDown()
    {
        TouchOccuredEvent?.Invoke();
    }

    public void OnPointerUp()
    {
        TouchEndEvent?.Invoke();
    }
}
