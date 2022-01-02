using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Friendly : MonoBehaviour, IInteractable
{
    public string characterName;
    public ConversationData conversation;
    public UnityEvent conversationEndEvent;

    public bool InConversation { get; set; }
    private int _numTimesTalked;


    public void Interact()
    {
        if (!InConversation)
        {
            StartConversation();
        }
    }


    public void StartConversation()
    {
        InConversation = true;
        GameManager.Instance.dialogManager.LoadConversation(this);
    }


    public void EndConversation()
    {
        _numTimesTalked++;
        conversationEndEvent.Invoke();
        InConversation = false;
    }
}
