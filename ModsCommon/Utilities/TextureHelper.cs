using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ColossalFramework.Importers;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.ModsCommon.Utilities
{
    public static class TextureHelper
    {
        public delegate UITextureAtlas.SpriteInfo[] SpriteParamsGetter(int texWidth, int texHeight, Rect rect);

        public static UITextureAtlas InGameAtlas { get; } = GetAtlas("Ingame");

        public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].name == name)
                {
                    return array[i];
                }
            }
            return UIView.GetAView().defaultAtlas;
        }

        public static UITextureAtlas CreateAtlas(string atlasName, Dictionary<string, RectOffset> spriteParams)
        {
            return CreateAtlas(Assembly.GetExecutingAssembly(), atlasName, spriteParams);
        }

        public static UITextureAtlas CreateAtlas(Assembly assembly, string atlasName, Dictionary<string, RectOffset> spriteParams)
        {
            Texture2D[] array = assembly.LoadAllTexturesFromAssembly((n) => spriteParams.ContainsKey(n));
            UITextureAtlas uITextureAtlas = CreateAtlas(array, atlasName, out Rect[] rects);
            for (int num = 0; num < array.Length; num++)
            {
                UITextureAtlas.SpriteInfo item = new()
                {
                    name = array[num].name,
                    texture = array[num],
                    region = rects[num],
                    border = spriteParams[array[num].name]
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static UITextureAtlas CreateAtlas(string atlasName, Dictionary<string, SpriteParamsGetter> files)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Texture2D[] array = [.. files.Select((f) => assembly.LoadTextureFromAssembly(f.Key))];
            UITextureAtlas uITextureAtlas = CreateAtlas(array, atlasName, out Rect[] rects);
            SpriteParamsGetter[] array2 = [.. files.Values];
            for (int num = 0; num < array2.Length; num++)
            {
                UITextureAtlas.SpriteInfo[] array3 = array2[num](array[num].width, array[num].height, rects[num]);
                foreach (UITextureAtlas.SpriteInfo spriteInfo in array3)
                {
                    spriteInfo.texture = array[num];
                    uITextureAtlas.AddSprite(spriteInfo);
                }
            }
            return uITextureAtlas;
        }

        private static UITextureAtlas CreateAtlas(Texture2D[] textures, string name, out Rect[] rects)
        {
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            uITextureAtlas.material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            uITextureAtlas.material.mainTexture = CreateTexture(1, 1, Color.white);
            uITextureAtlas.name = name;
            uITextureAtlas.padding = 5;
            rects = uITextureAtlas.texture.PackTextures(textures, uITextureAtlas.padding, 4096, makeNoLongerReadable: false);
            return uITextureAtlas;
        }

        public static Texture2D[] LoadAllTexturesFromAssembly(this Assembly assembly, Func<string, bool> filter = null)
        {
            List<Texture2D> list = [];
            string[] manifestResourceNames = assembly.GetManifestResourceNames();
            foreach (string text in manifestResourceNames)
            {
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }
                string extension = Path.GetExtension(text);
                if (!(extension != ".png") || !(extension != ".jpg"))
                {
                    string text2 = Path.GetFileNameWithoutExtension(text).Split('.').Last();
                    if (filter == null || filter(text2))
                    {
                        Stream manifestResourceStream = assembly.GetManifestResourceStream(text);
                        byte[] array = new byte[manifestResourceStream.Length];
                        manifestResourceStream.Read(array, 0, array.Length);
                        Texture2D texture2D = new Image(array).CreateTexture();
                        texture2D.wrapMode = TextureWrapMode.Clamp;
                        texture2D.name = text2;
                        list.Add(texture2D);
                    }
                }
            }
            return [.. list];
        }

        public static Texture2D LoadTextureFromAssembly(this Assembly assembly, string textureFile)
        {
            string search = "." + textureFile + ".";
            string text = assembly.GetManifestResourceNames().FirstOrDefault((n) => n.Contains(search));
            if (text == null)
            {
                return null;
            }
            Stream manifestResourceStream = assembly.GetManifestResourceStream(text);
            byte[] array = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(array, 0, array.Length);
            Texture2D texture2D = new Image(array).CreateTexture();
            texture2D.name = textureFile;
            return texture2D;
        }

        public static Texture2D CreateTexture(int height, int width, Color color)
        {
            Texture2D texture2D = new(height, width)
            {
                name = "Markup"
            };
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    texture2D.SetPixel(i, j, color);
                }
            }
            texture2D.Apply();
            return texture2D;
        }

        public static IEnumerable<UITextureAtlas.SpriteInfo> GetSpritesInfo(int texWidth, int texHeight, Rect rect, string name)
        {
            return GetSpritesInfo(texWidth, texHeight, rect, new RectOffset(), 0, name);
        }

        public static IEnumerable<UITextureAtlas.SpriteInfo> GetSpritesInfo(int texWidth, int texHeight, Rect rect, RectOffset border, int space, string name)
        {
            return GetSpritesInfo(texWidth, texHeight, rect, texWidth - 2 * space, texHeight - 2 * space, border, space, name);
        }

        public static IEnumerable<UITextureAtlas.SpriteInfo> GetSpritesInfo(int texWidth, int texHeight, Rect rect, int spriteWidth, int spriteHeight, params string[] names)
        {
            return GetSpritesInfo(texWidth, texHeight, rect, spriteWidth, spriteHeight, new RectOffset(), 0, names);
        }

        public static IEnumerable<UITextureAtlas.SpriteInfo> GetSpritesRowsInfo(int texWidth, int texHeight, Rect rect, int spriteWidth, int spriteHeight, RectOffset border, int space, int inRow, params string[] names)
        {
            int rows = names.Length / inRow + (names.Length % inRow != 0 ? 1 : 0);
            float rowHeight = rect.height / texHeight * spriteHeight;
            float spaceHeight = rect.height / texHeight * space;
            for (int i = 0; i < rows; i++)
            {
                Rect rect2 = new(rect.x, rect.y + (spaceHeight + rowHeight) * (rows - i - 1), rect.width, rowHeight + 2f * spaceHeight);
                string[] names2 = [.. names.Skip(inRow * i).Take(inRow)];
                foreach (UITextureAtlas.SpriteInfo item in GetSpritesInfo(texWidth, spriteHeight + 2 * space, rect2, spriteWidth, spriteHeight, border, space, names2))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<UITextureAtlas.SpriteInfo> GetSpritesInfo(int texWidth, int texHeight, Rect rect, int spriteWidth, int spriteHeight, RectOffset border, int space, params string[] names)
        {
            float width = spriteWidth / (float)texWidth * rect.width;
            float height = spriteHeight / (float)texHeight * rect.height;
            float spaceWidth = space / (float)texWidth * rect.width;
            float spaceHeight = space / (float)texHeight * rect.height;
            for (int i = 0; i < names.Length; i++)
            {
                float x = rect.x + i * width + (i + 1) * spaceWidth;
                float y = rect.y + spaceHeight;
                yield return new UITextureAtlas.SpriteInfo
                {
                    name = names[i],
                    region = new Rect(x, y, width, height),
                    border = border
                };
            }
        }
    }
}
