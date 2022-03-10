[System.Serializable]
public class Question
{
    // Case sensitive: Must match JSON attributes
    public string title;
    public string[] options;
    public string correctAnswer;
}

[System.Serializable]
public class Questions
{
    // Case sensitive: Must match JSON object
    public Question[] questions;
}
