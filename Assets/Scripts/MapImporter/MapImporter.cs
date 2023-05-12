using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HDyar.MapImporter
{


    [ScriptedImporter(version:1,new []{""},new []{"png"})]
    public class MapImporter : ScriptedImporter
    {
        [SerializeField] private ColorToPrefab[] _colors;
        [SerializeField] private Color[] _colorOptions;
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
            imageToPrefab.SetPrefabColors(_colors);

            ctx.AddObjectToAsset("sprite obj", texture);
            ctx.AddObjectToAsset("map object", imageToPrefab);
            _colorOptions = GetAllColorsFromTexture(texture);
            for (var i = 0; i < _colors.Length; i++)
            {
                _colors[i].ColorOptions = _colorOptions;
            }

            ctx.SetMainObject(imageToPrefab);
            // ctx..icon
        }

        public static Color[] GetAllColorsFromTexture(Texture2D texture)
        {
            HashSet<Color> colors = new HashSet<Color>();
            foreach (var color in texture.GetPixels(0, 0, texture.width, texture.height))
            {
                colors.Add(color);
            }

            return colors.ToArray();
        }

        public Color[] GetColorOptions()
        {
            return _colorOptions;
        }
    }
}