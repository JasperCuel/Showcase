using Cue.Core;
using UnityEngine;

public class TestAttributes : MonoBehaviour
{
    [SerializeField]
    private string serializedPrivate;
    public string publicString;
    [ReadOnly]
    public string readonlyPublic = "Test";
    [SerializeField]
    [ReadOnly]
    private string readonlyPrivateSerialized = "Test";
}