using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject instructionAreaObject;
    private TextMeshProUGUI instructionArea;

    private LinkedList<ConversationLine> conversations;
    private LinkedListNode<ConversationLine> currentLine;

    //Animation Scripts
    private Fader fader;
    [SerializeField] private float fadeDuration;

    private int isInteract = 0;
    private int clicked = 0;

    public int Clicked => clicked;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        conversations = new LinkedList<ConversationLine>();
        fader = FindObjectOfType<Fader>();
        instructionArea = instructionAreaObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public int getInteract()
    {
        return isInteract;
    }

    public void setInteract(int Inter)
    {
        isInteract = Inter;
    }
    public void enableInteraction(GameObject Interaction)
    {
        if (clicked == 0)
        {
            Interaction.SetActive(true);
            disableJoystick();
            Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(Interaction, fadeDuration));
        }
    }

    public void disableInteraction(GameObject Interaction)
    {
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(Interaction, fadeDuration));
        Interaction.SetActive(false);
    }

    public void enableJoystick()
    {
        if (clicked == 0)
        {
            joystick.SetActive(true);
            Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(joystick, fadeDuration));
        }
    }

    public void disableJoystick()
    {
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(joystick, fadeDuration));
        joystick.SetActive(false);
    }

    public void StartInstruction(Conversation conversation)
    {
        if (clicked == 0)
        {
            //Debug.Log("clicked 0");
            currentLine = null;
            conversations.Clear();
            clicked = 1;

            foreach (ConversationLine line in conversation.conversationLines)
            {
                conversations.AddLast(line);
            }

            DisplayNextInstructionLine();
        }
        else
        {
            DisplayNextInstructionLine();
        }
    }

    public void DisplayNextInstructionLine()
    {
        if (currentLine == null)
        {
            currentLine = conversations.First;
        }
        else
        {
            if (currentLine.Next == null)
            {
                clearInstruction();
                StartCoroutine(FadeOutAndUpdateContent());
                instructionAreaObject.gameObject.SetActive(false);
                setInteract(0);
                clicked = 0;
                enableJoystick();
                return;
            }
            currentLine = currentLine.Next;
        }

        DisplayCurrentLine();
    }

    public void DisplayPreviousInstructionLine()
    {
        if (currentLine == null)
        {
            currentLine = conversations.First;
        }
        else
        {
            if (currentLine.Previous == null)
            {
                return;
            }
            currentLine = currentLine.Previous;
        }
        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        //StartCoroutine(FadeOutAndUpdateContent());
        DisplayCurrentLineInstant();
    }

    private void DisplayCurrentLineInstant()
    {
        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        ConversationLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.Conversation.ToString();
    }

    private IEnumerator FadeOutAndUpdateContent()
    {
        // Start fade-out for both objects simultaneously
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-out processes are complete
        yield return fadeOutInstructionArea;

        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        ConversationLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.Conversation.ToString();

        // Ensure all updates are processed before starting fade-in
        yield return null;

        // Start fade-in for both objects simultaneously
        Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-in processes are complete (optional)
        yield return fadeInInstructionArea;
    }

    public void closeInstruction()
    {
        currentLine = null;
        clearInstruction();
    }

    private void clearInstruction()
    {
        instructionArea.text = "";
    }
}
