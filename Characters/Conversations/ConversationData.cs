using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Data/Conversation Data")]
public class ConversationData : ScriptableObject
{
    public int conversationID;
    public Dialog[] dialogs;

    public Dialog GetDialogByLineID(int id)
    {
        for(int i = 0; i < dialogs.Length; i++)
        {
            if(dialogs[i].line.dialogID == id)
            {
                return dialogs[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class Line
{
    public int dialogID; // id of this line
    public string content; // text content of the line
}

[System.Serializable]
public class Response
{
    public string content; // text content of the line
    public int next; // id of next dialog line. -1 is end of convo
}

[System.Serializable]
public class Dialog
{
    public Line line;
    public Response[] responses;
}
