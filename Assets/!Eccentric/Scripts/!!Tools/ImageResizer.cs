#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eccentric
{
    [ExecuteAlways]
    public class ImageResizer
    {
        private static string _path;

        private static string[] pngFiles;
        private static string[] jpgFiles;

        private static string _root = "Assets";


        [MenuItem("Tools/Optimization images")]
        public static void Resize()
        {
            pngFiles = Directory.GetFiles(_root + _path, "*.png", SearchOption.AllDirectories);
            jpgFiles = Directory.GetFiles(_root + _path, "*.jpg", SearchOption.AllDirectories);


            foreach (var filePath in pngFiles)
            {
                ResizeTexture(filePath, ImageType.png);
            }

            foreach (var filePath in jpgFiles)
            {
                ResizeTexture(filePath, ImageType.jpg);
            }

            Debug.Log("Resize completed");
        }


        private static Texture2D LoadTexture(string path)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }

        private static void ResizeTexture(string filePath, ImageType imageType)
        {



            Texture2D originalTexture = LoadTexture(filePath);

            int newWidth = originalTexture.width - originalTexture.width % 4;
            int newHeight = originalTexture.height - originalTexture.height % 4;


            if (originalTexture.width % 4 != 0) newWidth += 4;
            if (originalTexture.height % 4 != 0) newHeight += 4;


            if (newWidth == originalTexture.width && newHeight == originalTexture.height)
            {
                Object.DestroyImmediate(originalTexture);
                originalTexture = null;
                EnableCrunchCompression(filePath);
                Resources.UnloadUnusedAssets();
                return;
            }

            newWidth = Mathf.Max(4, newWidth);
            newHeight = Mathf.Max(4, newHeight);

            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            RenderTexture.active = rt;
            Graphics.Blit(originalTexture, rt);
            Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
            resizedTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            resizedTexture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            Debug.Log(
                $"original width {originalTexture.width}, new {newWidth}\noriginal height {originalTexture.height}, new {newHeight}");
            SaveTextureToFile(resizedTexture, filePath, imageType);

            Object.DestroyImmediate(originalTexture);
            originalTexture = null;

            EnableCrunchCompression(filePath);

            Resources.UnloadUnusedAssets();
        }

        private static void SaveTextureToFile(Texture2D texture, string path, ImageType imageType)
        {
            if (imageType == ImageType.png)
            {
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
            }
            else
            {
                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes(path, bytes);
            }
        }

        private static void EnableCrunchCompression(string pathName)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(pathName) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                textureImporter.crunchedCompression = true;
                textureImporter.compressionQuality = 100;
            }

            AssetDatabase.ImportAsset(pathName);
        }

        enum ImageType
        {
            png,
            jpg,
        }
    }
}
#endif