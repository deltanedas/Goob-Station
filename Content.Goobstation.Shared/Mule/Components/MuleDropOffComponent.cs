using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Mule.Components;

/// <summary>
/// Component for mule drop off beacons.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MuleDropOffComponent : Component
{
    /// <summary>
    /// Name of the drop off.
    /// Unrelated to TagPrototype.
    /// </summary>
    [DataField(required: true)]
    public string Tag = string.Empty;
}
