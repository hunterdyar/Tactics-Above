using UnityEngine;

namespace HDyar.MapImporter
{
	[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
	public class ImageToPrefabMap : ScriptableObject
	{
		public Texture2D _mapTexture;

		public void SetMapTexture(Texture2D texture)
		{
			_mapTexture = texture;
		}
	}
}