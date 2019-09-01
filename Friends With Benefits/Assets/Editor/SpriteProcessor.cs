using UnityEngine;
using UnityEditor;

public class SpriteProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        string lowerCasePath = assetPath.ToLower();
        bool isInSpritesDirectory = lowerCasePath.IndexOf("/sprites/") != -1;

        if (isInSpritesDirectory)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePixelsPerUnit = 16;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }
}
