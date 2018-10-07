using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float lerpValue = 0.1f;

    private SpriteRenderer effectSpriteRenderer;
    private Color32 tileColor;
    private Vector3 effectScale;
    private float colorAlpha;

    private void Awake()
    {
        if (GetComponentInChildren<SpriteRenderer>())
            effectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        else
            Debug.LogError("Sprite Rendere is missing");
    }

    private void OnEnable()
    {
        tileColor = GetComponent<MeshRenderer>().material.color;
        effectSpriteRenderer.transform.localScale = Vector3.zero;
        effectSpriteRenderer.color = tileColor;
        colorAlpha = 255f;
        effectScale = Vector3.zero;
        effectSpriteRenderer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TouchEffect();
        effectSpriteRenderer.transform.localScale =
                                Vector3.Lerp(effectSpriteRenderer.transform.localScale, effectScale, lerpValue);

        tileColor.a = (byte)Mathf.Lerp(tileColor.a, colorAlpha, lerpValue);
        effectSpriteRenderer.color = tileColor;
    }

    private void TouchEffect()
    {
        effectSpriteRenderer.gameObject.SetActive(true);
        effectScale = Vector3.one * 20;
        colorAlpha = 0f;
    }


}
