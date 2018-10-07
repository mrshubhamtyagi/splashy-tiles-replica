using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float hitEffectSpeed = 0.1f;
    public float tileSpawnAnimSpeed = 0.3f;

    private Vector3 tileFinalPosition;
    [HideInInspector] public Color newColor;

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

    public void EffectColorSetup()
    {
        tileColor = GetComponent<MeshRenderer>().material.color;
        newColor = tileColor;
        effectSpriteRenderer.transform.localScale = Vector3.zero;
        effectSpriteRenderer.color = tileColor;
        colorAlpha = 255f;
        effectScale = Vector3.zero;
        effectSpriteRenderer.gameObject.SetActive(false);
    }

    void Update()
    {
        EffectLerp();

        transform.localPosition = Vector3.Lerp(transform.localPosition, tileFinalPosition, tileSpawnAnimSpeed);
        GetComponent<MeshRenderer>().material.color =
                                        Color.Lerp(GetComponent<MeshRenderer>().material.color, newColor, hitEffectSpeed);
    }

    private void EffectLerp()
    {
        // Scale
        effectSpriteRenderer.transform.localScale = Vector3.Lerp(effectSpriteRenderer.transform.localScale, effectScale, hitEffectSpeed);

        // Color
        tileColor.a = (byte)Mathf.Lerp(tileColor.a, colorAlpha, hitEffectSpeed);
        effectSpriteRenderer.color = tileColor;
    }

    public void OnHitEffect()
    {
        effectSpriteRenderer.gameObject.SetActive(true);
        effectScale = Vector3.one * 30;
        colorAlpha = 0f;
        tileFinalPosition.y = -1f;
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
        OnHitEffect();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnHitEffect();
        }
    }
}
