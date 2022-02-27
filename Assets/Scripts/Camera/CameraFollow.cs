using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Target { get; set; }

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private Vector3 _eulerRotation;

    [SerializeField]
    private float _offsetSmoothing;

    private bool _followBehind = false;

    private void Start()
    {
        transform.eulerAngles = _eulerRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleFollowBehind();
        }
    }

    /// <summary>
    /// Continuously move the camera over time based on the target's position.
    /// </summary>
    private void FixedUpdate()
    {
        if (_followBehind)
        {
            Vector3 followOffset = Target.transform.up * 3f + Target.transform.forward * 3f;
            transform.position =
                Vector3.Lerp(transform.position, Target.transform.position + followOffset, _offsetSmoothing * Time.deltaTime);
            transform.LookAt(Target.transform.position + Vector3.up);
        }
        else
        {
            transform.eulerAngles = _eulerRotation;
            transform.position =
                Vector3.Lerp(transform.position, Target.transform.position + _offset, _offsetSmoothing * Time.deltaTime);
        }
    }

    /// <summary>
    /// Toggles the camera follow mode to follow behind a target.
    /// </summary>
    public void ToggleFollowBehind()
    {
        _followBehind = !_followBehind;
    }
}
