using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgress", menuName = "System/Level/LevelProgress")]
public class LevelLine : ScriptableObject
{
    [SerializeField] private AnimationCurve _curve;

    public AnimationCurve CurveLevel { get { return _curve; } }

    public void SetLevelLine(AnimationCurve curve)
    {
        _curve = curve;
    }
}


