using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Mule.Components;

/// <summary>
/// Prevents activation (E) when this entity is buckled.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BlockActivationWhenBuckledComponent : Component;
