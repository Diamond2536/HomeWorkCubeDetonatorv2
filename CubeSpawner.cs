using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubeTemplate;
    [SerializeField] private int _minCubesQuantity = 2;
    [SerializeField] private int _maxCubesQuantity = 7;
    [SerializeField] private bool _spawnOnStart = false;
    [SerializeField] private bool _fragmentOnSpawn = false;
    [SerializeField] private float _scaleFactor = 0.5f;

    private const float InitialSplitChance = 1.0f;

    private void Start()
    {
        if (_spawnOnStart)
        {
            SpawnCubes(transform.position, InitialSplitChance);
        }
    }

    public void SetFragmentOnSpawn(bool value)
    {
        _fragmentOnSpawn = value;
    }

    public void SetScaleFactor(float value)
    {
        _scaleFactor = value;
    }

    public List<Cube> SpawnCubes(Vector3 position, float parentSplitChance)
    {
        List<Cube> spawnedCubes = new List<Cube>();
        int cubesAmount = GetCubesQuantity();

        for (int i = 0; i < cubesAmount; i++)
        {
            Cube cube = SpawnCube(position, parentSplitChance);
            spawnedCubes.Add(cube);
        }

        return spawnedCubes;
    }

    private int GetCubesQuantity()
    {
        return Random.Range(_minCubesQuantity, _maxCubesQuantity);
    }

    private Cube SpawnCube(Vector3 position, float parentSplitChance)
    {
        Cube cube = Instantiate(_cubeTemplate, position, Quaternion.identity);
        cube.RandomizeColor();

        if (_fragmentOnSpawn)
        {
            cube.transform.localScale *= _scaleFactor;
        }

        cube.SetSplitChance(parentSplitChance * _scaleFactor);

        return cube;
    }
}
