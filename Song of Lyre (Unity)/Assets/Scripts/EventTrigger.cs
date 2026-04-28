using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent onEnter, onExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        onEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onExit.Invoke();
    }
}
