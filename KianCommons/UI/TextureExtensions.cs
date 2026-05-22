using System;
using System.IO;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.KianCommons.UI
{
    internal static class TextureExtensions
    {
        public static Texture2D GetReadableCopy(this Texture2D tex, bool linear = false)
        {
            Assertion.Assert(tex, "tex!=null");
            Texture2D texture2D = tex.MakeReadable(linear);
            texture2D.name = tex.name;
            texture2D.anisoLevel = tex.anisoLevel;
            texture2D.filterMode = tex.filterMode;
            return texture2D;
        }

        public static Texture2D MakeReadable(this Texture texture, bool linear)
        {
            RenderTextureReadWrite readWrite = linear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.Default;
            RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, readWrite);
            Graphics.Blit(texture, temporary);
            texture = temporary.ToTexture2D();
            RenderTexture.ReleaseTemporary(temporary);
            return texture as Texture2D;
        }

        public static bool IsReadable(this Texture2D texture)
        {
            try
            {
                texture.GetPixel(0, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Texture2D TryMakeReadable(this Texture2D texture)
        {
            if (texture.IsReadable())
            {
                return texture;
            }
            return texture.MakeReadable();
        }

        public static void Dump(this Texture2D tex, string path)
        {
            if (tex == null)
            {
                throw new ArgumentNullException("tex");
            }
            Log.Called(tex.name, path);
            byte[] bytes = tex.TryMakeReadable().EncodeToPNG() ?? throw new Exception($"bytes == null. Failed to dump {tex?.name} with format  {tex.format} to {path}.");
            File.WriteAllBytes(path, bytes);
            Log.Succeeded();
        }
    }
}
