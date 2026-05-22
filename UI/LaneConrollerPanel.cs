using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using LaneController.KianCommons.Utils;
using LaneController.UI.Editors;
using UnityEngine;

namespace LaneController.UI
{
    public class LaneControllerPanel : UIPanel
    {
        public static LaneControllerPanel Instance { get; private set; }

        private PanelHeader Header { get; set; }

        private CustomUITabstrip TabStrip { get; set; }

        private UIPanel SizeChanger { get; set; }

        public List<BaseEditor> Editors { get; } = [];

        public BaseEditor CurrentEditor { get; set; }

        private Vector2 EditorSize => base.size - new Vector2(0f, HeaderHeight + TabStrip.height);

        private Vector2 EditorPosition => new(0f, TabStrip.relativePosition.y + TabStrip.height);

        private static float TabStripHeight => 20f;

        private static float HeaderHeight => 42f;

        public static UITextureAtlas ResizeAtlas { get; } = GetResizeIcon();

        private static UITextureAtlas GetResizeIcon()
        {
            return TextureUtil.GetAtlasOrNull("ResizeAtlas") ?? TextureUtil.CreateTextureAtlas("resize.png", "ResizeAtlas", 9, 9, ["resize"], null, 2);
        }

        public static LaneControllerPanel CreatePanel()
        {
            UIView aView = UIView.GetAView();
            Instance = aView.AddUIComponent(typeof(LaneControllerPanel)) as LaneControllerPanel;
            Instance.Init();
            return Instance;
        }

        public static void RemovePanel()
        {
            if (Instance != null)
            {
                Instance.Hide();
                UnityEngine.Object.Destroy(Instance);
                Instance = null;
            }
        }

        public void Init()
        {
            base.atlas = TextureUtil.InGameAtlas;
            base.backgroundSprite = "MenuPanel2";
            base.absolutePosition = new Vector3(100f, 100f);
            base.name = "LaneManagerPanel";
            CreateHeader();
            CreateTabStrip();
            CreateEditors();
            CreateSizeChanger();
            base.size = new Vector2(550f, HeaderHeight + TabStrip.height + 400f);
            base.minimumSize = new Vector2(500f, HeaderHeight + TabStrip.height + 200f);
        }

        public void UpdatePanel()
        {
            CurrentEditor?.UpdateEditor();
        }

        public void Render(RenderManager.CameraInfo cameraInfo)
        {
            CurrentEditor?.Render(cameraInfo);
        }

        private void CreateHeader()
        {
            Header = AddUIComponent<PanelHeader>();
            Header.relativePosition = new Vector2(0f, 0f);
            Header.Target = base.parent;
            Header.Init(HeaderHeight);
        }

        private void CreateTabStrip()
        {
            TabStrip = AddUIComponent<CustomUITabstrip>();
            TabStrip.relativePosition = new Vector3(0f, HeaderHeight);
            TabStrip.eventSelectedIndexChanged += TabStripSelectedIndexChanged;
            TabStrip.selectedIndex = -1;
        }

        private void CreateEditors()
        {
            Log.Called();
            CreateEditor<LaneEditor>();
        }

        private void CreateEditor<EditorType>() where EditorType : BaseEditor
        {
            Log.Called();
            EditorType val = AddUIComponent<EditorType>();
            val.Init(this);
            TabStrip.AddTab(val.Name);
            val.isVisible = false;
            val.size = EditorSize;
            val.relativePosition = EditorPosition;
            Editors.Add(val);
        }

        public void SetSegment(ushort segmentId)
        {
            Log.Called(segmentId);
            Show();
            Header.Text = $"Segment #{segmentId}";
            TabStrip.selectedIndex = -1;
            SelectEditor<LaneEditor>();
        }

        private void TabStripSelectedIndexChanged(UIComponent component, int index)
        {
            Log.Called(component, index);
            CurrentEditor = SelectEditor(index);
            UpdatePanel();
        }

        private void CreateSizeChanger()
        {
            Log.Called(base.size);
            SizeChanger = AddUIComponent<UIPanel>();
            SizeChanger.size = new Vector2(9f, 9f);
            SizeChanger.atlas = ResizeAtlas;
            SizeChanger.backgroundSprite = "resize";
            SizeChanger.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 160);
            SizeChanger.eventPositionChanged += SizeChangerPositionChanged;
            UIDragHandle uIDragHandle = SizeChanger.AddUIComponent<UIDragHandle>();
            uIDragHandle.size = SizeChanger.size;
            uIDragHandle.relativePosition = Vector2.zero;
            uIDragHandle.target = SizeChanger;
        }

        private void SizeChangerPositionChanged(UIComponent component, Vector2 value)
        {
            Log.Called(value);
            base.size = (Vector2)SizeChanger.relativePosition + SizeChanger.size;
            SizeChanger.relativePosition = base.size - SizeChanger.size;
        }

        protected override void OnSizeChanged()
        {
            Log.Called(base.size);
            base.OnSizeChanged();
            if (CurrentEditor != null)
            {
                CurrentEditor.size = EditorSize;
            }
            if (Header != null)
            {
                Header.size = new Vector2(base.size.x, HeaderHeight);
            }
            if (SizeChanger != null)
            {
                SizeChanger.relativePosition = base.size - SizeChanger.size;
            }
        }

        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            if (base.isVisible)
            {
                UpdatePanel();
                OnSizeChanged();
            }
        }

        private BaseEditor SelectEditor(int index)
        {
            Log.Called(index);
            if (index >= 0 && Editors.Count > index)
            {
                foreach (BaseEditor editor in Editors)
                {
                    editor.isVisible = false;
                }
                Editors[index].isVisible = true;
                return Editors[index];
            }
            return null;
        }

        private EditorType SelectEditor<EditorType>() where EditorType : BaseEditor
        {
            Log.Called();
            int editor = GetEditor(typeof(EditorType));
            TabStrip.selectedIndex = editor;
            return Editors[editor] as EditorType;
        }

        private int GetEditor(Type editorType)
        {
            return Editors.FindIndex((BaseEditor e) => e.GetType() == editorType);
        }
    }
}
