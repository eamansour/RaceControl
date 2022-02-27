public interface ICarAI
{
    /// <summary>
    /// Sets the AI's target position to a pit stop if one is available.
    /// </summary>
    void GoToPit();

    /// <summary>
    /// Sets the AI's next target position to a given checkpoint's position.
    /// </summary>
    void SetTarget(Checkpoint newTarget);
}
