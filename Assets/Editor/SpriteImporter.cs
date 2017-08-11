using UnityEngine;
using UnityEditor;


class SpriteImporter : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		if (assetPath.Contains("Sprites"))
		{
			TextureImporter textureImporter = (TextureImporter)assetImporter;
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.spriteImportMode = SpriteImportMode.Single;
			textureImporter.alphaIsTransparency = true;
			textureImporter.wrapMode = TextureWrapMode.Clamp;
			textureImporter.anisoLevel = 1;
		}
	}
	void OnPreprocessModel()
	{
		var modelImporter = (ModelImporter)assetImporter;
		modelImporter.importAnimation = false;
		modelImporter.animationType = ModelImporterAnimationType.None;
	}

}