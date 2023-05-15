using System;
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
            if (_colors == null)
            {
                _colors = Array.Empty<ColorToPrefab>();
            }

            if (_colorOptions == null)
            {
                _colorOptions = Array.Empty<Color>();
            }

            var imageToPrefab = ScriptableObject.CreateInstance<ImageToPrefabMap>();
            imageToPrefab.name = Path.GetFileNameWithoutExtension(ctx.assetPath) + " map";
            
            var texture = new Texture2D(2, 2);//size does not matter, since loadImage will replace it.
            texture.LoadImage(System.IO.File.ReadAllBytes(ctx.assetPath));

            EditorUtility.SetDirty(imageToPrefab);
            
            texture.filterMode = FilterMode.Point;
            // texture.wrapMode = TextureWrapMode.Repeat;
            texture.name = Path.GetFileNameWithoutExtension(ctx.assetPath) + " tex";
            imageToPrefab.SetMapTexture(texture);

            ctx.AddObjectToAsset("texture obj", texture);
            ctx.AddObjectToAsset("map object", imageToPrefab);
            _colorOptions = GetAllColorsFromTexture(texture);
            if (_colorOptions.Length > 25)
            {
                Debug.LogWarning("You have a lot of colors in your map. This is likely unintended and may cause performance and UX issues.", this);
            }
            for (var i = 0; i < _colors.Length; i++)
            {
                _colors[i].ColorOptions = _colorOptions;
            }

            imageToPrefab.AllColorsInTexture = _colorOptions;
            imageToPrefab.SetPrefabColors(_colors);
            EditorUtility.SetDirty(this);
            
            ctx.SetMainObject(imageToPrefab);
            
            // ctx..icon
            EditorGUIUtility.SetIconForObject(imageToPrefab, texture);
            

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