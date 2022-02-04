using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelFeedback : MonoBehaviour, ILevelFeedback
{
    private TMP_Text _feedbackText;
    private TMP_Text _statisticsText;
    private Image _background;

    [SerializeField]
    private Transform _consoleContent;

    [SerializeField]
    private Color _winColour;

    [SerializeField]
    private Color _loseColour;

    [TextArea(5, 20)]
    [SerializeField]
    private string _winMessage;

    [TextArea(5, 20)]
    [SerializeField]
    private string _loseMessage;

    // Initialise the background image and text UI elements
    private void Awake()
    {
        _background = GetComponent<Image>();
        _feedbackText = GetComponentInChildren<TMP_Text>();
        _statisticsText = transform.Find("StatisticsPanel").GetComponentInChildren<TMP_Text>();
    }

    // Set the appropriate background and feedback text depending on the level's outcome
    public void DisplayFeedback(bool win)
    {
        gameObject.SetActive(true);
        _background.color = win ? _winColour : _loseColour;
        _feedbackText.text = win ? _winMessage : _loseMessage;
        DisplayProgramStatistics();
    }

    // Analyses and displays the inputted program's statistics
    private void DisplayProgramStatistics()
    {
        if (!_statisticsText || !_consoleContent) return;

        int numberOfStatements = _consoleContent.childCount;

        CompoundStatement[] compoundStatements = _consoleContent.GetComponentsInChildren<CompoundStatement>();
        foreach (CompoundStatement statement in compoundStatements)
        {
            foreach (Transform child in statement.transform)
            {
                if (child.GetComponent<Statement>())
                {
                    numberOfStatements++;
                }
            }
        }

        int cyclomaticComplexity = numberOfStatements >= 1 ? 1 : 0;

        // Each compound statement increases cyclomatic complexity by 1
        cyclomaticComplexity += compoundStatements.Length;

        string statistics = $"Number of statements: {numberOfStatements}\n" +
                            $"Complexity: {cyclomaticComplexity}";

        _statisticsText.text = statistics;
    }
}
