using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Mule.COmponents;

/// <summary>
/// This is used for the MULE
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(MuleSystem))]
[AutoGenerateComponentState]
public sealed partial class MuleComponent : Component
{
    /// <summary>
    /// The current order that the MULE has been assigned.
    /// </summary>
    [DataField, AutoNetworkedField]
    public MuleOrderType CurrentOrder = MuleOrderType.Idle;

    [DataField, AutoNetworkedField]
    public EntityUid? CurrentTarget;

    [DataField, AutoNetworkedField]
    public EntityUid? CurrentObject;
}

[Serializable, NetSerializable]
public enum MuleOrderType : byte
{
    Idle,
    Return,
    Transport,
    Blocked
}
