[System.Serializable]
public class Question
{
    // Case sensitive: Must match JSON attributes
    public string title;
    public string[] options;
    public int correctIndex;
}

[System.Serializable]
public class Questions
{
    // Case sensitive: Must match JSON object
    public Question[] questions;
}
