using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackground : MonoBehaviour
{
    public int textureSize = 32;
    public bool scaleHorizontally = true;
    public bool scaleVertically = true;

    // Use this for initialization
    private void Start()
    {
        var width = !scaleHorizontally ? 1 : Mathf.Ceil(Screen.width / (textureSize * PixelPerfectCamera.scale));
        var height = !scaleVertically ? 1 : Mathf.Ceil(Screen.height / (textureSize * PixelPerfectCamera.scale));

        transform.localScale = new Vector3(width * textureSize, height * textureSize, 1);
        GetComponent<Renderer>().material.mainTextureScale = new Vector3(width, height, 1);
    }
}