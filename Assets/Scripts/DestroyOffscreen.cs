using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    public float offset = 16f;

    public delegate void OnDestroy();

    public event OnDestroy DestroyCallback;

    private bool offscreen;
    private float offscreenX;
    private Rigidbody2D body2d;

    private void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    private void Start()
    {
        offscreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offset;
    }

    // Update is called once per frame
    private void Update()
    {
        var posX = transform.position.x;
        var dirX = body2d.velocity.x;
        offscreen = false;
        if (Mathf.Abs(posX) > offscreenX)
        {
            if (dirX < 0 && posX < -offscreenX)
            {
                offscreen = true;
            }
            else if (dirX > 0 && posX > offscreenX)
            {
                offscreen = true;
            }
        }

        if (offscreen)
        {
            OnOutOfBounds();
        }
    }

    private void OnOutOfBounds()
    {
        offscreen = false;
        GameObjectUtil.Destroy(gameObject);

        if (DestroyCallback != null)
        {
            DestroyCallback();
        }
    }
}