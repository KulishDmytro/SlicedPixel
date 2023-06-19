using UnityEngine;

namespace Assets.PixelSlicer.Scripts
{
  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  public class PassBufferIntoRegularShader : MonoBehaviour
  {
    [SerializeField] private ComputeShader _cShader;
    [SerializeField] private Transform _controlPoint;
    [SerializeField] private float _maskSize = 1;
    [SerializeField] private float _bladeSize = 1;
    [SerializeField] private float _offsetIntensity = 1;
    [SerializeField] private float _dissolveSpeed = 1;

    [SerializeField] [Range(0.0f, 1.0f)] public float _dissolveCutout = 0.1f;

    private GraphicsBuffer _vectorBuffer;
    private GraphicsBuffer _pixelPosBuffer;
    private Vector3 _controlPointPrev;
    private MeshFilter _meshFilter;
    private Material _material;

    private readonly int _mvBufferId = Shader.PropertyToID("_MvBuffer");
    private readonly int _mvBufferSizeId = Shader.PropertyToID("_MvBufferSize");
    private readonly int _csMvBufferId = Shader.PropertyToID("MvBuffer");
    private readonly int _csPixelPosId = Shader.PropertyToID("PixelPos");
    private readonly int _cPosId = Shader.PropertyToID("cPos");
    private readonly int _cRightId = Shader.PropertyToID("cRight");
    private readonly int _cRotId = Shader.PropertyToID("cRot");
    private readonly int _bladeSizeId = Shader.PropertyToID("BladeSize");
    private readonly int _maskSizeId = Shader.PropertyToID("MaskSize");
    private readonly int _offsetIntensityId = Shader.PropertyToID("OffsetIntensity");
    private readonly int _dissolveSpeedId = Shader.PropertyToID("DissolveSpeed");
    private readonly int _dissolveCutoutId = Shader.PropertyToID("DissolveCutout");
    private readonly int _dtId = Shader.PropertyToID("dt");
    private readonly int _moveIndicationId = Shader.PropertyToID("MoveIndication");

    private void OnEnable()
    {
      if (_vectorBuffer != null || _pixelPosBuffer != null)
        OnDisable();

      // Calculate pixel count from mesh
      // Each pixel constructed from 4 vertex
      _meshFilter = GetComponent<MeshFilter>();
      var mesh = _meshFilter.sharedMesh;
      var pixCount = mesh.vertices.Length / 4;

      // Initialize Graphic Buffers for Compute Shader
      var stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2));
      _vectorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, pixCount, stride);
      _pixelPosBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, pixCount, stride);

      // Get material and set Pixels Count
      _material = GetComponent<MeshRenderer>().sharedMaterial;
      _material.SetInt(_mvBufferSizeId, pixCount);

      // Calculate the position of pixels in world space except rotation
      var pixelPos = new Vector2[pixCount];
      var objScale = new Vector2(transform.localScale.x, transform.localScale.y);
      var objPos = new Vector2(transform.position.x, transform.position.y);

      for (var i = 0; i < pixCount; i++)
        pixelPos[i] = mesh.uv[i*4] * objScale + objPos;

      // Set pixel positions for Motion Vector calculation in Compute Shader
      _pixelPosBuffer.SetData(pixelPos);
    }

    private void FixedUpdate()
    {
      if (_vectorBuffer == null || _pixelPosBuffer == null)
        return;

      // Calculate simple movement delta for movement indication
      var rawDelta = _controlPoint.position - _controlPointPrev;
      var delta = new Vector2(Mathf.Abs(rawDelta.x), Mathf.Abs(rawDelta.y));

      // Set necessary data into Compute Shader for Motion Vector calculation
      UpdateComputeShader(_controlPoint, delta);

      // Set Calculated Motion Vector Buffer into regular shader
      _material.SetBuffer(_mvBufferId, _vectorBuffer);

      // Store old ControlPoint position for correct delta calculations
      _controlPointPrev = _controlPoint.position;
    }

    private void UpdateComputeShader(Transform cPoint, Vector2 delta)
    {
      _cShader.SetVector(_cPosId, cPoint.position);
      _cShader.SetVector(_cRightId, cPoint.right);
      _cShader.SetMatrix(_cRotId, Matrix4x4.Rotate(Quaternion.Inverse(cPoint.rotation)));
      _cShader.SetFloat(_bladeSizeId, _bladeSize);
      _cShader.SetFloat(_maskSizeId, _maskSize);
      _cShader.SetFloat(_offsetIntensityId, _offsetIntensity);
      _cShader.SetFloat(_dissolveSpeedId, _dissolveSpeed);
      _cShader.SetFloat(_dissolveCutoutId, _dissolveCutout);
      _cShader.SetFloat(_dtId, Time.deltaTime);
      _cShader.SetFloat(_moveIndicationId, delta.x > 0.0f || delta.y > 0.0f ? 1.0f : 0.0f);
      _cShader.SetBuffer(0, _csMvBufferId, _vectorBuffer);
      _cShader.SetBuffer(0, _csPixelPosId, _pixelPosBuffer);
      _cShader.Dispatch(0, _pixelPosBuffer.count/64+1, 1, 1);
    }

    private void OnDisable()
    {
      _vectorBuffer?.Dispose();
      _vectorBuffer = null;
      _pixelPosBuffer?.Dispose();
      _pixelPosBuffer = null;
    }

    private void OnDestroy() => OnDisable();
  }
}