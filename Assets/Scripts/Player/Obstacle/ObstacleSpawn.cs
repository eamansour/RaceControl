using UnityEngine;

public class ObstacleSpawn : MonoBehaviour, IObstacleSpawn
{
    private const float SpawnForce = 3000f;

    [SerializeField]
    private Rigidbody _obstacle;

    public void Construct(Rigidbody obstacle)
    {
        _obstacle = obstacle;
    }

    /// <inheritdoc />
    public void SpawnObstacle()
    {
        Rigidbody clone = Instantiate(_obstacle, transform.position, _obstacle.transform.rotation);
        clone.AddForce(transform.forward * SpawnForce);
    }
}
