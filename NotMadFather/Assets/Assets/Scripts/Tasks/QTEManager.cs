using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject qteUI;
    public TextMeshProUGUI sequenceText;
    public Slider timerSlider; // UI Slider for visual timer

    [Header("Settings")]
    public float timeLimit = 5f;
    public UnityEvent onSuccess;
    public UnityEvent onFailure;

    private List<KeyCode> sequence = new List<KeyCode>();
    private int currentIndex = 0;
    private float timer = 0f;
    private bool isActive = false;

    public bool IsActive => isActive;

    private KeyCode[] possibleKeys = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    public void StartQTE()
    {
        // Setup
        sequence.Clear();
        currentIndex = 0;
        timer = 0f;
        isActive = true;

        // Generate random sequence
        for (int i = 0; i < 8; i++)
        {
            sequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }

        DisplaySequence();

        // Reset timer UI
        if (timerSlider != null)
        {
            timerSlider.maxValue = timeLimit;
            timerSlider.value = 0f;
        }

        qteUI.SetActive(true);
    }

    private void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;

        if (timerSlider != null)
        {
            timerSlider.value = timer;
        }

        if (timer > timeLimit)
        {
            EndQTE(false);
            return;
        }

        // Input check
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(sequence[currentIndex]))
            {
                currentIndex++;
                DisplaySequence(); // Update visual progress

                if (currentIndex >= sequence.Count)
                {
                    EndQTE(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                     || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                EndQTE(false); // Wrong key
            }
        }
    }

    private void DisplaySequence()
    {
        string display = "";

        for (int i = 0; i < sequence.Count; i++)
        {
            if (i < currentIndex)
            {
                display += $"<color=green>{ArrowKeyToSymbol(sequence[i])}</color> ";
            }
            else
            {
                display += $"<color=white>{ArrowKeyToSymbol(sequence[i])}</color> ";
            }
        }

        sequenceText.text = display.Trim();
    }

    private string ArrowKeyToSymbol(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            default: return "?";
        }
    }

    private void EndQTE(bool success)
    {
        isActive = false;
        qteUI.SetActive(false);

        if (success)
            onSuccess.Invoke();
        else
            onFailure.Invoke();
    }
}
