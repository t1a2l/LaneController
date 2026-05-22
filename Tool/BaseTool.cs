using LaneController.UI;
using UnityEngine;

namespace LaneController.Tool
{
    public abstract class BaseTool
    {
        public abstract ToolType Type { get; }

        public virtual bool ShowPanel => true;

        protected LaneControllerTool Tool => LaneControllerTool.Instance;

        protected LaneControllerPanel Panel => LaneControllerPanel.Instance;

        public virtual void Init()
        {
            Reset();
        }

        public virtual void DeInit()
        {
        }

        protected virtual void Reset()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnGUI(Event e)
        {
        }

        public virtual void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
        }

        public virtual void OnMouseDown(Event e)
        {
        }

        public virtual void OnMouseDrag(Event e)
        {
        }

        public virtual void OnKeyUp(Event e)
        {
        }

        public virtual void OnMouseUp(Event e)
        {
            OnPrimaryMouseClicked(e);
        }

        public virtual void OnPrimaryMouseClicked(Event e)
        {
        }

        public virtual string OnToolInfo()
        {
            return null;
        }

        public virtual void OnSecondaryMouseClicked()
        {
            if (Type >= ToolType.SelectLane)
            {
                Tool.SetMode(Type - 1);
            }
            else
            {
                Tool.enabled = false;
            }
        }

        public virtual void SimulationStep()
        {
        }
    }
}
