public interface IObjective
{
    bool Passed { get; set; }
    bool Failed { get; }

    void Construct(IPlayerManager player);
    void Reset();
    void UpdateCompletion();
    void UpdateUI(bool complete);
}
