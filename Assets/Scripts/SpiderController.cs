using System.Collections;
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

        StartCoroutine(Init());

        Application.targetFrameRate = 144;
    }

    private IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _legs.Length; i++)
            _legs[i].TargetLegPosition = _legs[i].LegRaycast.RaycastPoint;
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

        if (Physics.Raycast(transform.position, Vector3.down + transform.forward, out hit))
        {
            _timeToUp = true;

            Ray ray = new Ray(hit.point, Vector3.up);
            _forceDirection = (new Vector3(transform.position.x, ray.GetPoint(_controlSettings.FloatingHeight).y, transform.position.z) - transform.position) * _controlSettings.FloatingForce;
        }

        Debug.DrawLine(transform.position, hit.point, Color.red);

        return _timeToUp;
    }

    private void Controls()
    {
        Vector3 joystickDirection = new Vector3(_controlSettings.JoystickController.Horizontal, 0, _controlSettings.JoystickController.Vertical);

        if (joystickDirection.magnitude != 0)
        {
            _moveDirection = _controlSettings.MoveSpeed * joystickDirection;
            transform.forward = joystickDirection;
        }
        else
        {
            _moveDirection = Vector3.zero;
        }
    }

    private void LegsMove()
    {
        for (int i = 0; i < _legs.Length; i++)
        {
            var leg = _legs[i];

            Vector3 direction = leg.LegRaycast.RaycastPoint - leg.TargetLegPosition;

            if (direction.magnitude >= _controlSettings.StepLength)
            {
                StartCoroutine(MakeStep(leg));
            }
            else
            {
                leg.LegTransform.position = leg.TargetLegPosition;
            }
        }
    }

    private IEnumerator MakeStep(LegData leg)
    {
        leg.TargetLegPosition = leg.LegRaycast.RaycastPoint;

        leg.LegTransform.position = leg.LegRaycast.RaycastPoint;

        yield return null;
    }
}

[System.Serializable]
public struct ControlSettings
{
    public Joystick JoystickController;
    public AnimationCurve StepHeightAnimation;
    public float FloatingHeight;
    public float FloatingForce;
    public float MoveSpeed;
    public float RotateSpeed;
    public float StepLength;
}

[System.Serializable]
public class LegData
{
    public Vector3 TargetLegPosition;
    public Transform LegTransform;
    public LegRaycast LegRaycast;
}