using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float touchEffectSpeed = 0.1f;
    public float tileAnimationSpeed = 0.3f;

    private Vector3 tileFinalPosition;

    #region Touch Effect 
    private SpriteRenderer effectSpriteRenderer;
    private Color32 tileColor;
    private Vector3 effectScale;
    private float colorAlpha;
    #endregion

    private void Awake()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>())
            effectSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        else
            Debug.LogError("Sprite Renderer is missing");
    }

    private void Start()
    {
        EffectColorSetup();
    }

    private void OnEnable()
    {
        EffectColorSetup();

        //StartCoroutine(DeactivateTile(10));
    }

    private void EffectColorSetup()
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
        EffectLerp();

        transform.localPosition = Vector3.Lerp(transform.localPosition, tileFinalPosition, tileAnimationSpeed);

    }

    private void EffectLerp()
    {
        // Scale
        effectSpriteRenderer.transform.localScale = Vector3.Lerp(effectSpriteRenderer.transform.localScale, effectScale, touchEffectSpeed);

        // Color
        tileColor.a = (byte)Mathf.Lerp(tileColor.a, colorAlpha, touchEffectSpeed);
        effectSpriteRenderer.color = tileColor;
    }

    public void TouchEffect()
    {
        effectSpriteRenderer.gameObject.SetActive(true);
        effectScale = Vector3.one * 20;
        colorAlpha = 0f;
    }

    public void SetTilePosition(Vector3 position)
    {
        tileFinalPosition = position;
    }

    public IEnumerator DeactivateTile(float time = 5f)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        TouchEffect();
    }
}
