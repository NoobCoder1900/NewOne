using UnityEngine;

public struct FrameInput
{
    public float runInput;
    public bool jumpKeyDown;
    public bool jumpKeyUp;
};

public interface IPlayerController
{
    public Vector3 Velocity { get; }
    public FrameInput input { get; }
    public bool JumpingThisFrame { get; }
    public bool LandingThisFrame { get; }
    public Vector3 RawMovement { get; }
    public bool Grounded { get; }
}

public struct RayRange
{
    public RayRange(float start1, float start2, float end1, float end2, Vector2 dir)
    {
        start = new Vector2(start1, start2);
        end = new Vector2(end1, end2);
        direction = dir;
    }

    public readonly Vector2 start, end, direction;
}