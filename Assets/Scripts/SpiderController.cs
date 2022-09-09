using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpiderController : MonoBehaviour
{
    [SerializeField] private float _floatingHeight = 2;
    [SerializeField] private float _floatingForce = 1;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;

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
    }

    private bool CheckHeight()
    {
        bool _timeToUp = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            _timeToUp = true;

            Ray ray = new Ray(hit.point, Vector3.up);
            _forceDirection = (ray.GetPoint(_floatingHeight) - transform.position) * _floatingForce;
        }

        return _timeToUp;
    }

    private void Controls()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        _moveDirection = transform.forward * vertical * _moveSpeed;

        if (horizontal != 0)
            _rigidbody.AddTorque(0, horizontal * _rotateSpeed, 0);
        else
            _rigidbody.rotation = transform.rotation;
    }
}
