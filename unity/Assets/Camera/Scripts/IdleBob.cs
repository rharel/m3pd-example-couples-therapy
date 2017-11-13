using rharel.Debug;
using UnityEngine;

/// <summary>
/// This behavior manipulates the transform as to mimic an idle person's 
/// natural head-bob using Perlin noise.
/// </summary>
public sealed class IdleBob: MonoBehaviour
{
    /// <summary>
    /// The frequency of direction changes. This is the velocity with which we
    /// move through the noise texture (in pixels/second).
    /// </summary>
    public float roughness = 1.0f;
    /// <summary>
    /// The velocity with which the transform moves (in units/second).
    /// </summary>
    public float speed = 0.1f;
    
    /// <summary>
    /// The minimum/maximum rotational offset from its original value.
    /// </summary>
    public Rect offsetRange = new Rect(-1, -1, 2, 2);

    /// <summary>
    /// The seeding value for the noise generator.
    /// </summary>
    public Vector2 seed = Vector2.zero;

    /// <summary>
    /// The original position.
    /// </summary>
    public Vector2 Midpoint { get; private set; }

    void Awake()
    {
        Require.IsAtLeast(roughness, 0.0f);
        Require.IsAtLeast(speed, 0.0f);

        _perlin_position = seed;
    }

    void Start()
    {
        Midpoint = transform.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        _perlin_position += Vector2.one * roughness * dt;

        Vector2 noise = new Vector2(
            Mathf.PerlinNoise(_perlin_position[0], 0.0f),
            Mathf.PerlinNoise(0.0f, _perlin_position[1])
        );
        Vector2 direction = (noise - 0.5f * Vector2.one) * 2;
        Vector2 velocity = direction * speed * dt;

        Vector3 new_position = new Vector3(
            transform.position.x + velocity.x,
            transform.position.y + velocity.y,
            transform.position.z
        );
        new_position = new Vector3(
            Mathf.Clamp(
                new_position.x, 
                Midpoint.x + offsetRange.xMin, 
                Midpoint.x + offsetRange.xMax
            ),
            Mathf.Clamp(
                new_position.y,
                Midpoint.y + offsetRange.yMin,
                Midpoint.y + offsetRange.yMax
            ),
            new_position.z
        );

        transform.position = new_position;
    }

    private Vector2 _perlin_position;
}
