using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float hitEffectSpeed = 0.1f;
    public float tileSpawnAnimSpeed = 0.3f;
    public float force = 2f;
    public float moveSpeed = 2f;
    public bool isStatic = false;

    #region Touch Effect Attributes
    private SpriteRenderer effectSpriteRenderer;
    private Color32 tileColor;
    private Vector3 effectScale;
    private float colorAlpha;
    #endregion

    [HideInInspector] public Color newColor;
    private Vector3 tileFinalPosition;
    private TileSpawner tileSpawner;

    public void SetTilePosition(Vector3 position)
    {
        tileFinalPosition = position;
    }

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
        //tileFinalPosition = transform.localPosition;
        FadeEffectSetup();
    }

    #region Effect Stuff
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

    public IEnumerator Co_OnHitEffect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        effectSpriteRenderer.gameObject.SetActive(true);
        effectScale = Vector3.one * 30;
        colorAlpha = 0f;
        GetComponent<MeshRenderer>().material.color = Color.black;

        yield return new WaitForSeconds(waitTime * 2); // Lower the tile on hit
        tileFinalPosition.y = -3f;

        yield return new WaitForSeconds(1f); // Deactivate Tile
        gameObject.SetActive(false);
    }

    private void OnHitEffect()
    {
        // Scale
        effectSpriteRenderer.transform.localScale = Vector3.Lerp(effectSpriteRenderer.transform.localScale, effectScale, hitEffectSpeed);

        // Color
        tileColor.a = (byte)Mathf.Lerp(tileColor.a, colorAlpha, hitEffectSpeed);
        effectSpriteRenderer.color = tileColor;
    }
    #endregion

    private void MoveTile()
    {
        if (!isStatic)
            transform.localPosition = Vector3.Lerp(transform.localPosition, tileFinalPosition, tileSpawnAnimSpeed);
    }

    void Update()
    {
        OnHitEffect();
        MoveTile();

        GetComponent<MeshRenderer>().material.color =
                                        Color.Lerp(GetComponent<MeshRenderer>().material.color, newColor, hitEffectSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.VelocityChange);
            StartCoroutine(Co_OnHitEffect(0.1f));

            //if (tileSpawner.transform.childCount > 5)
            tileSpawner.SpawnTileFromPool();
        }
    }

    private IEnumerator DeactivateTile(float time = 1f)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
