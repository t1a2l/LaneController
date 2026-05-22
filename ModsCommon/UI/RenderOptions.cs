using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.UI
{
    public struct RenderOptions
    {
        public UITextureAtlas _atlas;

        public UITextureAtlas.SpriteInfo _spriteInfo;

        public Color32 _color;

        public float _pixelsToUnits;

        public Vector2 _size;

        public UISpriteFlip _flip;

        public bool _invertFill;

        public UIFillDirection _fillDirection;

        public float _fillAmount;

        public Vector3 _offset;

        public int _baseIndex;
    }
}
