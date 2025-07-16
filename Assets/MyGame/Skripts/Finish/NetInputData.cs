using Fusion;
using UnityEngine;

public struct NetInputData : INetworkInput
{
    public const byte Attack = 1;
    public const byte Defence = 2;

    public NetworkButtons buttons;
    public Vector3 _rawInputMovement;
}
