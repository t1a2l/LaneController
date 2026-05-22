using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ColossalFramework.UI;
using LaneController.KianCommons.Plugins;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.KianCommons.UI
{
    internal static class TextureUtil
    {
        private static UITextureAtlas inGame_;

        private static UITextureAtlas inMapEditor_;

        public static string FILE_PATH = ModPath;

        public static bool EmbededResources = true;

        public static UITextureAtlas InGameAtlas
        {
            get
            {
                if (!inGame_)
                {
                    inGame_ = GetAtlasOrNull("Ingame") ?? UIView.GetAView().defaultAtlas;
                }
                return inGame_;
            }
        }

        public static UITextureAtlas InMapEditorAtlas
        {
            get
            {
                if (!inMapEditor_)
                {
                    inMapEditor_ = GetAtlasOrNull("InMapEditor") ?? UIView.GetAView().defaultAtlas;
                }
                return inMapEditor_;
            }
        }

        private static string PATH => typeof(TextureUtil).Assembly.GetName().Name + ".Resources.";

        private static string ModPath => PluginUtil.GetPlugin().modPath;

        public static UITextureAtlas CreateTextureAtlas(string textureFile, string atlasName, int spriteWidth, int spriteHeight, string[] spriteNames, RectOffset border = null, int space = 0)
        {
            Texture2D texture2D = LoadTextureFromAssembly(textureFile, spriteWidth * spriteNames.Length + space * (spriteNames.Length + 1), spriteHeight + 2 * space);
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            float height = spriteHeight / (float)texture2D.height;
            float num = spriteWidth / (float)texture2D.width;
            float y = space / (float)texture2D.height;
            float num2 = space / (float)texture2D.width;
            for (int i = 0; i < spriteNames.Length; i++)
            {
                UITextureAtlas.SpriteInfo item = new()
                {
                    name = spriteNames[i],
                    texture = texture2D,
                    region = new Rect(i * num + (i + 1) * num2, y, num, height),
                    border = border ?? new RectOffset()
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static UITextureAtlas CreateTextureAtlas(string textureFile, string atlasName, string[] spriteNames)
        {
            Texture2D texture2D = EmbededResources ? GetTextureFromAssemblyManifest(textureFile) : GetTextureFromFile(textureFile);
            return CreateTextureAtlas(texture2D, atlasName, spriteNames);
        }

        public static UITextureAtlas CreateTextureAtlas(Texture2D texture2D, string atlasName, string[] spriteNames)
        {
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Assertion.Assert(uITextureAtlas, "atlas");
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            Assertion.Assert(material, "material");
            material.mainTexture = texture2D.TryMakeReadable();
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            int num = spriteNames.Length;
            for (int i = 0; i < num; i++)
            {
                float num2 = 1f / spriteNames.Length;
                UITextureAtlas.SpriteInfo item = new()
                {
                    name = spriteNames[i],
                    texture = texture2D,
                    region = new Rect(i * num2, 0f, num2, 1f)
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static UITextureAtlas CreateTextureAtlas(string atlasName, string[] spriteNames)
        {
            Texture2D[] textures = [.. spriteNames.Select(GetTexture)];
            return CreateTextureAtlas(atlasName, textures);
            static Texture2D GetTexture(string _spriteName)
            {
                Texture2D textureFromAssemblyManifest = GetTextureFromAssemblyManifest(_spriteName + ".png");
                textureFromAssemblyManifest.name = _spriteName;
                return textureFromAssemblyManifest;
            }
        }

        public static UITextureAtlas CreateTextureAtlas(string atlasName, Texture2D[] textures)
        {
            Texture2D texture2D = new(2048, 2048, TextureFormat.ARGB32, mipmap: false);
            Rect[] array = texture2D.PackTextures(textures, 2, 2048);
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            for (int i = 0; i < textures.Length; i++)
            {
                UITextureAtlas.SpriteInfo item = new()
                {
                    name = textures[i].name,
                    texture = textures[i],
                    region = array[i]
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static void AddTexturesToAtlas(UITextureAtlas atlas, Texture2D[] newTextures)
        {
            Texture2D[] array = new Texture2D[atlas.count + newTextures.Length];
            for (int i = 0; i < atlas.count; i++)
            {
                Texture2D texture = atlas.sprites[i].texture;
                texture = texture.TryMakeReadable();
                array[i] = texture;
                array[i].name = atlas.sprites[i].name;
            }
            for (int j = 0; j < newTextures.Length; j++)
            {
                array[atlas.count + j] = newTextures[j];
            }
            Rect[] array2 = atlas.texture.PackTextures(array, atlas.padding, 4096, makeNoLongerReadable: false);
            atlas.sprites.Clear();
            for (int k = 0; k < array.Length; k++)
            {
                UITextureAtlas.SpriteInfo spriteInfo = atlas[array[k].name];
                atlas.sprites.Add(new UITextureAtlas.SpriteInfo
                {
                    texture = array[k],
                    name = array[k].name,
                    border = spriteInfo?.border ?? new RectOffset(),
                    region = array2[k]
                });
            }
            atlas.RebuildIndexes();
        }

        public static UITextureAtlas GetAtlasOrNull(string name)
        {
            UITextureAtlas[] source = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            return source.FirstOrDefault((atlas) => atlas.name == name);
        }

        public static Texture2D LoadTextureFromAssembly(string textureFile, int width, int height)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = PATH + textureFile;
            Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text);
            Assertion.NotNull(manifestResourceStream, "could not find " + text);
            byte[] array = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(array, 0, array.Length);
            Texture2D texture2D = new(width, height, TextureFormat.ARGB32, mipmap: false);
            Assertion.Assert(texture2D, "texture2D");
            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.LoadImage(array);
            texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
            return texture2D;
        }

        public static Stream GetFileStream(string file)
        {
            try
            {
                string text = Path.Combine(FILE_PATH, file);
                return File.OpenRead(text) ?? throw new Exception(text + "not find");
            }
            catch (Exception ex)
            {
                ex.Exception();
                throw ex;
            }
        }

        public static Texture2D GetTextureFromFile(string file)
        {
            using Stream stream = GetFileStream(file);
            return GetTextureFromStream(stream);
        }

        public static Stream GetManifestResourceStream(string file)
        {
            try
            {
                string text = PATH + file;
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(text) ?? throw new Exception(text + " not find");
            }
            catch (Exception ex)
            {
                ex.Exception();
                throw ex;
            }
        }

        public static Texture2D GetTextureFromAssemblyManifest(string file)
        {
            using Stream stream = GetManifestResourceStream(file);
            return GetTextureFromStream(stream);
        }

        public static Texture2D GetTextureFromStream(Stream stream)
        {
            Texture2D texture2D = new(1, 1, TextureFormat.ARGB32, mipmap: false);
            texture2D.LoadImage(stream.ReadAllBytes());
            texture2D.wrapMode = TextureWrapMode.Clamp;
            texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
            return texture2D;
        }

        private static byte[] ReadAllBytes(this Stream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }
    }
}
