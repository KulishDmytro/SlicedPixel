using UnityEngine;

namespace Assets.PixelSlicer.Scripts
{
  public class ControlPointMove : MonoBehaviour
  {
    [SerializeField] private Transform _controlPoint;
    [SerializeField] private float _rotationSpeed = 1;

    private Vector3 _prevControlPoint;

    private void Update()
    {
      if (!Input.GetMouseButton(0))
        return;

      var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      var magnitude = new Vector2(_prevControlPoint.x - mousePos.x, _prevControlPoint.y - mousePos.y).magnitude;

      if (magnitude > 0)
      {
        var position = _controlPoint.position;
        var direction = new Vector3(mousePos.x - position.x, mousePos.y - position.y, 0.0f);
        var toQuaternion = Quaternion.LookRotation(direction, Vector3.back);

        _controlPoint.rotation = Quaternion.RotateTowards(_controlPoint.rotation, toQuaternion, _rotationSpeed * Time.deltaTime);
      }

      _prevControlPoint = mousePos;
      _controlPoint.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
  }
}