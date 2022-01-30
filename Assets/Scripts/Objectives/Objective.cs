using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour
{
    public bool Passed { get; set; } = false;
    public bool Failed { get; protected set; } = false;

    [field: SerializeField]
    protected TMP_Text ObjectiveText { get; private set; }

    protected IPlayerManager Player { get; private set; }

    private CameraFollow _mainCamera;

    public void Construct(IPlayerManager player)
    {
        Player = player;
    }

    protected virtual void Start()
    {
        _mainCamera = GameObject.FindObjectOfType<CameraFollow>();

        if (Player == null || Player.Equals(null))
        {
            Player = _mainCamera.Target.GetComponent<IPlayerManager>();
        }
    }

    private void Update()
    {
        // The player in camera focus is the main player, so objectives should refer to it
        if (!(Player as Object) || _mainCamera.Target != Player.AttachedGameObject)
        {
            Player = _mainCamera.Target.GetComponent<IPlayerManager>();
        }
    }

    public abstract bool IsComplete();

    public void UpdateUI(bool complete)
    {
        ObjectiveText.color = complete ? Color.green : Color.red;
    }

    public void Reset()
    {
        ObjectiveText.color = Color.white;
        Passed = false;
        Failed = false;
    }
}
