using UnityEngine;

[System.Serializable]
public class SerializableWeapon : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;
    [SerializeField] public int ashOfWarID;

    public WeaponItem GetWeapon()
    {
        WeaponItem weaponItem = WorldItemDatabase.instance.GetWeaponFromSerializedData(this);
        return weaponItem;
    }
    
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
      
    }
}
