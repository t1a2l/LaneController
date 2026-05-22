using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ColossalFramework.UI;
using LaneController.KianCommons.UI;
using LaneController.Tool;
using UnityEngine;

namespace LaneController.UI.Editors
{
    public abstract class BaseEditor : UIPanel
    {
        public enum LaneType
        {
            [Description("Bicycle")]
            Bicycle,
            [Description("Car")]
            Car,
            [Description("Tram")]
            Tram,
            [Description("Pedestrian")]
            Pedestrian,
            [Description("Cablecar")]
            Cablecar,
            [Description("Monorail")]
            Monorail,
            [Description("Metro")]
            Metro,
            [Description("TrolleyBus")]
            Trolleybus,
            [Description("Train")]
            Train,
            [Description("Parking")]
            Parking,
            [Description("None")]
            None,
            [Description("Multiple")]
            Multiple
        }

        protected LaneControllerTool ToolInstance => LaneControllerTool.Instance;

        public static Dictionary<LaneType, string> LaneSpriteNames { get; set; }

        public static Dictionary<NetInfo.Direction, string> DirectionSpriteNames { get; set; }

        public static UITextureAtlas LaneAtlas { get; } = GetLaneIcons();

        public static UITextureAtlas DirectionAtlas { get; } = GetDirectionIcons();

        public LaneControllerPanel LaneManagerPanel { get; private set; }

        protected UIScrollablePanel ItemsPanel { get; set; }

        protected UIScrollablePanel SettingsPanel { get; set; }

        protected UILabel SelectionLabel { get; set; }

        public abstract string Name { get; }

        public abstract string SelectionMessage { get; }

        private static UITextureAtlas GetLaneIcons()
        {
            LaneSpriteNames = new Dictionary<LaneType, string>
            {
                {
                    LaneType.Bicycle,
                    "Bicycle"
                },
                {
                    LaneType.Car,
                    "Car"
                },
                {
                    LaneType.Tram,
                    "Tram"
                },
                {
                    LaneType.Pedestrian,
                    "Pedestrian"
                },
                {
                    LaneType.Cablecar,
                    "Cablecar"
                },
                {
                    LaneType.Monorail,
                    "Monorail"
                },
                {
                    LaneType.Metro,
                    "Metro"
                },
                {
                    LaneType.Trolleybus,
                    "Trolleybus"
                },
                {
                    LaneType.Train,
                    "Train"
                },
                {
                    LaneType.Parking,
                    "Parking"
                },
                {
                    LaneType.None,
                    "None"
                },
                {
                    LaneType.Multiple,
                    "Multiple"
                }
            };
            return TextureUtil.GetAtlasOrNull("LaneAtlas") ?? TextureUtil.CreateTextureAtlas("laneTypes.png", "LaneAtlas", 64, 64, [.. LaneSpriteNames.Values]);
        }

        private static UITextureAtlas GetDirectionIcons()
        {
            DirectionSpriteNames = new Dictionary<NetInfo.Direction, string>
            {
                {
                    NetInfo.Direction.None,
                    "DirectionNone"
                },
                {
                    NetInfo.Direction.Forward,
                    "DirectionForward"
                },
                {
                    NetInfo.Direction.Backward,
                    "DirectionBackward"
                },
                {
                    NetInfo.Direction.Both,
                    "DirectionBoth"
                },
                {
                    NetInfo.Direction.Avoid,
                    "DirectionAvoid"
                },
                {
                    NetInfo.Direction.AvoidForward,
                    "DirectionAvoidForward"
                },
                {
                    NetInfo.Direction.AvoidBackward,
                    "DirectionAvoidBackward"
                },
                {
                    NetInfo.Direction.AvoidBoth,
                    "DirectionAvoidBoth"
                }
            };
            return TextureUtil.GetAtlasOrNull("DirectionAtlas") ?? TextureUtil.CreateTextureAtlas("directions.png", "DirectionAtlas", 64, 64, [.. DirectionSpriteNames.Values]);
        }

        public BaseEditor()
        {
            clipChildren = true;
            atlas = TextureUtil.InGameAtlas;
            backgroundSprite = "UnlockingItemBackground";
            AddItemsPanel();
            AddSettingPanel();
            AddSelectionLabel();
        }

        private void AddItemsPanel()
        {
            ItemsPanel = AddUIComponent<UIScrollablePanel>();
            ItemsPanel.autoLayout = true;
            ItemsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            ItemsPanel.autoLayoutPadding = new RectOffset(0, 0, 0, 0);
            ItemsPanel.scrollWheelDirection = UIOrientation.Vertical;
            ItemsPanel.builtinKeyNavigation = true;
            ItemsPanel.clipChildren = true;
            ItemsPanel.eventSizeChanged += ItemsPanelSizeChanged;
            ItemsPanel.atlas = TextureUtil.InGameAtlas;
            ItemsPanel.backgroundSprite = "ScrollbarTrack";
            UIUtils.AddScrollbar(this, ItemsPanel);
            ItemsPanel.verticalScrollbar.eventVisibilityChanged += ItemsScrollbarVisibilityChanged;
        }

        private void ItemsPanelSizeChanged(UIComponent component, Vector2 value)
        {
            foreach (UIComponent component2 in ItemsPanel.components)
            {
                component2.width = ItemsPanel.width;
            }
        }

        private void ItemsScrollbarVisibilityChanged(UIComponent component, bool value)
        {
            ItemsPanel.width = size.x / 10f * 3f - (ItemsPanel.verticalScrollbar.isVisible ? ItemsPanel.verticalScrollbar.width : 0f);
            ItemsPanel.verticalScrollbar.relativePosition = ItemsPanel.relativePosition + new Vector3(ItemsPanel.width, 0f);
        }

        private void AddSettingPanel()
        {
            SettingsPanel = AddUIComponent<UIScrollablePanel>();
            SettingsPanel.autoLayout = true;
            SettingsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            SettingsPanel.autoLayoutPadding = new RectOffset(10, 10, 10, 10);
            SettingsPanel.scrollWheelDirection = UIOrientation.Vertical;
            SettingsPanel.builtinKeyNavigation = true;
            SettingsPanel.clipChildren = true;
            SettingsPanel.atlas = TextureUtil.InGameAtlas;
            SettingsPanel.backgroundSprite = "UnlockingItemBackground";
            SettingsPanel.eventSizeChanged += SettingsPanelSizeChanged;
            UIUtils.AddScrollbar(this, SettingsPanel);
            SettingsPanel.verticalScrollbar.eventVisibilityChanged += SettingsScrollbarVisibilityChanged;
        }

        private void SettingsPanelSizeChanged(UIComponent component, Vector2 value)
        {
            foreach (UIComponent component2 in SettingsPanel.components)
            {
                component2.width = SettingsPanel.width - SettingsPanel.autoLayoutPadding.horizontal;
            }
        }

        private void SettingsScrollbarVisibilityChanged(UIComponent component, bool value)
        {
            SettingsPanel.width = size.x / 10f * 7f - (value ? SettingsPanel.verticalScrollbar.width : 0f);
            SettingsPanel.verticalScrollbar.relativePosition = SettingsPanel.relativePosition + new Vector3(SettingsPanel.width, 0f);
        }

        private void AddSelectionLabel()
        {
            SelectionLabel = AddUIComponent<UILabel>();
            SelectionLabel.textAlignment = UIHorizontalAlignment.Center;
            SelectionLabel.verticalAlignment = UIVerticalAlignment.Middle;
            SelectionLabel.padding = new RectOffset(10, 10, 0, 0);
            SelectionLabel.wordWrap = true;
            SelectionLabel.autoSize = false;
            SwitchEmptySelected();
        }

        protected void SwitchEmptySelected()
        {
            if (ItemsPanel.components.Any())
            {
                SelectionLabel.isVisible = false;
                return;
            }
            SelectionLabel.isVisible = true;
            SelectionLabel.text = SelectionMessage;
        }

        protected void ShowEmptySelected()
        {
            SelectionLabel.text = SelectionMessage;
            SelectionLabel.isVisible = true;
        }

        protected void HideEmptySelected()
        {
            SelectionLabel.isVisible = false;
        }

        public virtual void Init(LaneControllerPanel panel)
        {
            LaneManagerPanel = panel;
        }

        public virtual void UpdateEditor()
        {
        }

        public virtual void Render(RenderManager.CameraInfo cameraInfo)
        {
        }

        protected virtual void ClearItems()
        {
        }

        protected virtual void ClearSettings()
        {
        }

        protected virtual void FillItems()
        {
        }

        public virtual void Select(int index)
        {
        }

        protected virtual void Deselect()
        {
        }

        protected abstract void ItemClick(UIComponent component, UIMouseEventParameter eventParam);

        protected abstract void ItemHover(UIComponent component, UIMouseEventParameter eventParam);

        protected abstract void ItemLeave(UIComponent component, UIMouseEventParameter eventParam);
    }
    public abstract class BaseEditor<DynamicButtonType, DynamicObject, ItemIcon> : BaseEditor where DynamicButtonType : DynamicButton<DynamicObject, ItemIcon> where DynamicObject : class where ItemIcon : UIComponent
    {
        private DynamicButtonType _selectItem;

        protected DynamicButtonType HoverItem { get; set; }

        protected bool IsHoverItem => HoverItem != null;

        protected bool IsSelectItem => SelectItem != null;

        protected DynamicButtonType SelectItem
        {
            get
            {
                return _selectItem;
            }
            private set
            {
                if (_selectItem != null)
                {
                    _selectItem.IsSelect = false;
                }
                _selectItem = value;
                if (_selectItem != null)
                {
                    _selectItem.IsSelect = true;
                }
            }
        }

        public DynamicObject EditObject
        {
            get
            {
                DynamicButtonType selectItem = SelectItem;
                if ((object)selectItem == null)
                {
                    return null;
                }
                return selectItem.Object;
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            float x = size.x / 10f * 3f - (ItemsPanel.verticalScrollbar.isVisible ? ItemsPanel.verticalScrollbar.width : 0f);
            ItemsPanel.size = new Vector2(x, size.y);
            ItemsPanel.relativePosition = new Vector2(0f, 0f);
            float x2 = size.x / 10f * 7f - (SettingsPanel.verticalScrollbar.isVisible ? SettingsPanel.verticalScrollbar.width : 0f);
            SettingsPanel.size = new Vector2(x2, size.y);
            SettingsPanel.relativePosition = new Vector2(size.x / 10f * 3f, 0f);
            SelectionLabel.size = new Vector2(size.x / 10f * 7f, size.y / 2f);
            SelectionLabel.relativePosition = SettingsPanel.relativePosition;
        }

        protected virtual DynamicButtonType GetItem(DynamicObject editObject)
        {
            return ItemsPanel.components.OfType<DynamicButtonType>().FirstOrDefault((c) => c.Object == editObject);
        }

        public virtual DynamicButtonType AddItem(DynamicObject editableObject)
        {
            DynamicButtonType val = NewItem();
            InitItem(val, editableObject);
            return val;
        }

        protected DynamicButtonType NewItem()
        {
            DynamicButtonType val = ItemsPanel.AddUIComponent<DynamicButtonType>();
            val.width = ItemsPanel.width;
            return val;
        }

        protected void InitItem(DynamicButtonType item, DynamicObject editableObject)
        {
            item.Text = editableObject.ToString();
            item.Init();
            item.Object = editableObject;
            item.eventClick += ItemClick;
            item.eventMouseEnter += ItemHover;
            item.eventMouseLeave += ItemLeave;
            item.OnDelete += ItemDelete;
        }

        protected void ItemDelete(DynamicButton<DynamicObject, ItemIcon> deleteItem)
        {
            DynamicButtonType item = deleteItem as DynamicButtonType;
            if ((object)item != null)
            {
                Delete();
            }
            bool Delete()
            {
                OnObjectDelete(item.Object);
                bool flag = item == SelectItem;
                DeleteItem(item);
                if (flag)
                {
                    Select(0);
                }
                return true;
            }
        }

        protected virtual void DeleteItem(DynamicButtonType item)
        {
            DeInitItem(item);
            DeleteUIComponent(item);
            SwitchEmptySelected();
        }

        protected void DeInitItem(DynamicButtonType item)
        {
            item.eventClick -= ItemClick;
            item.eventMouseEnter -= ItemHover;
            item.eventMouseLeave -= ItemLeave;
        }

        protected void DeleteUIComponent(UIComponent item)
        {
            ItemsPanel.RemoveUIComponent(item);
            Destroy(item.gameObject);
        }

        protected override void ClearItems()
        {
            UIComponent[] array = [.. ItemsPanel.components];
            UIComponent[] array2 = array;
            foreach (UIComponent item in array2)
            {
                DeleteUIComponent(item);
            }
        }

        public virtual void UpdateEditor(DynamicObject selectObject = null)
        {
            DynamicObject val = EditObject;
            if (selectObject != null && selectObject == val)
            {
                OnObjectUpdate();
                return;
            }
            ClearItems();
            if (ToolInstance.SegmentInstance != null)
            {
                FillItems();
            }
            if (ToolInstance.LaneInstance != null)
            {
                Select(ToolInstance.LaneInstance.Index);
                SwitchEmptySelected();
            }
            else
            {
                Deselect();
                ClearSettings();
            }
            if (!IsSelectItem)
            {
                val = null;
            }
            if (selectObject != null)
            {
                DynamicButtonType item = GetItem(selectObject);
                if ((object)item != null)
                {
                    Select(item);
                    return;
                }
            }
            if (val != null)
            {
                DynamicButtonType item2 = GetItem(val);
                if ((object)item2 != null)
                {
                    SelectItem = item2;
                    ScrollTo(SelectItem);
                    OnObjectUpdate();
                }
            }
        }

        public override void UpdateEditor()
        {
            UpdateEditor();
        }

        protected override void ClearSettings()
        {
            UIComponent[] array = [.. SettingsPanel.components];
            UIComponent[] array2 = array;
            foreach (UIComponent uIComponent in array2)
            {
                if (!(uIComponent == SelectionLabel))
                {
                    SettingsPanel.RemoveUIComponent(uIComponent);
                    Destroy(uIComponent.gameObject);
                }
            }
        }

        protected override void ItemClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            ItemClick((DynamicButtonType)component);
        }

        protected virtual void ItemClick(DynamicButtonType item)
        {
            SettingsPanel.autoLayout = false;
            SelectItem = item;
            OnObjectSelect();
            SettingsPanel.autoLayout = true;
        }

        protected override void ItemHover(UIComponent component, UIMouseEventParameter eventParam)
        {
            HoverItem = component as DynamicButtonType;
        }

        protected override void ItemLeave(UIComponent component, UIMouseEventParameter eventParam)
        {
            HoverItem = null;
        }

        protected virtual void OnObjectSelect()
        {
        }

        protected virtual void OnObjectDelete(DynamicObject editableObject)
        {
        }

        protected virtual void OnObjectUpdate()
        {
        }

        protected override void Deselect()
        {
            if (IsSelectItem)
            {
                SelectItem.Unfocus();
                ShowEmptySelected();
                SelectItem = null;
                ToolInstance.SetLane(-1);
            }
        }

        public override void Select(int index)
        {
            if (ItemsPanel.components.Count > index && ItemsPanel.components[index] is DynamicButtonType item)
            {
                Select(item);
            }
        }

        public virtual void Select(DynamicButtonType item)
        {
            item.SimulateClick();
            item.Focus();
            ScrollTo(item);
        }

        public virtual void ScrollTo(DynamicButtonType item)
        {
            ItemsPanel.ScrollToBottom();
            ItemsPanel.ScrollIntoView(item);
        }

        protected virtual void RefreshItems()
        {
            foreach (DynamicButtonType item in ItemsPanel.components.OfType<DynamicButtonType>())
            {
                item.Refresh();
            }
        }
    }
}
