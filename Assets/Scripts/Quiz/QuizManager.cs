using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using Random = System.Random;

public class QuizManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset _questionsJson;

    [SerializeField]
    private TMP_Text _titleText;

    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private Transform _optionsParent;

    [SerializeField]
    private int _questionSampleSize = 1;

    [SerializeField]
    private Animator _animator;

    private List<Question> _sampledQuestions = new List<Question>();
    private Question _currentQuestion;
    private int _currentQuestionIndex = -1;
    private int _score = 0;

    // Retrieve the questions from the JSON file and start the quiz
    private void Start()
    {
        List<Question> questions = new List<Question>();
        try
        {
            questions = JsonUtility.FromJson<Questions>(_questionsJson.text).questions.ToList();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        SetRandomSample(questions, _questionSampleSize);
        NextQuestion();
    }

    /// <summary>
    /// Sets the random sample of questions to be presented in the quiz.
    /// </summary>
    private void SetRandomSample(List<Question> questions, int sampleSize)
    {
        while (sampleSize > 0)
        {
            if (questions.Count == 0) return;

            int randomIndex = UnityEngine.Random.Range(0, questions.Count);
            Question sampledQuestion = questions[randomIndex];

            sampledQuestion.options = ShuffleOptions(sampledQuestion.options);
            _sampledQuestions.Add(sampledQuestion);

            questions.RemoveAt(randomIndex);
            sampleSize--;
        }
    }

    /// <summary>
    /// Randomly shuffles a given array and returns the shuffled array.
    /// Uses the Knuth shuffle algorithm - Knuth, 1969. "The art of computer programming. Volume 2, Seminumerical algorithms".
    /// </summary>
    private string[] ShuffleOptions(string[] options)
    {
        Random random = new Random();
        for (int i = options.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i);
            
            // Swap elements
            string temp = options[i];
            options[i] = options[j];
            options[j] = temp;
        }
        return options;
    }

    /// <summary>
    /// Selects an option with a given index and updates the quiz progress.
    /// </summary>
    public void SelectOption(Button button)
    {
        string optionText = button.GetComponentInChildren<TMP_Text>().text;
        if (_currentQuestion.correctAnswer == optionText)
        {
            _animator.SetTrigger("Correct");
            _score++;
        }
        else
        {
            _animator.SetTrigger("Incorrect");
        }
        _animator.SetTrigger("Done");
        NextQuestion();
    }

    /// <summary>
    /// Displays the quiz ending UI with the appropriate text depending on the user's final score.
    /// </summary>
    private void EndQuiz()
    {
        _animator.Play("QuizEnd");

        if (_score > (_questionSampleSize * 0.75f))
        {
            _titleText.text = "Well done! You've got a great understanding of Python!";
        }
        else if (_score > (_questionSampleSize * 0.5f))
        {
            _titleText.text = "Nice work! Keep working hard and you'll be a Python pro in no time!";
        }
        else if (_score > (_questionSampleSize * 0.25f))
        {
            _titleText.text = "Good try! There's room for improvement so feel free to try again!";
        }
        else
        {
            _titleText.text = "Seems you struggled a bit, feel free to try again whenever you're ready!";
        }
    }

    /// <summary>
    /// Updates the UI and retrieves the next question, ending the quiz if all questions are exhausted.
    /// </summary>
    private void NextQuestion()
    {
        _scoreText.text = $"Score:  {_score} / {_sampledQuestions.Count}";

        // Make sure there are more questions to ask
        _currentQuestionIndex++;
        if (_currentQuestionIndex < _sampledQuestions.Count)
        {
            _currentQuestion = _sampledQuestions[_currentQuestionIndex];
        }
        else
        {
            EndQuiz();
            return;
        }

        _titleText.text = _currentQuestion.title;
        for (int i = 0; i < _optionsParent.childCount; i++)
        {
            TMP_Text optionText = _optionsParent.GetChild(i).GetComponentInChildren<TMP_Text>();
            optionText.text = _currentQuestion.options[i];
        }
    }
}
