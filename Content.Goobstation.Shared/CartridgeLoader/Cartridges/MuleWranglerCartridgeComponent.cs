using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.CartridgeLoader.Cartridges;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedMuleWranglerCartridgeSystem))]
[AutoGenerateComponentState]
public sealed partial class MuleWranglerCartridgeComponent : Component
{
    [AutoNetworkedField]
    public NetEntity? SelectedMule;

    [AutoNetworkedField]
    public NetEntity? SelectedBeacon;
}
