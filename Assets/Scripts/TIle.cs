using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float hitEffectSpeed = 0.1f;
    public float tileSpawnAnimSpeed = 0.3f;
    public float force = 2f;
    public float moveSpeed = 2f;
    public bool move;

    #region Touch Effect 
    private SpriteRenderer effectSpriteRenderer;
    private Color32 tileColor;
    private Vector3 effectScale;
    private float colorAlpha;
    #endregion

    [HideInInspector] public Color newColor;
    private Vector3 tileFinalPosition;
    private TileSpawner tileSpawner;
    private bool isLerpEnable;

    private void Awake()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>())
            effectSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        else
            Debug.LogError("Sprite Renderer is missing");

        tileSpawner = FindObjectOfType<TileSpawner>();
    }

    private void OnEnable()
    {
        FadeEffectSetup();
        isLerpEnable = true;
        Invoke("DisableLerp", 0.2f);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        //tileSpawner.SpawnTileFromPool();
    }

    public void FadeEffectSetup()
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
        MoveTile();

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

    public IEnumerator OnHitEffect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        effectSpriteRenderer.gameObject.SetActive(true);
        effectScale = Vector3.one * 30;
        colorAlpha = 0f;

        yield return new WaitForSeconds(waitTime * 2);
        tileFinalPosition.y = -1f;

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void SetTilePosition(Vector3 position)
    {
        tileFinalPosition = position;
    }

    private void OnMouseDown()
    {
        StartCoroutine(OnHitEffect(0.1f));
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.VelocityChange);
            //col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * (force / 3.6f), ForceMode.VelocityChange);
            StartCoroutine(OnHitEffect(0.1f));
            tileSpawner.SpawnTileFromPool();
        }
    }

    private void MoveTile()
    {
        if (isLerpEnable)
            transform.localPosition = Vector3.Lerp(transform.localPosition, tileFinalPosition, tileSpawnAnimSpeed);
        else if (move)
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

    }

    private void DisableLerp()
    {
        isLerpEnable = false;
    }
}
