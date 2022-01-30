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
    public void IsComplete_ReturnsTrueIfPlayerPassedRequiredCheckpoint()
    {
        _player.LastCheckpoint.Returns(_requiredCheckpoint);

        bool result = _checkpointObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsComplete_ReturnsFalseIfPlayerHasNotPassedRequiredCheckpoint()
    {
        Checkpoint otherCheckpoint = new GameObject().AddComponent<Checkpoint>();
        _player.LastCheckpoint.Returns(otherCheckpoint);

        bool result = _checkpointObjective.IsComplete();

        Assert.IsFalse(result);
        Object.Destroy(otherCheckpoint);
    }
}
