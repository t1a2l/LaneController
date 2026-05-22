using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public static class ComponentPool
    {
        private static Dictionary<Type, Queue<UIComponent>> Pool { get; } = [];

        private static Dictionary<Type, FieldInfo[]> EventFields { get; } = [];

        public static Component GetAfter<Component>(UIComponent parent, string beforeName, string name = null) where Component : UIComponent, IReusable
        {
            return Get<Component>(parent, beforeName, 1, name);
        }

        public static Component GetBefore<Component>(UIComponent parent, string afterName, string name = null) where Component : UIComponent, IReusable
        {
            return Get<Component>(parent, afterName, 0, name);
        }

        private static Component Get<Component>(UIComponent parent, string otherName, int delta, string name = null) where Component : UIComponent, IReusable
        {
            UIComponent uIComponent = parent.Find(otherName);
            if (uIComponent is not null)
            {
                return Get<Component>(parent, name, uIComponent.zOrder + delta);
            }
            return Get<Component>(parent, name);
        }

        public static Component Get<Component>(UIComponent parent, string name = null, int zOrder = -1) where Component : UIComponent, IReusable
        {
            Queue<UIComponent> queue = GetQueue(typeof(Component));
            Component val;
            if (queue.Count != 0)
            {
                val = queue.Dequeue() as Component;
                parent.AttachUIComponent(val.gameObject);
            }
            else
            {
                val = parent.AddUIComponent<Component>();
            }
            val.InCache = false;
            if (name != null)
            {
                val.cachedName = name;
            }
            if (zOrder != -1)
            {
                val.zOrder = zOrder;
            }
            return val;
        }

        public static void Free<Component>(Component component) where Component : UIComponent
        {
            if (component is IReusable reusable)
            {
                if (!reusable.InCache)
                {
                    component.parent?.RemoveUIComponent(component);
                    component.transform.parent = null;
                    component.cachedName = string.Empty;
                    component.isVisible = true;
                    component.isEnabled = true;
                    reusable.DeInit();
                    Type type = component.GetType();
                    if (!EventFields.TryGetValue(type, out var value))
                    {
                        value = GetFields(type);
                        EventFields[type] = value;
                    }
                    FieldInfo[] array = value;
                    foreach (FieldInfo fieldInfo in array)
                    {
                        fieldInfo.SetValue(component, null);
                    }
                    Queue<UIComponent> queue = GetQueue(type);
                    queue.Enqueue(component);
                    reusable.InCache = true;
                }
            }
            else
            {
                Delete(component);
            }
        }

        private static Queue<UIComponent> GetQueue(Type type)
        {
            if (!Pool.TryGetValue(type, out var value))
            {
                value = new Queue<UIComponent>();
                Pool[type] = value;
            }
            return value;
        }

        private static FieldInfo[] GetFields(Type type)
        {
            if (type == null)
            {
                return [];
            }
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            IEnumerable<FieldInfo> first = from f in type.GetFields(bindingAttr)
                                           where typeof(Delegate).IsAssignableFrom(f.FieldType) && !f.IsDefined(typeof(HideInInspector), inherit: true)
                                           select f;
            first = first.Concat(GetFields(type.BaseType));
            return [.. first];
        }

        public static void Clear()
        {
            foreach (Queue<UIComponent> value in Pool.Values)
            {
                while (value.Any())
                {
                    Delete(value.Dequeue());
                }
            }
            Pool.Clear();
        }

        private static void Delete(UIComponent component)
        {
            if (component != null)
            {
                component.parent?.RemoveUIComponent(component);
                UnityEngine.Object.Destroy(component.gameObject);
                UnityEngine.Object.Destroy(component);
            }
        }
    }
}
