using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private float _radius; 
    [SerializeField] private float _explosionForce; 

    private Camera _mainCamera;

    private int _leftMouseButton = 0;

    private float _scaleFactor = 0.5f;

    private void Start()
    {
        _mainCamera = Camera.main;

        if (_cubeSpawner != null)
        {
            _cubeSpawner.SetFragmentOnSpawn(true);
            _cubeSpawner.SetScaleFactor(_scaleFactor);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_leftMouseButton))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Cube hitCube))
                {
                    float splitChance = hitCube.GetSplitChance();

                    Vector3 hitCubePosition = hitCube.transform.position;
                    List<Cube> spawnedCubes = _cubeSpawner.SpawnCubes(hitCubePosition, splitChance);

                    Destroy(hitCube.gameObject);
                    Explode(hitCubePosition, spawnedCubes);
                }
            }
        }
    }

    private void Explode(Vector3 center, List<Cube> spawnedCubes)
    {
        if (spawnedCubes != null)
        {
            foreach (Cube cube in spawnedCubes)
            {
                if (cube.TryGetComponent(out Rigidbody rigidBody))
                {
                    float cubeSize = cube.transform.localScale.x;
                    float adjustedRadius = _radius / cubeSize;
                    float adjustedForce = _explosionForce / cubeSize;

                    rigidBody.AddExplosionForce(adjustedForce, center, adjustedRadius);
                }
            }
        }

        Collider[] colliders = Physics.OverlapSphere(center, _radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent(out Rigidbody nearbyRigidBody))
            {
                float distanceToExplosion = Vector3.Distance(center, nearbyObject.transform.position);

                float distanceFactor = 1 - (distanceToExplosion / _radius);
                float distanceAdjustedForce = _explosionForce * distanceFactor;

                nearbyRigidBody.AddExplosionForce(distanceAdjustedForce, center, _radius);
            }
        }
    }
}
