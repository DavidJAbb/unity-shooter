using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    private Friendly _friendly;
    private Dialog _curDialog;


    public void LoadConversation(Friendly friendly)
    {
        GameManager.Instance.player.EnableMovementAndFreeLook(false);
        GameManager.Instance.player.AllowInput = false;

        _friendly = friendly;
        // Load the first dialog to test...
        _curDialog = _friendly.conversation.dialogs[0]; // Start on first for now...
        GameManager.Instance.hud.SetDialogUI(_curDialog);
    }

    public void SelectResponse(int id)
    {
        if (_curDialog.responses[id].next == -2)
        {
            // End convo...
            EndConversation();
            return;
        }

        // Debug.Log($"Response selected: {id}");
        _curDialog = _friendly.conversation.GetDialogByLineID(_curDialog.responses[id].next);
        if(_curDialog != null)
        {
            GameManager.Instance.hud.SetDialogUI(_curDialog);
        }
        else
        {
            // Fail state - just end convo...
            Debug.Log($"Dialog with line ID {id} could not be found...");
            EndConversation();
        }
    }

    public void EndConversation()
    {
        _curDialog = null;
        _friendly.EndConversation();
        _friendly = null;
        GameManager.Instance.hud.CloseDialogUI();

        GameManager.Instance.player.EnableMovementAndFreeLook(true);
        GameManager.Instance.player.AllowInput = true;
    }
}
