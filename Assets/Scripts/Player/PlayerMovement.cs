using Eccentric;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(0.55f, 2.5f)] private float _inputSensitivity = 1f;
    [SerializeField][Range(5f, 30f)] private float _xLerpSensitivity = 22f;
    [SerializeField] private float _offset = 2f;
    [SerializeField] private float MouseSensetivity;
    [SerializeField] private float KeyboardSensetivity;
    [Space]
    [SerializeField] private Transform _particle;

    private const string MouseInPut = "Mouse X";
    private const string KeyboardInput = "Horizontal";

    private string _currentInput = "Horizontal";
    private float _currentSensetivity = 1f;
    private float _moveSpeed;
    private float _newPositionX;
    private float _deltaPositionX;
    private float _targetX;
    private float _newPositionZ;

    public bool CanMove { get; set; } = false;
    public bool IsInputAvailable { get; set; }

    private void Start()
    {
        _moveSpeed = SongManager.Speed;
        _currentSensetivity = KeyboardSensetivity;

        if (EccentricInit.Instance.IsMobile)
            MouseSensetivity /= 1.5f;

        //Application.targetFrameRate = -1;
    }

    private void Update()
    {
        CheckInputSource();
        CheckInput();
        Move();
    }

    private void Move()
    {
        if (!CanMove)
            return;

        _newPositionZ += _moveSpeed * Time.deltaTime;

        _targetX += _deltaPositionX;

        _targetX = Mathf.Clamp(_targetX, -_offset, _offset);
        _newPositionX = Mathf.Lerp(_newPositionX, _targetX, Time.deltaTime * _xLerpSensitivity);

        transform.position = new Vector3(_newPositionX, 0, _newPositionZ);
        _particle.position = new Vector3(0, 0, _newPositionZ);
    }

    private void CheckInput()
    {
        _deltaPositionX = Input.GetAxisRaw(_currentInput) * _currentSensetivity * 0.065f;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    IsInputAvailable = true;
        //    SetMousePositions();
        //    _prevPositionX = _localMousePosition.x;
        //    return;
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    IsInputAvailable = false;
        //    return;
        //}
    }

    //private void SetMousePositions()
    //{
    //    _inputMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mousePositionZ);
    //    _worldMousePosition = _camera.ScreenToWorldPoint(_inputMousePosition);
    //    _localMousePosition = _camera.transform.InverseTransformPoint(_worldMousePosition);
    //}

    private void CheckInputSource()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentInput = MouseInPut;
            _currentSensetivity = MouseSensetivity;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _currentInput = KeyboardInput;
            _currentSensetivity = KeyboardSensetivity;
        }
    }
}