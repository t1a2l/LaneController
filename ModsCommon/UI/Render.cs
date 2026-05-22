using System;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public static class Render
    {
        private static readonly int[] kTriangleIndices = [0, 1, 3, 3, 1, 2];

        private static readonly int[] kSlicedTriangleIndices =
        [
            0, 1, 2, 2, 3, 0, 4, 5, 6, 6,
            7, 4, 8, 9, 10, 10, 11, 8, 12, 13,
            14, 14, 15, 12, 1, 4, 7, 7, 2, 1,
            9, 12, 15, 15, 10, 9, 3, 2, 9, 9,
            8, 3, 7, 6, 13, 13, 12, 7, 2, 7,
            12, 12, 9, 2
        ];

        private static readonly int[][] kHorzFill =
        [
            [0, 1, 4, 5],
            [3, 2, 7, 6],
            [8, 9, 12, 13],
            [11, 10, 15, 14]
        ];

        private static readonly int[][] kVertFill =
        [
            [11, 8, 3, 0],
            [10, 9, 2, 1],
            [15, 12, 7, 4],
            [14, 13, 6, 5]
        ];

        private static readonly Vector3[] _Vertices = new Vector3[16];

        private static readonly Vector2[] _UV = new Vector2[16];

        private static readonly int[][] kFillIndices =
        [
            new int[4],
            new int[4],
            new int[4],
            new int[4]
        ];

        public static void RenderSlicedSprite(UIRenderData renderData, RenderOptions options)
        {
            options._baseIndex = renderData.vertices.Count;
            RebuildSlicedTriangles(renderData, options);
            RebuildSlicedVertices(renderData, options);
            RebuildSlicedUV(renderData, options);
            RebuildSlicedColors(renderData, options);
            if (options._fillAmount < 1f)
            {
                DoSlicedFill(renderData, options);
            }
        }

        private static void RebuildSlicedTriangles(UIRenderData renderData, RenderOptions options)
        {
            int baseIndex = options._baseIndex;
            PoolList<int> triangles = renderData.triangles;
            for (int i = 0; i < kSlicedTriangleIndices.Length; i++)
            {
                triangles.Add(baseIndex + kSlicedTriangleIndices[i]);
            }
        }

        private static void RebuildSlicedVertices(UIRenderData renderData, RenderOptions options)
        {
            float x = 0f;
            float y = 0f;
            float num = Mathf.Ceil(options._size.x);
            float num2 = Mathf.Ceil(0f - options._size.y);
            UITextureAtlas.SpriteInfo spriteInfo = options._spriteInfo;
            int num3 = spriteInfo.border.left;
            int num4 = spriteInfo.border.top;
            int num5 = spriteInfo.border.right;
            int num6 = spriteInfo.border.bottom;
            if (options._flip.IsFlagSet(UISpriteFlip.FlipHorizontal))
            {
                int num7 = num5;
                num5 = num3;
                num3 = num7;
            }
            if (options._flip.IsFlagSet(UISpriteFlip.FlipVertical))
            {
                int num8 = num6;
                num6 = num4;
                num4 = num8;
            }
            Vector3[] vertices = _Vertices;
            vertices[0] = new Vector3(x, y, 0f) + options._offset;
            vertices[1] = vertices[0] + new Vector3(num3, 0f, 0f);
            vertices[2] = vertices[0] + new Vector3(num3, 0f - num4, 0f);
            vertices[3] = vertices[0] + new Vector3(0f, 0f - num4, 0f);
            vertices[4] = new Vector3(num - num5, y, 0f) + options._offset;
            vertices[5] = vertices[4] + new Vector3(num5, 0f, 0f);
            vertices[6] = vertices[4] + new Vector3(num5, 0f - num4, 0f);
            vertices[7] = vertices[4] + new Vector3(0f, 0f - num4, 0f);
            vertices[8] = new Vector3(x, num2 + num6, 0f) + options._offset;
            vertices[9] = vertices[8] + new Vector3(num3, 0f, 0f);
            vertices[10] = vertices[8] + new Vector3(num3, 0f - num6, 0f);
            vertices[11] = vertices[8] + new Vector3(0f, 0f - num6, 0f);
            vertices[12] = new Vector3(num - num5, num2 + num6, 0f) + options._offset;
            vertices[13] = vertices[12] + new Vector3(num5, 0f, 0f);
            vertices[14] = vertices[12] + new Vector3(num5, 0f - num6, 0f);
            vertices[15] = vertices[12] + new Vector3(0f, 0f - num6, 0f);
            for (int i = 0; i < vertices.Length; i++)
            {
                renderData.vertices.Add((vertices[i] * options._pixelsToUnits).Quantize(options._pixelsToUnits));
            }
        }

        private static void RebuildSlicedUV(UIRenderData renderData, RenderOptions options)
        {
            UITextureAtlas atlas = options._atlas;
            Vector2 vector = new(atlas.texture.width, atlas.texture.height);
            UITextureAtlas.SpriteInfo spriteInfo = options._spriteInfo;
            float num = spriteInfo.border.top / vector.y;
            float num2 = spriteInfo.border.bottom / vector.y;
            float num3 = spriteInfo.border.left / vector.x;
            float num4 = spriteInfo.border.right / vector.x;
            Rect region = spriteInfo.region;
            Vector2[] uV = _UV;
            uV[0] = new Vector2(region.x, region.yMax);
            uV[1] = new Vector2(region.x + num3, region.yMax);
            uV[2] = new Vector2(region.x + num3, region.yMax - num);
            uV[3] = new Vector2(region.x, region.yMax - num);
            uV[4] = new Vector2(region.xMax - num4, region.yMax);
            uV[5] = new Vector2(region.xMax, region.yMax);
            uV[6] = new Vector2(region.xMax, region.yMax - num);
            uV[7] = new Vector2(region.xMax - num4, region.yMax - num);
            uV[8] = new Vector2(region.x, region.y + num2);
            uV[9] = new Vector2(region.x + num3, region.y + num2);
            uV[10] = new Vector2(region.x + num3, region.y);
            uV[11] = new Vector2(region.x, region.y);
            uV[12] = new Vector2(region.xMax - num4, region.y + num2);
            uV[13] = new Vector2(region.xMax, region.y + num2);
            uV[14] = new Vector2(region.xMax, region.y);
            uV[15] = new Vector2(region.xMax - num4, region.y);
            if (options._flip != UISpriteFlip.None)
            {
                for (int i = 0; i < uV.Length; i += 4)
                {
                    Vector2 zero;
                    if (options._flip.IsFlagSet(UISpriteFlip.FlipHorizontal))
                    {
                        zero = uV[i];
                        uV[i] = uV[i + 1];
                        uV[i + 1] = zero;
                        zero = uV[i + 2];
                        uV[i + 2] = uV[i + 3];
                        uV[i + 3] = zero;
                    }
                    if (options._flip.IsFlagSet(UISpriteFlip.FlipVertical))
                    {
                        zero = uV[i];
                        uV[i] = uV[i + 3];
                        uV[i + 3] = zero;
                        zero = uV[i + 1];
                        uV[i + 1] = uV[i + 2];
                        uV[i + 2] = zero;
                    }
                }
                if (options._flip.IsFlagSet(UISpriteFlip.FlipHorizontal))
                {
                    Vector2[] array = new Vector2[uV.Length];
                    Array.Copy(uV, array, uV.Length);
                    Array.Copy(uV, 0, uV, 4, 4);
                    Array.Copy(array, 4, uV, 0, 4);
                    Array.Copy(uV, 8, uV, 12, 4);
                    Array.Copy(array, 12, uV, 8, 4);
                }
                if (options._flip.IsFlagSet(UISpriteFlip.FlipVertical))
                {
                    Vector2[] array2 = new Vector2[uV.Length];
                    Array.Copy(uV, array2, uV.Length);
                    Array.Copy(uV, 0, uV, 8, 4);
                    Array.Copy(array2, 8, uV, 0, 4);
                    Array.Copy(uV, 4, uV, 12, 4);
                    Array.Copy(array2, 12, uV, 4, 4);
                }
            }
            for (int j = 0; j < uV.Length; j++)
            {
                renderData.uvs.Add(uV[j]);
            }
        }

        private static void RebuildSlicedColors(UIRenderData renderData, RenderOptions options)
        {
            Color linear = ((Color)options._color).linear;
            for (int i = 0; i < 16; i++)
            {
                renderData.colors.Add(linear);
            }
        }

        private static void DoSlicedFill(UIRenderData renderData, RenderOptions options)
        {
            int baseIndex = options._baseIndex;
            PoolList<Vector3> vertices = renderData.vertices;
            PoolList<Vector2> uvs = renderData.uvs;
            int[][] fillIndices = GetFillIndices(options._fillDirection, baseIndex);
            bool invertFill = options._invertFill;
            if (options._invertFill)
            {
                for (int i = 0; i < fillIndices.Length; i++)
                {
                    Array.Reverse(fillIndices[i]);
                }
            }
            int index = options._fillDirection != UIFillDirection.Horizontal ? 1 : 0;
            float num = vertices[fillIndices[0][invertFill ? 3 : 0]][index];
            float num2 = vertices[fillIndices[0][!invertFill ? 3 : 0]][index];
            float num3 = Mathf.Abs(num2 - num);
            float num4 = invertFill ? num2 - options._fillAmount * num3 : num + options._fillAmount * num3;
            for (int j = 0; j < fillIndices.Length; j++)
            {
                if (!invertFill)
                {
                    for (int num5 = 3; num5 > 0; num5--)
                    {
                        float num6 = vertices[fillIndices[j][num5]][index];
                        if (num6 >= num4)
                        {
                            Vector3 value = vertices[fillIndices[j][num5]];
                            value[index] = num4;
                            vertices[fillIndices[j][num5]] = value;
                            float num7 = vertices[fillIndices[j][num5 - 1]][index];
                            if (num7 <= num4)
                            {
                                float num8 = num6 - num7;
                                float t = (num4 - num7) / num8;
                                float b = uvs[fillIndices[j][num5]][index];
                                float a = uvs[fillIndices[j][num5 - 1]][index];
                                Vector2 value2 = uvs[fillIndices[j][num5]];
                                value2[index] = Mathf.Lerp(a, b, t);
                                uvs[fillIndices[j][num5]] = value2;
                            }
                        }
                    }
                    continue;
                }
                for (int k = 1; k < 4; k++)
                {
                    float num9 = vertices[fillIndices[j][k]][index];
                    if (num9 <= num4)
                    {
                        Vector3 value3 = vertices[fillIndices[j][k]];
                        value3[index] = num4;
                        vertices[fillIndices[j][k]] = value3;
                        float num10 = vertices[fillIndices[j][k - 1]][index];
                        if (num10 >= num4)
                        {
                            float num11 = num9 - num10;
                            float t2 = (num4 - num10) / num11;
                            float b2 = uvs[fillIndices[j][k]][index];
                            float a2 = uvs[fillIndices[j][k - 1]][index];
                            Vector2 value4 = uvs[fillIndices[j][k]];
                            value4[index] = Mathf.Lerp(a2, b2, t2);
                            uvs[fillIndices[j][k]] = value4;
                        }
                    }
                }
            }
        }

        private static int[][] GetFillIndices(UIFillDirection fillDirection, int baseIndex)
        {
            int[][] array = fillDirection == UIFillDirection.Horizontal ? kHorzFill : kVertFill;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    kFillIndices[i][j] = baseIndex + array[i][j];
                }
            }
            return kFillIndices;
        }

        public static void RenderSprite(UIRenderData data, RenderOptions options)
        {
            options._baseIndex = data.vertices.Count;
            RebuildTriangles(data, options);
            RebuildVertices(data, options);
            RebuildUV(data, options);
            RebuildColors(data, options);
            if (options._fillAmount < 1f)
            {
                DoFill(data, options);
            }
        }

        private static void RebuildTriangles(UIRenderData renderData, RenderOptions options)
        {
            int baseIndex = options._baseIndex;
            PoolList<int> triangles = renderData.triangles;
            triangles.EnsureCapacity(triangles.Count + kTriangleIndices.Length);
            for (int i = 0; i < kTriangleIndices.Length; i++)
            {
                triangles.Add(baseIndex + kTriangleIndices[i]);
            }
        }

        private static void RebuildVertices(UIRenderData renderData, RenderOptions options)
        {
            PoolList<Vector3> vertices = renderData.vertices;
            int baseIndex = options._baseIndex;
            float x = 0f;
            float y = 0f;
            float x2 = Mathf.Ceil(options._size.x);
            float y2 = Mathf.Ceil(0f - options._size.y);
            vertices.Add(new Vector3(x, y, 0f) * options._pixelsToUnits);
            vertices.Add(new Vector3(x2, y, 0f) * options._pixelsToUnits);
            vertices.Add(new Vector3(x2, y2, 0f) * options._pixelsToUnits);
            vertices.Add(new Vector3(x, y2, 0f) * options._pixelsToUnits);
            Vector3 vector = options._offset.RoundToInt() * options._pixelsToUnits;
            for (int i = 0; i < 4; i++)
            {
                vertices[baseIndex + i] = (vertices[baseIndex + i] + vector).Quantize(options._pixelsToUnits);
            }
        }

        private static void RebuildUV(UIRenderData renderData, RenderOptions options)
        {
            Rect region = options._spriteInfo.region;
            PoolList<Vector2> uvs = renderData.uvs;
            uvs.Add(new Vector2(region.x, region.yMax));
            uvs.Add(new Vector2(region.xMax, region.yMax));
            uvs.Add(new Vector2(region.xMax, region.y));
            uvs.Add(new Vector2(region.x, region.y));
            Vector2 zero;
            if (options._flip.IsFlagSet(UISpriteFlip.FlipHorizontal))
            {
                zero = uvs[1];
                uvs[1] = uvs[0];
                uvs[0] = zero;
                zero = uvs[3];
                uvs[3] = uvs[2];
                uvs[2] = zero;
            }
            if (options._flip.IsFlagSet(UISpriteFlip.FlipVertical))
            {
                zero = uvs[0];
                uvs[0] = uvs[3];
                uvs[3] = zero;
                zero = uvs[1];
                uvs[1] = uvs[2];
                uvs[2] = zero;
            }
        }

        private static void RebuildColors(UIRenderData renderData, RenderOptions options)
        {
            Color linear = ((Color)options._color).linear;
            PoolList<Color32> colors = renderData.colors;
            for (int i = 0; i < 4; i++)
            {
                colors.Add(linear);
            }
        }

        private static void DoFill(UIRenderData renderData, RenderOptions options)
        {
            int baseIndex = options._baseIndex;
            PoolList<Vector3> vertices = renderData.vertices;
            PoolList<Vector2> uvs = renderData.uvs;
            int index = baseIndex + 3;
            int index2 = baseIndex + 2;
            int index3 = baseIndex;
            int index4 = baseIndex + 1;
            if (options._invertFill)
            {
                if (options._fillDirection == UIFillDirection.Horizontal)
                {
                    index = baseIndex + 1;
                    index2 = baseIndex;
                    index3 = baseIndex + 2;
                    index4 = baseIndex + 3;
                }
                else
                {
                    index = baseIndex;
                    index2 = baseIndex + 1;
                    index3 = baseIndex + 3;
                    index4 = baseIndex + 2;
                }
            }
            if (options._fillDirection == UIFillDirection.Horizontal)
            {
                vertices[index2] = Vector3.Lerp(vertices[index2], vertices[index], 1f - options._fillAmount);
                vertices[index4] = Vector3.Lerp(vertices[index4], vertices[index3], 1f - options._fillAmount);
                uvs[index2] = Vector2.Lerp(uvs[index2], uvs[index], 1f - options._fillAmount);
                uvs[index4] = Vector2.Lerp(uvs[index4], uvs[index3], 1f - options._fillAmount);
            }
            else
            {
                vertices[index3] = Vector3.Lerp(vertices[index3], vertices[index], 1f - options._fillAmount);
                vertices[index4] = Vector3.Lerp(vertices[index4], vertices[index2], 1f - options._fillAmount);
                uvs[index3] = Vector2.Lerp(uvs[index3], uvs[index], 1f - options._fillAmount);
                uvs[index4] = Vector2.Lerp(uvs[index4], uvs[index2], 1f - options._fillAmount);
            }
        }
    }
}
