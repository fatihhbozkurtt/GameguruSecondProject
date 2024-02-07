using UnityEngine;

public class GameStarter : MonoBehaviour
{

    private void Start()
    {
        GameManager.instance.StartGame();
    }

    // bool ready = false;
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (ready)
    //    {
    //        ready = false;
    //        GameManager.instance.StartGame();
    //    }
    //}
}
