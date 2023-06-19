using UnityEngine;

namespace Assets.PixelSlicer.Scripts
{
  public class SinglePixelMesh
  {
    public Vector3[] Pos { get; }
    public Vector2[] UV0 { get; }
    public Vector2[] UV1 { get; }
    public int[] Triangles { get; }

    private const int VertCount = 4;
    private const int TrisCount = 6;

    public SinglePixelMesh(float size, int idx, int idy, int pixelCount)
    {
      Pos = new Vector3[VertCount];
      UV0 = new Vector2[VertCount];
      UV1 = new Vector2[VertCount];
      Triangles = new int[TrisCount];

      // Generate Base Pixel Position
      size *= 0.5f;
      Pos[0] = new Vector3(x: -size, y: -size, 0.0f);
      Pos[1] = new Vector3(x: -size, y: size, 0.0f);
      Pos[2] = new Vector3(x: size, y: size, 0.0f);
      Pos[3] = new Vector3(x: size, y: -size, 0.0f);

      // Offset pixel to correct position
      for (var i = 0; i < Pos.Length; i++)
      {
        var xOffset = idx * size * 2 + size;
        var yOffset = idy * size * 2 + size;

        Pos[i] += new Vector3(xOffset, yOffset, 0.0f);
      }

      // Generate noise
      var noise = Random.Range(0.0f, 1.0f);
      for (var i = 0; i < UV0.Length; i++)
      {
        // RG uv channel - store pixel center
        UV0[i] = new Vector2(Pos[2].x - size, Pos[2].y - size);

        // R uv1 channel - store Pixel Id
        // G uv1 channel - store one random value for whole pixel
        UV1[i] = new Vector2(pixelCount, noise);
      }

      // Create basic triangle pattern
      Triangles[0] = 0 + pixelCount * 4;
      Triangles[1] = 1 + pixelCount * 4;
      Triangles[2] = 2 + pixelCount * 4;
      Triangles[3] = 0 + pixelCount * 4;
      Triangles[4] = 2 + pixelCount * 4;
      Triangles[5] = 3 + pixelCount * 4;
    }
  }
}