public interface IObjective
{
    bool Passed { get; }
    bool Failed { get; }

    void Construct(IPlayerManager player);

    /// <summary>
    /// Resets the objective's completion to incomplete (i.e. not passed, not failed).
    /// </summary>
    void Reset();

    /// <summary>
    /// Evaluates the objectives completion, updating the pass and fail states where necessary.
    /// </summary>
    void UpdateCompletion();

    /// <summary>
    /// Displays the appropriate objective UI, depending on the given completion state.
    /// </summary>
    void UpdateUI(bool complete);
}
