using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public Dialogue d;
    public GameObject dPanel;

    public void TriggerDialogue()
    {
        dPanel.SetActive(true);
        FindObjectOfType<DialogueSystem>().StartDialogue(d);
    }

    public void HideDialogue()
    {
        dPanel.SetActive(false);
    }
}

