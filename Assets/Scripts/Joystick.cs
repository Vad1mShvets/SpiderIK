using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public Vector3 Direction { get; private set; }

    [SerializeField] private bool _snap;

    [Space]
    public float DeadZone;

    [SerializeField] private float _dragLimit;

    [Space]
    [SerializeField] private Image _joystickBorder;
    [SerializeField] private Image _joystickCenter;

    private void Start() => _joystickBorder.gameObject.SetActive(false);

    public void PointerDown()
    {
        _joystickBorder.gameObject.SetActive(true);

        _joystickBorder.rectTransform.position = Input.mousePosition;
        _joystickCenter.rectTransform.position = _joystickBorder.rectTransform.position;
    }

    public void PointerDrag()
    {
        ControlJoystick();
    }

    private void ControlJoystick()
    {
        Vector2 direction = Input.mousePosition - _joystickBorder.rectTransform.position;
        Vector2 normalizedDirection = direction.normalized;
        Vector2 fixedDirection = normalizedDirection * _dragLimit;

        if (fixedDirection.magnitude > DeadZone)
        {
            if (_snap)
            {
                _joystickCenter.rectTransform.position = _joystickBorder.rectTransform.position + new Vector3(fixedDirection.x, fixedDirection.y, 0);

                Horizontal = normalizedDirection.x;
                Vertical = normalizedDirection.y;
            }
            else
            {
                if (direction.magnitude < _dragLimit)
                {
                    _joystickCenter.rectTransform.position = Input.mousePosition;

                    Horizontal = Mathf.Clamp(direction.x, -_dragLimit, _dragLimit) / _dragLimit;
                    Vertical = Mathf.Clamp(direction.y, -_dragLimit, _dragLimit) / _dragLimit;
                }
                else
                {
                    _joystickCenter.rectTransform.position = _joystickBorder.rectTransform.position + new Vector3(fixedDirection.x, fixedDirection.y, 0);

                    Horizontal = normalizedDirection.x;
                    Vertical = normalizedDirection.y;
                }
            }
        }
    }

    public void PointerUp()
    {
        _joystickBorder.gameObject.SetActive(false);

        Horizontal = 0;
        Vertical = 0;
    }
}
