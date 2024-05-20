using UnityEngine;

public class Cube : MonoBehaviour
{
    private float _splitChance;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public float GetSplitChance()
    {
        return _splitChance;
    }

    public void SetSplitChance(float value)
    {
        _splitChance = value;
    }

    public void SetColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.material.color = color;
        }
    }

    public void RandomizeColor()
    {
        SetColor(Random.ColorHSV());
    }
}
