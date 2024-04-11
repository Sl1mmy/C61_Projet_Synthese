using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;

public class OpenImageplz : MonoBehaviour
{
    public Image displayImage;

    public void ImportImage()
    {
        // Open a file dialog to choose an image file
        string imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

        if (!string.IsNullOrEmpty(imagePath))
        {
            // Read the image file and load it into a texture
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);

            // Display the texture in the image element
            displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
    }
}
