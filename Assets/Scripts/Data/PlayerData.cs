using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData> ,INetworkSerializable
{ 
    public ulong clientID;
    public int colorId;
    public FixedString64Bytes playerName;
    public bool Equals(PlayerData other)
    {
        return clientID == other.clientID && colorId == other.colorId && playerName == other.playerName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerName);

    }
}
