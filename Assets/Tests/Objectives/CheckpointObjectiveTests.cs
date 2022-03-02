using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class CheckpointObjectiveTests
{
    private CheckpointObjective _checkpointObjective;
    private Checkpoint _requiredCheckpoint;
    private IPlayerManager _player;

    [SetUp]
    public void SetUp()
    {
        _checkpointObjective = new GameObject().AddComponent<CheckpointObjective>();
        _player = Substitute.For<IPlayerManager>();
        _requiredCheckpoint = new GameObject().AddComponent<Checkpoint>();

        _checkpointObjective.Construct(_player);
        _checkpointObjective.Construct(_requiredCheckpoint);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_checkpointObjective.gameObject);
        Object.Destroy(_requiredCheckpoint.gameObject);
    }

    [Test]
    public void UpdateCompletion_PassesObjectiveIfPlayerPassedRequiredCheckpoint()
    {
        _player.LastCheckpoint.Returns(_requiredCheckpoint);

        _checkpointObjective.UpdateCompletion();

        Assert.IsTrue(_checkpointObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_DoesNotPassObjectivePlayerHasNotPassedRequiredCheckpoint()
    {
        Checkpoint otherCheckpoint = new GameObject().AddComponent<Checkpoint>();
        _player.LastCheckpoint.Returns(otherCheckpoint);

        _checkpointObjective.UpdateCompletion();

        Assert.IsFalse(_checkpointObjective.Passed);
        Object.Destroy(otherCheckpoint);
    }
}
