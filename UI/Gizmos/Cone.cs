using System;
using UnityEngine;

namespace LaneController.UI.Gizmos
{
    public static class Cone
    {
        public static GameObject Create(Material material, int subdivisions = 10, float radius = 1f, float height = 2f)
        {
            GameObject gameObject = new("Cone");
            gameObject.AddComponent<MeshFilter>().sharedMesh = CreateMesh(subdivisions, radius, height);
            gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
            return gameObject;
        }

        private static Mesh CreateMesh(int subdivisions, float radius, float height)
        {
            Mesh mesh = new();
            Vector3[] array = new Vector3[subdivisions + 2];
            Vector2[] array2 = new Vector2[array.Length];
            int[] array3 = new int[subdivisions * 2 * 3];
            array[0] = Vector3.zero;
            array2[0] = new Vector2(0.5f, 0f);
            int i = 0;
            int num = subdivisions - 1;
            for (; i < subdivisions; i++)
            {
                float num2 = (float)i / (float)num;
                float f = num2 * ((float)Math.PI * 2f);
                float x = Mathf.Cos(f) * radius;
                float z = Mathf.Sin(f) * radius;
                array[i + 1] = new Vector3(x, 0f, z);
                array2[i + 1] = new Vector2(num2, 0f);
            }
            array[subdivisions + 1] = new Vector3(0f, height, 0f);
            array2[subdivisions + 1] = new Vector2(0.5f, 1f);
            int j = 0;
            for (int num3 = subdivisions - 1; j < num3; j++)
            {
                int num4 = j * 3;
                array3[num4] = 0;
                array3[num4 + 1] = j + 1;
                array3[num4 + 2] = j + 2;
            }
            int num5 = subdivisions * 3;
            int k = 0;
            for (int num6 = subdivisions - 1; k < num6; k++)
            {
                int num7 = (k * 3) + num5;
                array3[num7] = k + 1;
                array3[num7 + 1] = subdivisions + 1;
                array3[num7 + 2] = k + 2;
            }
            mesh.vertices = array;
            mesh.uv = array2;
            mesh.triangles = array3;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
