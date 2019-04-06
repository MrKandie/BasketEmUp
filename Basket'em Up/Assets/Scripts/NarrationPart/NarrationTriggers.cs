using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class NarrationTriggers : MonoBehaviour
{
    [Header("Trigger 0 Ref")]
    public CinemachineVirtualCamera bossZoomCamera;

    public int whichTrigger;

    public AudioClip[] dialogueClips;
    public AudioSource myAudioSource;
    public TextMeshProUGUI dialogueText;
    public Image blackUIImage;
    public string[] dialogueArray;
    public Color[] dialogueColor;
    private string actualDialogue;
    private bool dialogueActive;
    private int howMuchPoint = 1;

    public PostProcessProfile normalPP;
    public PostProcessProfile zoomOnBossPP;
    public PostProcessVolume cameraVolumePP;
    public TextMeshPro quarterbackTMP;

    private void Start()
    {
        StartCoroutine(PointingEndOfDialogues());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            switch (whichTrigger)
            {
                case 0:
                    myAudioSource.Stop();
                    SetDialogueText(0);
                    SetImageAlpha();
                    break;
                case 1:
                    myAudioSource.Stop();
                    SetDialogueText(1);
                    SetImageAlpha();
                    break;
                case 2:
                    myAudioSource.Stop();
                    SetDialogueText(2);
                    SetImageAlpha();
                    break;
                case 3:
                    myAudioSource.Stop();
                    SetDialogueText(3);
                    SetImageAlpha();
                    break;
                case 4:
                    bossZoomCamera.m_Priority = 11;
                    break;
                case 5:
                    cameraVolumePP.profile = zoomOnBossPP;
                    quarterbackTMP.enabled = true;
                    Time.timeScale = 0;
                    break;
                case 6:
                    cameraVolumePP.profile = normalPP;
                    quarterbackTMP.enabled = false;
                    Time.timeScale = 1;
                    break;
                case 7:
                    myAudioSource.Stop();
                    SetDialogueText(4);
                    SetImageAlpha();
                    break;
                case 8:
                    bossZoomCamera.m_Priority = 9;
                    break;
                case 9:
                    myAudioSource.Stop();
                    SetDialogueText(5);
                    SetImageAlpha();
                    break;
            }
            whichTrigger++;
        }
    }

    IEnumerator PointingEndOfDialogues()
    {
        if (dialogueActive)
        {
            switch (howMuchPoint)
            {
                case 0:
                    dialogueText.text = actualDialogue + " ";
                    howMuchPoint++;
                    break;
                case 1:
                    dialogueText.text = actualDialogue + " .";
                    howMuchPoint++;
                    break;
                case 2:
                    dialogueText.text = actualDialogue + " ..";
                    howMuchPoint++;
                    break;
                case 3:
                    dialogueText.text = actualDialogue + " ...";
                    howMuchPoint -= 3;
                    break;
            }
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(PointingEndOfDialogues());
    }

    IEnumerator HideText(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        dialogueActive = false;
        Color newColor = Color.black;
        newColor.a = 0;
        blackUIImage.color = newColor;
        dialogueText.text = "";
    }

    void SetDialogueText(int _whichDialogue)
    {
        //StopAllCoroutines();
        dialogueActive = true;
        StartCoroutine(HideText(dialogueClips[_whichDialogue].length));
        dialogueText.text = dialogueArray[_whichDialogue];
        actualDialogue = dialogueArray[_whichDialogue];
        dialogueText.color = dialogueColor[_whichDialogue];
        myAudioSource.PlayOneShot(dialogueClips[_whichDialogue]);
    }

    void SetImageAlpha()
    {
        Color newColor = Color.black;
        newColor.a = 0.4f;
        blackUIImage.color = newColor;
    }
}
