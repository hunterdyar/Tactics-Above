using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HDyar.MapImporter
{


    [ScriptedImporter(version:1,new []{"aseprite"},new []{"png"})]
        public class MapImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            
            var texture = new Texture2D(2, 2);//size does not matter, since loadImage will replace it.
            texture.LoadImage(System.IO.File.ReadAllBytes(ctx.assetPath));
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            ctx.AddObjectToAsset("main obj", sprite);
            ctx.SetMainObject(sprite);
        }
    }
}