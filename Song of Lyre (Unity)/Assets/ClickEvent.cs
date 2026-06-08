using UnityEngine;
using UnityEngine.Events;

public class ClickEvent : MonoBehaviour
{
    public UnityEvent onButtonClicked;

    public void InvokeButtonEvent()
    {
        onButtonClicked.Invoke();
    }
}
