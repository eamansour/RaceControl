public interface ILevelFeedback
{
    /// <summary>
    /// Set the appropriate background and feedback text depending on the level's given outcome.
    /// </summary>
    void DisplayFeedback(bool win);
}
