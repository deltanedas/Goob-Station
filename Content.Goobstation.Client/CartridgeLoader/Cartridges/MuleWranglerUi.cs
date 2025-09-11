using Content.Client.UserInterface.Fragments;
using Content.Goobstation.Shared.CartridgeLoader.Cartridges;
using Content.Shared.CartridgeLoader;
using Robust.Client.UserInterface;

namespace Content.Goobstation.Client.CartridgeLoader.Cartridges;

public sealed partial class MuleWranglerUi : UIFragment
{
    private MuleWranglerUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        private void Send(CartridgeUiMessageEvent msg)
        {
            userInterface.SendMessage(new CartridgeUiMessage(msg));
        }

        _fragment = new MuleWranglerUiFragment(fragmentOwner!.Value);
        _fragment.OnTransport += () => Send(new MuleWranglerTransportMessage());
        _fragment.OnReturn += () => Send(new MuleWranglerReturnMessage());
        _fragment.OnUnload += () => Send(new MuleWranglerUnloadMessage());
        _fragment.OnSetMule += mule => Send(new MuleWranglerSetMuleMessage(mule));
        _fragment.OnSetBeacon += beacon => Send(new MuleWranglerSetBeaconMessage(mule));
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not MuleWranglerUiState cast)
            return;

        _fragment?.UpdateState(cast);
    }
}
