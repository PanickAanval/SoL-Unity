using UnityEngine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Yarn.Unity;


public class NewDialogue3 : MonoBehaviour
{
    [SerializeField] DialogueRunner? dialogueRunner;

    [SerializeField] DialogueReference? dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async YarnTask StartDialogue(string Teleporter)
    {
        dialogueRunner.StartDialogue("Teleporter"); 
    }

    public void SwitchDialogue()
    {
        Debug.Log("kutzooi");
        StartDialogue("Teleporter");
    }

}
