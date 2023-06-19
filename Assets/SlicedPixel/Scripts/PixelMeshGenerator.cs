using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelSlicer.Scripts
{
  [ExecuteAlways]
  public class PixelMeshGenerator : MonoBehaviour
  {
    [SerializeField] private Texture2D _texture;
    [SerializeField] private int _pixelPerUnit = 100;
    [SerializeField] private DefaultAsset _path;
    [SerializeField] private string _name = "Mesh";
    [SerializeField] private bool _generate;

    private void Update()
    {
      if (!_generate)
        return;

      var pixels = new List<SinglePixelMesh>();
      var res = _texture.width >= _texture.height ? _texture.width : _texture.height;

      // First pixel should be with 0 id
      var pixelCount = -1;

      for (var i = 0; i < res; i++)
      {
        for (var k = 0; k < res; k++)
        {
          // Get color value for discard all pixels with alpha
          var color = _texture.GetPixel(i, k);
          if (!(Math.Abs(color.a) > 0.0001f))
            continue;

          pixelCount++;
          // Create pixels
          pixels.Add(new SinglePixelMesh(1.0f / _pixelPerUnit, i, k, pixelCount));
        }
      }

      // Generate and save as a file new Mesh
      CreateMesh(pixels);
      _generate = false;
    }

    // Combine all pixel data to one mesh data and create mesh file
    private void CreateMesh(List<SinglePixelMesh> pixels)
    {
      var mesh = new Mesh();
      var vertices = new List<Vector3>();
      var triangles = new List<int>();
      var uv0 = new List<Vector2>();
      var uv1 = new List<Vector2>();

      for (var i = 0; i < pixels.Count; i++)
      {
        vertices.AddRange(pixels[i].Pos);
        triangles.AddRange(pixels[i].Triangles);
        uv0.AddRange(pixels[i].UV0);
        uv1.AddRange(pixels[i].UV1);
      }

      for (var i = 0; i < uv1.Count; i++)
        uv1[i] = new Vector2(uv1[i].x / pixels.Count, uv1[i].y);

      mesh.vertices = vertices.ToArray();
      mesh.triangles = triangles.ToArray();
      mesh.uv = uv0.ToArray();
      mesh.uv2 = uv1.ToArray();

      var path = AssetDatabase.GetAssetPath(_path) + $"/{_name}.asset";

      AssetDatabase.DeleteAsset(path);
      AssetDatabase.CreateAsset(mesh, path);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
    }
  }
}