using UnityEngine;
using System.Reflection;
using Modding;
using SFCore;

namespace SerratedDreams
{
    public class SerratedDreamCharm : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "This charm alllows the bearer to deal damage with Dream nail.";
        protected override string GetName() => "Serrated Dreams";
        protected override Sprite GetSpriteInternal()
        {
            Texture2D tex = new Texture2D(2, 2);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SerratedDreams.Resources.charm.png"))
            {
                if (stream == null)
                {
                    Modding.Logger.Log("Image not found!!!");
                    return null;
                }
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                if (buffer == null)
                {
                    return null;
                }
                tex.LoadImage(buffer);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}