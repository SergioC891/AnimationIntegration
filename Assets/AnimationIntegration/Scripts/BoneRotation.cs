using UnityEngine;

public class BoneRotation : MonoBehaviour
{
    public Transform targetBone;
    public float rotSpeed = 5.0f;
    public float minAngleLimit = -130.0f;
    public float maxAngleLimit = 130.0f;
    public float rotationSpeedMouse = 16.0f;

    public GameObject objTransform;

    private float _rotX = 0.0f;

    Camera _camera;
    private Vector3 target;

    private Animator _animator;

    void Start()
    {
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    public void LateUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit mouseHit;

        if (Physics.Raycast(ray, out mouseHit))
        {
            Vector3 _targetPos = mouseHit.point;

            objTransform.transform.LookAt(_targetPos);
        }

        if (objTransform.transform.eulerAngles.y < 180.0f)
        {
            _rotX = -objTransform.transform.eulerAngles.y + transform.eulerAngles.y;
        }
        else
        {
            _rotX = -(objTransform.transform.eulerAngles.y - 360.0f) + transform.eulerAngles.y;
        }
/*
        if (_rotX > maxAngleLimit)
        {
            targetBone.localEulerAngles = new Vector3(maxAngleLimit, 0, 0);
        }
        else if (_rotX < minAngleLimit)
        {
            targetBone.localEulerAngles = new Vector3(minAngleLimit, 0, 0);
        }
        else if (_rotX > minAngleLimit && _rotX < maxAngleLimit)
        {
            targetBone.localEulerAngles = new Vector3(_rotX, 0, 0);
        }
*/
        if (!_animator.GetBool("Finishing"))
        {
            targetBone.localEulerAngles = new Vector3(_rotX, 0, 0);
        }
        else
        {
            targetBone.localEulerAngles = Vector3.zero;
        }
    }
}
