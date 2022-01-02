using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadableManager : MonoBehaviour
{
    public bool IsOpen { get; set; }
    private Readable _curReadable;

    public void LoadReadable(Readable readable)
    {
        IsOpen = true;
        _curReadable = readable;
        GameManager.Instance.player.EnableMovementAndFreeLook(false);
        GameManager.Instance.player.AllowInput = false;
        GameManager.Instance.hud.SetReadableUI(_curReadable.readableContent);
    }


    public void CloseReadable()
    {
        GameManager.Instance.hud.CloseReadableUI();
        GameManager.Instance.player.EnableMovementAndFreeLook(true);
        GameManager.Instance.player.AllowInput = true;
        _curReadable = null;
        IsOpen = false;
    }
}
