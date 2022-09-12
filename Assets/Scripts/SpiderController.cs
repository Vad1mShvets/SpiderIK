using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpiderController : MonoBehaviour
{
    [SerializeField] private ControlSettings _controlSettings;

    [SerializeField] private LegData[] _legs;

    private Rigidbody _rigidbody;

    private Vector3 _forceDirection;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Controls();
    }

    private void FixedUpdate()
    {
        if (CheckHeight())
            _rigidbody.velocity = _forceDirection + _moveDirection;

        LegsMove();
    }

    private bool CheckHeight()
    {
        bool _timeToUp = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            _timeToUp = true;

            Ray ray = new Ray(hit.point, Vector3.up);
            _forceDirection = (ray.GetPoint(_controlSettings.FloatingHeight) - transform.position) * _controlSettings.FloatingForce;
        }

        return _timeToUp;
    }

    private void Controls()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        _moveDirection = transform.forward * vertical * _controlSettings.MoveSpeed;

        if (horizontal != 0)
            _rigidbody.AddTorque(0, horizontal * _controlSettings.RotateSpeed, 0);
        else
            _rigidbody.rotation = transform.rotation;
    }

    private void LegsMove()
    {
        foreach (var leg in _legs)
        {
            leg.LegTransform.transform.position = leg.LegRaycast.LegRaycastPoint;
        }
    }
}

[System.Serializable]
public struct ControlSettings
{
    public float FloatingHeight;
    public float FloatingForce;
    public float MoveSpeed;
    public float RotateSpeed;
}

[System.Serializable]
public struct LegData
{
    public Transform LegTransform;
    public LegRaycast LegRaycast;
}