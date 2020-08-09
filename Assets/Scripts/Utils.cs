using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using System.Linq;
using System.IO;

namespace FloDa.Utils 
{
    public static class Utils
    {
        public const int sortingOrderDefault = 5000;
 
    public static class IMG2Sprite
    {

        //Static class instead of _instance
        // Usage from any other script:
        // MySprite = IMG2Sprite.LoadNewSprite(FilePath, [PixelsPerUnit (optional)], [spriteType(optional)])

        public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
        {

            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            Texture2D SpriteTexture = LoadTexture(FilePath);
            if (SpriteTexture != null)
            {
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
                return NewSprite;
            }
            return null;    
        }

        public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
        {
            // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference

            Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

            return NewSprite;
        }

        public static Texture2D LoadTexture(string FilePath)
        {

            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }


    public static int[,] FindAllIndexOf(int[,] list, int value)
        {
            int amount = 0;
            int counter = 0;

            for (int x = 0; x < list.GetLength(0); x++)
            {
                for (int y = 0; y < list.GetLength(1); y++)
                {
                    if (list[x,y] == value)
                    {
                        amount += 1;
                    }
                }
            }

            int[,] indexArray = new int[amount,2];

            for (int x = 0; x < list.GetLength(0); x++)
            {
                for (int y = 0; y < list.GetLength(1); y++)
                {
                    if (list[x, y] == value)
                    {
                        indexArray[counter, 0] = x;
                        indexArray[counter, 1] = y;
                        counter += 1;
                    }
                }
            }

            return indexArray;
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) 
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
            
        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) 
        {
            GameObject gameObject = new GameObject("idx"+text, typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }

        public static Vector2 GridArrayToWorldPosition(Vector2 gridPosition)
        {
            return new Vector2(gridPosition.x - 10, gridPosition.y - 5);
        }
        public static Vector3Int GridArrayToWorldPosition(Vector3 gridPosition)
        {
            return new Vector3Int((int)gridPosition.x - 10, (int)gridPosition.y - 5, 0);
        }

        public static GameObject CreateWorldSprite(string path, string name, int index, Vector3 localPosition, Vector2 localScale, bool active)
        {
            GameObject gameObject = new GameObject(name + index);
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();

            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localScale = new Vector3(10, 10, 0);
            renderer.enabled = active;

            return gameObject;
        }

        public static GameObject CreateWorldButton(Transform  parent, string name, int index, Vector3 localPosition)
        {
            GameObject gameObject = new GameObject(name + index);
            Image image = gameObject.AddComponent<Image>();
            Button button = gameObject.AddComponent<Button>();
            Transform transform = gameObject.transform;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            rectTransform.sizeDelta = new Vector2(50, 30);

            return gameObject;
        }

        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}