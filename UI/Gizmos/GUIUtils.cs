using UnityEngine;

namespace LaneController.UI.Gizmos
{
    public static class GUIUtils
    {
        public static Vector2 MousePos => new(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);

        public static Rect ClampRectToScreen(Rect source)
        {
            Rect result = new(source);
            if (result.width > (float)Screen.width || result.height > (float)Screen.height)
            {
                return result;
            }
            if (result.x < 0f)
            {
                result.x = 0f;
            }
            if (result.y < 0f)
            {
                result.y = 0f;
            }
            if (result.x + result.width > (float)Screen.width)
            {
                result.x = (float)Screen.width - result.width;
            }
            if (result.y + result.height > (float)Screen.height)
            {
                result.y = (float)Screen.height - result.height;
            }
            return result;
        }

        public static Rect RectFromCorners(Vector2 topLeftCorner, Vector2 bottomRightCorner, bool fixInversed)
        {
            if (fixInversed)
            {
                if (bottomRightCorner.x >= topLeftCorner.x && bottomRightCorner.y >= topLeftCorner.y)
                {
                    return new Rect(topLeftCorner, new Vector2(bottomRightCorner.x - topLeftCorner.x, bottomRightCorner.y - topLeftCorner.y));
                }
                if (bottomRightCorner.x < topLeftCorner.x && bottomRightCorner.y > topLeftCorner.y)
                {
                    return new Rect(bottomRightCorner.x, topLeftCorner.y, topLeftCorner.x - bottomRightCorner.x, bottomRightCorner.y - topLeftCorner.y);
                }
                if (bottomRightCorner.x < topLeftCorner.x && bottomRightCorner.y < topLeftCorner.y)
                {
                    return new Rect(bottomRightCorner.x, bottomRightCorner.y, topLeftCorner.x - bottomRightCorner.x, topLeftCorner.y - bottomRightCorner.y);
                }
                if (bottomRightCorner.x > topLeftCorner.x && bottomRightCorner.y < topLeftCorner.y)
                {
                    return new Rect(topLeftCorner.x, bottomRightCorner.y, bottomRightCorner.x - topLeftCorner.x, topLeftCorner.y - bottomRightCorner.y);
                }
            }
            return new Rect(topLeftCorner, new Vector2(bottomRightCorner.x - topLeftCorner.x, bottomRightCorner.y - topLeftCorner.y));
        }

        public static bool IsMouseInside(this Rect rect)
        {
            return rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y));
        }

        public static Vector2 RightMostPosition(params Vector2[] positions)
        {
            Vector2 result = positions[0];
            for (int i = 1; i < positions.Length; i++)
            {
                if (result.x < positions[i].x)
                {
                    result = positions[i];
                }
            }
            return result;
        }

        public static Vector2 WorldToGuiPoint(this Vector3 position)
        {
            Vector3 vector = Camera.main.WorldToScreenPoint(position);
            vector.y = (float)Screen.height - vector.y;
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2 WorldToGuiPoint(this Vector3 position, Camera cam)
        {
            Vector3 vector = cam.WorldToScreenPoint(position);
            vector.y = (float)Screen.height - vector.y;
            return new Vector2(vector.x, vector.y);
        }
    }
}
