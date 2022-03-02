using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class AvoidCheckpointObjectiveTests
{
    private AvoidCheckpointObjective _avoidObjective;
    private Checkpoint _checkpointToAvoid;
    private IPlayerManager _player;

    [SetUp]
    public void SetUp()
    {
        _avoidObjective = new GameObject().AddComponent<AvoidCheckpointObjective>();
        _player = Substitute.For<IPlayerManager>();
        _checkpointToAvoid = new GameObject().AddComponent<Checkpoint>();

        _avoidObjective.Construct(_player);
        _avoidObjective.Construct(_checkpointToAvoid);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_avoidObjective.gameObject);
        Object.Destroy(_checkpointToAvoid.gameObject);
    }

    [Test]
    public void UpdateCompletion_FailsObjectiveIfPlayerPassedCheckpointToAvoid()
    {
        _player.LastCheckpoint.Returns(_checkpointToAvoid);

        _avoidObjective.UpdateCompletion();

        Assert.IsTrue(_avoidObjective.Failed);
    }

    [Test]
    public void UpdateCompletion_DoesNotFailObjectivePlayerHasNotPassedRequiredCheckpoint()
    {
        Checkpoint otherCheckpoint = new GameObject().AddComponent<Checkpoint>();
        _player.LastCheckpoint.Returns(otherCheckpoint);

        _avoidObjective.UpdateCompletion();

        Assert.IsFalse(_avoidObjective.Failed);
        Object.Destroy(otherCheckpoint);
    }
}
