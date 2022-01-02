/*
User Interface Manager - controls the menus, player HUD etc.
Accessible through GameManager.Instance.hud
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    [Header("Crosshair")]
    public RectTransform crosshairRect;
    private float curScale;
    private float targetScale;

    [Header("Health")]
    public TextMeshProUGUI healthTotal;

    [Header("Ammo")]
    public GameObject WeaponHUD_GO;
    public TextMeshProUGUI ammoCurrentClip;
    public TextMeshProUGUI ammoTotal;

    [Header("Dialog")]
    public GameObject dialogBox;
    public TextMeshProUGUI dialogMain;
    public Transform responseTransform;
    public GameObject responseBtnPrefab;
    private List<GameObject> _responseBtns = new List<GameObject>();

    [Header("Readables")]
    public GameObject readablePanel;
    public TextMeshProUGUI readableTextBox;

    string[] numbers;

    // Start is called before the first frame update
    void Start()
    {
        numbers = new string[250];

        for(int i = 0; i < 250; i++)
        {
            numbers[i] = string.Format("{0:00}", i);
        }

        curScale = 1f;
        targetScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Crosshair expands when shooting (and player move speed?)
        if (curScale != targetScale)
        {
            curScale = Mathf.Lerp(curScale, targetScale, Time.deltaTime * 3);
            crosshairRect.localScale = new Vector3(curScale, curScale, 1f);
        }
    }

    public void ScaleCrosshair(float newScale)
    {
        curScale = newScale;
        crosshairRect.localScale = new Vector3(curScale, curScale, 1f);
    }

    public void ResetCrosshair()
    {
        crosshairRect.localScale = new Vector3(1f, 1f, 1f);
    }

    public void UpdateHealth(int health)
    {
        healthTotal.text = numbers[health];
    }

    public void TweenHealth(float from, float to)
    {
        LeanTween.value(gameObject, from, to, 0.25f).setOnUpdate(
            (float val) => { healthTotal.text = numbers[(int)val];
        });
    }

    public void UpdateAmmo(int curClip, int total)
    {
        ammoCurrentClip.text = numbers[curClip];
        ammoTotal.text = numbers[total];
    }

    // Dialog
    public void SetDialogUI(Dialog dialog)
    {
        if(_responseBtns.Count > 0)
        {
            ClearResponseBtns();
        }
        // Show UI if not already showing...
        dialogBox.SetActive(true);
        // Load the name and line
        dialogMain.text = dialog.line.content;
        // If there are responses - spawn in and set dialog button instances
        if(dialog.responses.Length > 0)
        {
            for(int i = 0; i < dialog.responses.Length; i++)
            {
                GameObject clone = Instantiate(responseBtnPrefab, responseTransform.position, responseTransform.rotation);
                clone.transform.SetParent(responseTransform);
                clone.GetComponent<RectTransform>().localScale = Vector3.one;
                clone.GetComponentInChildren<TextMeshProUGUI>().text = dialog.responses[i].content;
                clone.name = $"ResponseBtn_{i}";

                int btnIndex = i;
                clone.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.dialogManager.SelectResponse(btnIndex); });

                _responseBtns.Add(clone);
            }
        }

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ClearResponseBtns()
    {
        for (int i = 0; i < _responseBtns.Count; i++)
        {
            Destroy(_responseBtns[i].gameObject);
        }
        _responseBtns.Clear();
    }

    public void CloseDialogUI()
    {
        ClearResponseBtns();

        dialogBox.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }


    public void SetReadableUI(string content)
    {
        readablePanel.SetActive(true);
        readableTextBox.text = content;
    }

    public void CloseReadableUI()
    {
        readablePanel.SetActive(false);
    }
}
