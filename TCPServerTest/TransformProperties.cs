using System.Numerics;

namespace TCPServerTest;

public struct TransformProperties
{
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    public int Id { get; set; }
}