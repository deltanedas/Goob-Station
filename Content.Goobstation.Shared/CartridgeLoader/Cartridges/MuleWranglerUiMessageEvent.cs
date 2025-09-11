using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class MuleWranglerReturnMessage : CartridgeMessageEvent;

[Serializable, NetSerializable]
public sealed class MuleWranglerTransportMessage : CartridgeMessageEvent;

// TODO: implement
[Serializable, NetSerializable]
public sealed class MuleWranglerUnloadMessage : CartridgeMessageEvent;

[Serializable, NetSerializable]
public sealed class MuleWranglerSetMuleMessage(NetEntity? mule) : CartridgeMessageEvent
{
    public readonly NetEntity? Mule = mule;
}

[Serializable, NetSerializable]
public sealed class MuleWranglerSetDestinationMessage(NetEntity? beacon) : CartridgeMessageEvent
{
    public readonly NetEntity? Beacon = beacon;
}
