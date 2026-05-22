using System;
using ColossalFramework.UI;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.UI.Gizmos
{
    public class YGizmo
    {
        public struct GizmoAxisT
        {
            public GameObject Gizmo;

            public GameObject Head;

            public LineRenderer Renderer;

            public readonly BoxCollider Collider => Gizmo?.GetComponent<BoxCollider>();
        }

        public static float opacity = 1f;

        public static Color GizmoRed = new(1f, 0f, 0f, opacity);

        public static Color GizmoBlue = new(0f, 0f, 1f, opacity);

        public static Color GizmoGreen = new(0f, 1f, 0f, opacity);

        public static Color GizmoYellow = new(1f, 0.92f, 0.016f, opacity);

        public static float GizmoSize = 0.5f;

        public static Camera Cam;

        public GameObject CenterCube;

        public GizmoAxisT GizmoAxis;

        public static Shader shader = Shader.Find("GUI/Text Shader");

        public bool AxisClicked;

        private Vector3 HitPos0;

        private Vector3 Origin0;

        public KeyTyping KeyTyping;

        public Vector3 Origin => GizmoAxis.Gizmo.transform.position;

        public float Distance => Origin.y - Origin0.y;

        public bool IsVisible
        {
            get
            {
                return GizmoAxis.Renderer?.enabled ?? false;
            }
            set
            {
                LineRenderer renderer = GizmoAxis.Renderer;
                if (renderer is not null && (bool)renderer)
                {
                    renderer.enabled = value;
                }
                BoxCollider collider = GizmoAxis.Collider;
                if (collider is not null && (bool)collider)
                {
                    collider.enabled = value;
                }
                MeshRenderer meshRenderer = CenterCube?.GetComponent<MeshRenderer>();
                if (meshRenderer is not null && (bool)meshRenderer)
                {
                    meshRenderer.enabled = value;
                }
                MeshRenderer meshRenderer2 = GizmoAxis.Head?.GetComponent<MeshRenderer>();
                if (meshRenderer2 is not null && (bool)meshRenderer2)
                {
                    meshRenderer2.enabled = value;
                }
            }
        }

        public YGizmo()
        {
            Cam = Camera.main;
        }

        ~YGizmo()
        {
            Destroy();
        }

        public void Destroy()
        {
            if ((bool)GizmoAxis.Gizmo)
            {
                UnityEngine.Object.Destroy(GizmoAxis.Gizmo);
            }
            if ((bool)CenterCube)
            {
                UnityEngine.Object.Destroy(CenterCube);
            }
            if ((bool)GizmoAxis.Head)
            {
                UnityEngine.Object.Destroy(GizmoAxis.Head);
            }
        }

        public static YGizmo CreatePositionGizmo(Vector3 position)
        {
            YGizmo yGizmo = new();
            yGizmo.Init(position);
            return yGizmo;
        }

        private void Init(Vector3 position)
        {
            try
            {
                GameObject gameObject = GizmoAxis.Gizmo = new GameObject("LCAxis_Y");
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(2f, 2f, 2f);
                LineRenderer lineRenderer = GizmoAxis.Renderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(shader);
                lineRenderer.startColor = GizmoGreen;
                lineRenderer.endColor = GizmoGreen;
                lineRenderer.widthMultiplier = 1f;
                Vector3[] array = new Vector3[2];
                array[1] = array[0] = position;
                array[1][1] += 20f;
                lineRenderer.SetPositions(array);
                gameObject.transform.position = position;
                gameObject.transform.localScale = new Vector3(0.5f, 20f, 0.5f);
                Material material = new(shader)
                {
                    color = GizmoYellow
                };
                CenterCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                CenterCube.transform.position = position;
                CenterCube.GetComponent<MeshRenderer>().material = material;
                UnityEngine.Object.Destroy(CenterCube.GetComponent<MeshCollider>());
                Material material2 = new(shader)
                {
                    color = GizmoGreen
                };
                (GizmoAxis.Head = Cone.Create(material2)).transform.position = array[1];
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public static Plane GetCollisionPlane(Vector3 axisHitPoint)
        {
            Vector3 forward = Cam.transform.forward;
            Vector3 normalized = new Vector3(forward.x, 0f, forward.z).normalized;
            return new Plane(normalized, axisHitPoint);
        }

        public void UpdatePosition(Vector3 position)
        {
            try
            {
                float magnitude = (Cam.transform.position - position).magnitude;
                float num = ((0.0070455f * magnitude) + 0.0386363f) * GizmoSize;
                float y = 20f * num;
                float num2 = 0.5f * num;
                Vector3 vector = position + new Vector3(0f, y, 0f);
                Vector3 localScale = new(num2, y, num2);
                Vector3 localScale2 = Vector3.one * num;
                GameObject gizmo = GizmoAxis.Gizmo;
                if (gizmo is not null && (bool)gizmo)
                {
                    gizmo.transform.position = position;
                    gizmo.transform.localScale = localScale;
                }
                if (CenterCube != null)
                {
                    CenterCube.transform.position = position;
                    CenterCube.transform.localScale = localScale2;
                }
                GameObject head = GizmoAxis.Head;
                if (head is not null && (bool)head)
                {
                    head.transform.position = vector;
                    head.transform.localScale = localScale2;
                }
                LineRenderer renderer = GizmoAxis.Renderer;
                if (renderer is not null && (bool)renderer)
                {
                    renderer.widthMultiplier = num;
                    renderer.SetPositions([position, vector]);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public void OnUpdate()
        {
            KeyTyping?.Register();
        }

        public bool GetAxisHitPoint()
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo) && hitInfo.transform.gameObject == GizmoAxis.Gizmo)
            {
                if (GetCollisionPlane(Origin).Raycast(ray, out var enter))
                {
                    HitPos0 = ray.GetPoint(enter);
                }
                else
                {
                    HitPos0 = hitInfo.point;
                }
                return true;
            }
            HitPos0 = default;
            return false;
        }

        public bool Movement(out Vector3 newPosition)
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            Plane collisionPlane = GetCollisionPlane(HitPos0);
            newPosition = Origin0;
            if (collisionPlane.Raycast(ray, out var enter))
            {
                float t = Helpers.AltIsPressed ? 0.2f : 1f;
                Vector3 vector = Vector3.Lerp(HitPos0, ray.GetPoint(enter), t);
                if (KeyTyping != null && KeyTyping.registeredFloat != 0f)
                {
                    newPosition += new Vector3(0f, KeyTyping.registeredFloat);
                }
                else
                {
                    newPosition.y += vector.y - HitPos0.y;
                }
            }
            return (double)(newPosition - Origin).sqrMagnitude > 0.001;
        }

        public bool Drag()
        {
            try
            {
                if (UIView.HasModalInput() || UIView.HasInputFocus())
                {
                    return false;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (GetAxisHitPoint())
                    {
                        Origin0 = Origin;
                        AxisClicked = true;
                        KeyTyping = new KeyTyping();
                    }
                }
                else if (AxisClicked)
                {
                    if (!Input.GetMouseButton(0))
                    {
                        AxisClicked = false;
                        KeyTyping = null;
                        return true;
                    }
                    if (Movement(out var newPosition))
                    {
                        UpdatePosition(newPosition);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
            return false;
        }
    }
}
