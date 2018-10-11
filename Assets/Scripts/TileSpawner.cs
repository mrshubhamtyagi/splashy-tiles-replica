using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector3 tileOffset; // next tile distance
    public float tileAnimDistanceInZ = 10f;
    public bool randomX = false;

    [Header("Color Stuff")]
    public bool randomColor = true;
    public float tilesColorChangeSpeed = 0.01f;
    public Color[] tileColors;


    private Vector3 prevTilePosition = Vector3.zero;
    private List<Transform> tiles = new List<Transform>(10);
    private int _prevNumber;

    void Start()
    {
        SpawnTileFromPool();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnTileFromPool();

        if (Input.GetKeyDown(KeyCode.Backspace))
            StartCoroutine(Co_ChangeTilesColor(tileColors[GetRandomNumber(0, tileColors.Length)], tilesColorChangeSpeed));
    }

    public void AddToList(Tile tile)
    {
        tiles.Add(tile.transform);
    }

    public void RemoveFromList(Tile tile)
    {
        tiles.Remove(tile.transform);
    }

    [ContextMenu("SpawnTile")]
    public Transform SpawnTileFromPool()
    {
        string tileStatus = "New Tile is Generated";
        bool _isTileAvailable = false;
        Transform _tile = null;

        // Find a tile that is not active (available to spawn)
        if (tiles.Count > 0)
        {
            foreach (Transform tile in transform)
            {
                if (tile.gameObject.activeSelf) // if tile is active skip it
                    continue;

                // ---------------------------- if a tile is available in the pool, use that tile
                _tile = tile;
                _isTileAvailable = true;
                tileStatus = "Tile is Re-Generated";
                break;
            }
        }

        // ------------------ if no tile is available to spawn, create a new tile
        if (!_isTileAvailable || tiles.Count == 0)
        {
            _tile = Instantiate(tilePrefab);
            _tile.parent = transform;
            tiles.Add(_tile);
        }

        Vector3 _tilePos = GetNextTileFixedPosition(); // get tile position
        _tile.localPosition = new Vector3(_tilePos.x, _tilePos.y, _tilePos.z + tileAnimDistanceInZ); // add tile animation distance
        _tile.GetComponent<Tile>().SetTilePosition(_tilePos);
        _tile.rotation = Quaternion.identity;

        // set tile color
        if (randomColor)
            _tile.GetComponent<MeshRenderer>().material.color = tileColors[GetRandomNumber(0, tileColors.Length)];
        //else
        //_tile.GetComponent<MeshRenderer>().material.color = tileColors[0];

        _tile.gameObject.SetActive(true);
        print(tileStatus);
        return _tile.transform;
    }

    public IEnumerator Co_ChangeTilesColor(Color newColor, float waitTime = 0.1f)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (!tiles[i].gameObject.activeSelf)
                continue;

            yield return new WaitForSeconds(waitTime);
            tiles[i].GetComponent<MeshRenderer>().material.color = newColor;
            tiles[i].GetComponent<Tile>().FadeEffectSetup();
            tiles[i].GetComponent<Tile>().newColor = newColor;
        }
    }

    private Vector3 GetNextTileFixedPosition()
    {
        Vector3 _position = Vector3.zero;
        float[] _rangeX = new float[3] { -1.5f, 0f, 1.5f }; // X positions
        int _randomNumber = 1; //  1st position in the _range which is 0 is set by default

        // Find new random X position
        if (randomX)
        {
            _randomNumber = GetRandomNumber(0, _rangeX.Length);
            while (Mathf.Abs(_prevNumber - _randomNumber) != 1
                   || _prevNumber == _randomNumber)
            {
                _randomNumber = GetRandomNumber(0, _rangeX.Length);
            }
            _prevNumber = _randomNumber;
        }

        _position = new Vector3(_rangeX[_randomNumber], prevTilePosition.y, tileOffset.z + prevTilePosition.z);

        prevTilePosition = _position;
        return _position;
    }

    private Vector3 GetNextTileRandomPosition()
    {
        Vector3 _position = Vector3.zero;
        float[] _range = new float[3] { -1.5f, 0f, 1.5f };

        int _randomNumber = GetRandomNumber(0, _range.Length);

        while (Mathf.Abs(_prevNumber - _randomNumber) != 1
               || _prevNumber == _randomNumber)
        {
            _randomNumber = GetRandomNumber(0, _range.Length);
        }
        _prevNumber = _randomNumber;

        _position.x = _range[_randomNumber];
        _position.z = prevTilePosition.z + tileOffset.z;

        if (!randomX)
            _position.x = 0;

        prevTilePosition = _position;


        return _position;
    }

    private int GetRandomNumber(int start, int end)
    {
        return Random.Range(start, end);
    }
}
