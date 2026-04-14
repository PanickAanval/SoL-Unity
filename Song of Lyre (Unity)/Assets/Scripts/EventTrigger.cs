using UnityEngine;
using UnityEngine.Events;
public class EventTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent Event;
    void Start()
    {
        
    }

    void OnTriggerEnter()
    {
        Event.Invoke();
    }

    public void Werk()
    {
        print("Werkt");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
