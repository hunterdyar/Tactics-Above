using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
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
            var imageToPrefab = ScriptableObject.CreateInstance<ImageToPrefabMap>();
            imageToPrefab.name = Path.GetFileNameWithoutExtension(ctx.assetPath) + " map";
            
            var texture = new Texture2D(2, 2);//size does not matter, since loadImage will replace it.
            texture.LoadImage(System.IO.File.ReadAllBytes(ctx.assetPath));

            EditorUtility.SetDirty(imageToPrefab);

            texture.filterMode = FilterMode.Point;
            // texture.wrapMode = TextureWrapMode.Repeat;
            texture.name = Path.GetFileNameWithoutExtension(ctx.assetPath) + " sprite";
            imageToPrefab.SetMapTexture(texture);
            

            ctx.AddObjectToAsset("sprite obj", texture);
            ctx.AddObjectToAsset("map object", imageToPrefab);
            ctx.SetMainObject(imageToPrefab);
            // ctx..icon
        }
    }
}