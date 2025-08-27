using UnityEngine;

[System.Serializable]
public class SerizlizableProjectile : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;
    [SerializeField] public int itemAmount;

    public RangedProjectileItem GetProjectile()
    {
        RangedProjectileItem projectile = WorldItemDatabase.instance.GetRangedProjectileFromSerializedData(this);
        return projectile;
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
