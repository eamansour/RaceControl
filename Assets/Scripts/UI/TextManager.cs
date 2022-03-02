using System.Collections;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    private TMP_Text _messageText;

    [TextArea(5, 20)]
    [SerializeField]
    private string[] messages;
    private int _messageIndex = 0;
    private float _textDelay = 0.05f;
    private bool _isWriting = false;
    private Coroutine _runningCoroutine;

    private void Start()
    {
        _messageText = GetComponentInChildren<TMP_Text>();
        _runningCoroutine = StartCoroutine(WriteText());
    }

    /// <summary>
    /// Directly sets the message text, displaying a message immediately.
    /// </summary>
    public void SetMessage(string message)
    {
        _messageText.text = message.Replace("\\n", "\n");
    }

    /// <summary>
    /// Finishes the current message or begins writing the next message onto the UI.
    /// </summary>
    public void DisplayMessage()
    {
        // Skip the animation of the current message
        if (_isWriting)
        {
            StopCoroutine(_runningCoroutine);
            _isWriting = false;
            _messageText.text = messages[_messageIndex];
            return;
        }

        // Begin writing the next message
        _messageIndex++;
        if (_messageIndex < messages.Length)
        {
            _runningCoroutine = StartCoroutine(WriteText());
        }
        else
        {
            Destroy(_messageText.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Applies a letter-by-letter animation to displaying text.
    /// </summary>
    private IEnumerator WriteText()
    {
        if (messages.Length == 0) yield break;

        _isWriting = true;
        string currentMessage = messages[_messageIndex];
        for (int i = 0; i <= currentMessage.Length; i++)
        {
            // Add a slight delay between each typing sound
            if (i % 2 == 0)
            {
                SoundManager.PlaySound("Typing");
            }
            _messageText.text = currentMessage.Substring(0, i);
            yield return new WaitForSeconds(_textDelay);
        }
        _isWriting = false;
    }
}
