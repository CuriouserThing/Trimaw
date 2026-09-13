using Godot;

namespace Trimaw.Core.GodotControls;

public abstract class NodeWrapper<T>(T node) where T : Node
{
    public T Node { get; } = node;
}