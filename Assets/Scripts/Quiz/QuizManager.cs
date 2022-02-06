using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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
        List<Question> questions = JsonUtility.FromJson<Questions>(_questionsJson.text).questions.ToList();
        SetRandomSample(questions, _questionSampleSize);
        NextQuestion();
    }

    // Retrieves a random sample of questions to be presented
    private void SetRandomSample(List<Question> questions, int sampleSize)
    {
        while (sampleSize > 0)
        {
            if (questions.Count == 0) return;

            int randomIndex = Random.Range(0, questions.Count);
            _sampledQuestions.Add(questions[randomIndex]);

            questions.RemoveAt(randomIndex);
            sampleSize--;
        }
    }

    // Selects an option with a given index
    public void SelectOption(int optionIndex)
    {
        if (_currentQuestion.correctIndex == optionIndex)
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

    // Displays the quiz ending UI
    private void EndQuiz()
    {
        _animator.Play("QuizEnd");

        if (_score > (_questionSampleSize * 0.75f))
        {
            _titleText.text = "Wow! You're a Python pro!";
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
            _titleText.text = "Seems you had a rough time, feel free to try again whenever you're ready!";
        }
    }

    // Updates the UI and retrieves the next question, ending the quiz if all questions are exhausted
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
