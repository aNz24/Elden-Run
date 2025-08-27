using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    [Header("Weapon Model Instantiation Slot")]
    [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
    [HideInInspector] public WeaponModelInstantiationSlot leftHandWeaponSlot;
    [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
    [HideInInspector] public WeaponModelInstantiationSlot backSlot;

    [Header("Weapon Models")]
    [HideInInspector] public GameObject rightHandWeaponModel;
    [HideInInspector] public GameObject leftHandWeaponModel;

    [Header("Weapon Managers")]
    public WeaponManager rightWeaponManager;
    public WeaponManager leftWeaponManager;


    [Header("General Equipment Models")]
    public GameObject hatsObject;
    [HideInInspector] public GameObject[] hats;
    public GameObject hoodsOject;
    [HideInInspector] public GameObject[] hoods;
    public GameObject faceCoversOject;
    [HideInInspector] public GameObject[] faceCovers;
    public GameObject helmetAccessoriesObject;
    [HideInInspector] public GameObject[] helmetAccessories;
    public GameObject backAccessoriesObject;
    [HideInInspector] public GameObject[] backAccessories;
    public GameObject hipAccessoriesObject;
    [HideInInspector] public GameObject[] hipAccessories;
    public GameObject rightShoulderObject;
    [HideInInspector] public GameObject[] rightShoulder;
    public GameObject rightElbowObject;
    [HideInInspector] public GameObject[] rightElbow;
    public GameObject rightKneeObject;
    [HideInInspector] public GameObject[] rightKnee;
    public GameObject leftShoulderObject;
    [HideInInspector] public GameObject[] leftShoulder;
    public GameObject leftElbowObject;
    [HideInInspector] public GameObject[] leftElbow;
    public GameObject leftKneeObject;
    [HideInInspector] public GameObject[] leftKnee;

    [Header("Male Equipment Models")]
    public GameObject maleFullHelmetObject;
    [HideInInspector] public GameObject[] maleHeadFullHelmets;
    public GameObject maleFullBodyObject;
    [HideInInspector] public GameObject[] maleBodies;
    public GameObject maleRightUpperArmObject;
    [HideInInspector] public GameObject[] maleRightUpperArms;
    public GameObject maleRightLowerArmObject;
    [HideInInspector] public GameObject[] maleRightLowerArms;
    public GameObject maleRightHandObject;
    [HideInInspector] public GameObject[] maleRightHands;
    public GameObject maleLeftUpperArmObject;
    [HideInInspector] public GameObject[] maleLeftUpperArms;
    public GameObject maleLeftLowerArmObject;
    [HideInInspector] public GameObject[] maleLeftLowerArms;
    public GameObject maleLeftHandObject;
    [HideInInspector] public GameObject[] maleLeftHands;
    public GameObject maleHipsObject;
    [HideInInspector] public GameObject[] maleHips;
    public GameObject maleRightLegObject;
    [HideInInspector] public GameObject[] maleRightLegs;
    public GameObject maleLeftLegObject;
    [HideInInspector] public GameObject[] maleLeftLegs;

    [Header("Female Equipment Models")]
    public GameObject femaleFullHelmetObject;
    [HideInInspector] public GameObject[] femaleHeadFullHelmets;
    public GameObject femaleFullBodyObject;
    [HideInInspector] public GameObject[] femaleBodies;
    public GameObject femaleRightUpperArmObject;
    [HideInInspector] public GameObject[] femaleRightUpperArms;
    public GameObject femaleRightLowerArmObject;
    [HideInInspector] public GameObject[] femaleRightLowerArms;
    public GameObject femaleRightHandObject;
    [HideInInspector] public GameObject[] femaleRightHands;
    public GameObject femaleLeftUpperArmObject;
    [HideInInspector] public GameObject[] femaleLeftUpperArms;
    public GameObject femaleLeftLowerArmObject;
    [HideInInspector] public GameObject[] femaleLeftLowerArms;
    public GameObject femaleLeftHandObject;
    [HideInInspector] public GameObject[] femaleLeftHands;
    public GameObject femaleHipsObject;
    [HideInInspector] public GameObject[] femaleHips;
    public GameObject femaleRightLegObject;
    [HideInInspector] public GameObject[] femaleRightLegs;
    public GameObject femaleLeftLegObject;
    [HideInInspector] public GameObject[] femaleLeftLegs;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();

        InitializeWeaponSlot();


        // UNISEX
        #region
        // HATS
        List<GameObject> hatsList = new List<GameObject>();

        foreach (Transform child in hatsObject.transform)
        {
            hatsList.Add(child.gameObject);
        }

        hats = hatsList.ToArray();

        // HOODS
        List<GameObject> hoodsList = new List<GameObject>();

        foreach (Transform child in hoodsOject.transform)
        {
            hoodsList.Add(child.gameObject);
        }

        hoods = hoodsList.ToArray();


        // FACE COVER
        List<GameObject> faceCoverList = new List<GameObject>();

        foreach (Transform child in faceCoversOject.transform)
        {
            faceCoverList.Add(child.gameObject);
        }

        faceCovers = faceCoverList.ToArray();

        // HELMET ACCESSORIES
        List<GameObject> helmetAccessoriesList = new List<GameObject>();

        foreach (Transform child in helmetAccessoriesObject.transform)
        {
            helmetAccessoriesList.Add(child.gameObject);
        }

        helmetAccessories = helmetAccessoriesList.ToArray();


        // BACK ACCESSORIES

        List<GameObject> backAccessoriesList = new List<GameObject>();

        foreach (Transform child in backAccessoriesObject.transform)
        {
            backAccessoriesList.Add(child.gameObject);
        }

        backAccessories = backAccessoriesList.ToArray();


        // HIP ACCESSORIES

        List<GameObject> hipAccessoriesList = new List<GameObject>();

        foreach (Transform child in hipAccessoriesObject.transform)
        {
            hipAccessoriesList.Add(child.gameObject);
        }

        hipAccessories = hipAccessoriesList.ToArray();


        // RIGHT SHOULDER

        List<GameObject> rightShoulderList = new List<GameObject>();

        foreach (Transform child in rightShoulderObject.transform)
        {
            rightShoulderList.Add(child.gameObject);
        }

        rightShoulder = rightShoulderList.ToArray();

        // LEFT SHOULDER

        List<GameObject> leftShoulderList = new List<GameObject>();

        foreach (Transform child in leftShoulderObject.transform)
        {
            leftShoulderList.Add(child.gameObject);
        }

        leftShoulder = leftShoulderList.ToArray();

        // RIGHT ELBOW

        List<GameObject> rightElbowList = new List<GameObject>();

        foreach (Transform child in rightElbowObject.transform)
        {
            rightElbowList.Add(child.gameObject);
        }

        rightElbow = rightElbowList.ToArray();

        // LEFT ELBOW

        List<GameObject> leftElbowList = new List<GameObject>();

        foreach (Transform child in leftElbowObject.transform)
        {
            leftElbowList.Add(child.gameObject);
        }

        leftElbow = leftElbowList.ToArray();

        // RIGHT KNEE

        List<GameObject> rightKneeList = new List<GameObject>();

        foreach (Transform child in rightKneeObject.transform)
        {
            rightKneeList.Add(child.gameObject);
        }

        rightKnee = rightKneeList.ToArray();

        // LEFT KNEE

        List<GameObject> leftKneeList = new List<GameObject>();

        foreach (Transform child in leftKneeObject.transform)
        {
            leftKneeList.Add(child.gameObject);
        }

        leftKnee = leftKneeList.ToArray();
        #endregion
        // MALE
        #region
        // MALE FULL HELMET
        List<GameObject> maleFullHelmetsList = new List<GameObject>();

        foreach (Transform child in maleFullHelmetObject.transform)
        {
            maleFullHelmetsList.Add(child.gameObject);
        }

        maleHeadFullHelmets = maleFullHelmetsList.ToArray();


        // MALE FULL BODY
        List<GameObject> maleBodiesList = new List<GameObject>();

        foreach (Transform child in maleFullBodyObject.transform)
        {
            maleBodiesList.Add(child.gameObject);
        }

        maleBodies = maleBodiesList.ToArray();

        // MALE RIGHT UPPER ARM
        List<GameObject> maleRightUpperArmList = new List<GameObject>();

        foreach (Transform child in maleRightUpperArmObject.transform)
        {
            maleRightUpperArmList.Add(child.gameObject);
        }

        maleRightUpperArms = maleRightUpperArmList.ToArray();

        // MALE LEFT UPPER ARM

        List<GameObject> maleLeftUpperArmList = new List<GameObject>();

        foreach (Transform child in maleLeftUpperArmObject.transform)
        {
            maleLeftUpperArmList.Add(child.gameObject);
        }

        maleLeftUpperArms = maleLeftUpperArmList.ToArray();

        // MALE RIGHT LOWER ARM
        List<GameObject> maleRightLowerArmList = new List<GameObject>();

        foreach (Transform child in maleRightLowerArmObject.transform)
        {
            maleRightLowerArmList.Add(child.gameObject);
        }

        maleRightLowerArms = maleRightLowerArmList.ToArray();

        // MALE LEFT LOWER ARM
        List<GameObject> maleLeftLowerArmList = new List<GameObject>();

        foreach (Transform child in maleLeftLowerArmObject.transform)
        {
            maleLeftLowerArmList.Add(child.gameObject);
        }

        maleLeftLowerArms = maleLeftLowerArmList.ToArray();

        // MALE RIGHT HAND
        List<GameObject> maleRightHandList = new List<GameObject>();

        foreach (Transform child in maleRightHandObject.transform)
        {
            maleRightHandList.Add(child.gameObject);
        }

        maleRightHands = maleRightHandList.ToArray();

        // MALE LEFT HAND
        List<GameObject> maleLeftHandList = new List<GameObject>();

        foreach (Transform child in maleLeftHandObject.transform)
        {
            maleLeftHandList.Add(child.gameObject);
        }

        maleLeftHands = maleLeftHandList.ToArray();

        // MALE HIPS

        List<GameObject> maleHipsList = new List<GameObject>();

        foreach (Transform child in maleHipsObject.transform)
        {
            maleHipsList.Add(child.gameObject);
        }

        maleHips = maleHipsList.ToArray();

        // MALE RIGHT LEG 

        List<GameObject> maleRightLegList = new List<GameObject>();

        foreach (Transform child in maleRightLegObject.transform)
        {
            maleRightLegList.Add(child.gameObject);
        }

        maleRightLegs = maleRightLegList.ToArray();

        // MALE LEFT LEG 

        List<GameObject> maleLeftLegList = new List<GameObject>();

        foreach (Transform child in maleLeftLegObject.transform)
        {
            maleLeftLegList.Add(child.gameObject);
        }

        maleLeftLegs = maleLeftLegList.ToArray();

        #endregion

        // FEMALE
        #region
        // MALE FULL HELMET
        List<GameObject> femaleFullHelmetsList = new List<GameObject>();

        foreach (Transform child in femaleFullHelmetObject.transform)
        {
            femaleFullHelmetsList.Add(child.gameObject);
        }

        femaleHeadFullHelmets = femaleFullHelmetsList.ToArray();


        // MALE FULL BODY
        List<GameObject> femaleBodiesList = new List<GameObject>();

        foreach (Transform child in femaleFullBodyObject.transform)
        {
            femaleBodiesList.Add(child.gameObject);
        }

        femaleBodies = femaleBodiesList.ToArray();

        // MALE RIGHT UPPER ARM
        List<GameObject> femaleRightUpperArmList = new List<GameObject>();

        foreach (Transform child in femaleRightUpperArmObject.transform)
        {
            femaleRightUpperArmList.Add(child.gameObject);
        }

        femaleRightUpperArms = femaleRightUpperArmList.ToArray();

        // MALE LEFT UPPER ARM

        List<GameObject> femaleLeftUpperArmList = new List<GameObject>();

        foreach (Transform child in femaleLeftUpperArmObject.transform)
        {
            femaleLeftUpperArmList.Add(child.gameObject);
        }

        femaleLeftUpperArms = femaleLeftUpperArmList.ToArray();

        // MALE RIGHT LOWER ARM
        List<GameObject> femaleRightLowerArmList = new List<GameObject>();

        foreach (Transform child in femaleRightLowerArmObject.transform)
        {
            femaleRightLowerArmList.Add(child.gameObject);
        }

        femaleRightLowerArms = femaleRightLowerArmList.ToArray();

        // MALE LEFT LOWER ARM
        List<GameObject> femaleLeftLowerArmList = new List<GameObject>();

        foreach (Transform child in femaleLeftLowerArmObject.transform)
        {
            femaleLeftLowerArmList.Add(child.gameObject);
        }

        femaleLeftLowerArms = femaleLeftLowerArmList.ToArray();

        // MALE RIGHT HAND
        List<GameObject> femaleRightHandList = new List<GameObject>();

        foreach (Transform child in femaleRightHandObject.transform)
        {
            femaleRightHandList.Add(child.gameObject);
        }

        femaleRightHands = femaleRightHandList.ToArray();

        // MALE LEFT HAND
        List<GameObject> femaleLeftHandList = new List<GameObject>();

        foreach (Transform child in femaleLeftHandObject.transform)
        {
            femaleLeftHandList.Add(child.gameObject);
        }

        femaleLeftHands = femaleLeftHandList.ToArray();

        // MALE HIPS

        List<GameObject> femaleHipsList = new List<GameObject>();

        foreach (Transform child in femaleHipsObject.transform)
        {
            femaleHipsList.Add(child.gameObject);
        }

        femaleHips = femaleHipsList.ToArray();

        // MALE RIGHT LEG 

        List<GameObject> femaleRightLegList = new List<GameObject>();

        foreach (Transform child in femaleRightLegObject.transform)
        {
            femaleRightLegList.Add(child.gameObject);
        }

        femaleRightLegs = femaleRightLegList.ToArray();

        // MALE LEFT LEG 

        List<GameObject> femaleLeftLegList = new List<GameObject>();

        foreach (Transform child in femaleLeftLegObject.transform)
        {
            femaleLeftLegList.Add(child.gameObject);
        }

        femaleLeftLegs = femaleLeftLegList.ToArray();

        #endregion

    }


    protected override void Start()
    {
        base.Start();

        LoadWeaponsOnBothHands();

    }
    public void EquipArmor()
    {

        LoadHeadEquipment(player.playerInventoryManager.headEquipment);
        LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
        LoadHandEquipment(player.playerInventoryManager.handEquipment);
        LoadLegEquipment(player.playerInventoryManager.legEquipment);
    }

    private void InitializeArmorModels()
    {
      
    }

    //QUICK SLOTS
    public void SwitchQuickSlotItem()
    {
        if (!player.IsOwner)
            return;


        QuickSlotItem selecteItem = null;

        player.playerInventoryManager.quickSlotItemIndex += 1;

        if (player.playerInventoryManager.quickSlotItemIndex < 0 || player.playerInventoryManager.quickSlotItemIndex > 2)
        {
            player.playerInventoryManager.quickSlotItemIndex = 0;

            float itemCount = 0;
            QuickSlotItem firstITem = null;
            int firstItemPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlot.Length; i++)
            {
                if (player.playerInventoryManager.quickSlotItemsInQuickSlot[i] != null)
                {
                    itemCount += 1;
                    if (firstITem == null)
                    {
                        firstITem = player.playerInventoryManager.quickSlotItemsInQuickSlot[i];
                        firstItemPosition = i;
                    }
                }
            }

            if (itemCount <= 1)
            {
                player.playerInventoryManager.quickSlotItemIndex = -1;
                selecteItem = null;
                player.playerNetWorkManager.currentQuickSlotItemID.Value =-1;
            }
            else
            {
                player.playerInventoryManager.quickSlotItemIndex = firstItemPosition;
                player.playerNetWorkManager.currentQuickSlotItemID.Value = firstITem.itemID;
            }

            return;
        }

        if (player.playerInventoryManager.quickSlotItemsInQuickSlot[player.playerInventoryManager.quickSlotItemIndex] != null)
        {
            selecteItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[player.playerInventoryManager.quickSlotItemIndex];

            player.playerNetWorkManager.currentQuickSlotItemID.Value =
                player.playerInventoryManager.quickSlotItemsInQuickSlot[player.playerInventoryManager.quickSlotItemIndex].itemID;
        }
        else
        {
            player.playerNetWorkManager.currentQuickSlotItemID.Value = -1;

        }


        if (selecteItem == null && player.playerInventoryManager.quickSlotItemIndex <= 2)
        {
            SwitchQuickSlotItem();
        }
    }

    // EQUIPMENT
    public void LoadHeadEquipment(HeadEquipmentItem equipment)
    {
        // 1. UNLOAD OLD HEAD EQUIPMENT MODELS (IF ANY)
        UnloadHeadEquipmentModels();
        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if(equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.headEquipmentID.Value = -1;

            player.playerInventoryManager.headEquipment = null;
            return;
        }
        // 4. SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.headEquipment = equipment;

        // 5. IF YOU NEED TO CHECK FOR HEAD EQUIPMENT TYPE TO DISABLE CERTAIN BODY FEATURES

        switch (equipment.headEquipmentType)
        {
            case HeadEquipmentType.FullHelmet:
                player.playerBodyManager.DisableHair();
                player.playerBodyManager.DisableHead();
                break;
            case HeadEquipmentType.FaceCover:
                player.playerBodyManager.DisableFacialHair();
                break;
            case HeadEquipmentType.Hood:
                player.playerBodyManager.DisableHair();
                break;
            case HeadEquipmentType.Hat:
                break;
            default:
                break;
        }

        // 6. LOAD HEAD EQUIPMENT MODELS
        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetWorkManager.isMale.Value);
        }
  

        player.playerStatManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetWorkManager.headEquipmentID.Value = equipment.itemID;

    }

    private void UnloadHeadEquipmentModels()
    {
        foreach (var model in maleHeadFullHelmets)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleHeadFullHelmets)
        {
            model.SetActive(false);
        }

        foreach (var model in hats)
        {
            model.SetActive(false);
        }

        foreach (var model in faceCovers)
        {
            model.SetActive(false);
        }

        foreach (var model in hoods)
        {
            model.SetActive(false);
        }

        foreach (var model in helmetAccessories)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableHead();
        player.playerBodyManager.EnableHair();
    }

    public void LoadBodyEquipment(BodyEquipmentItem equipment)
    {
        UnloadBodyEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.bodyEquipmentID.Value = -1;

            player.playerInventoryManager.bodyEquipment = null;
            return;
        }
        // 4. SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.bodyEquipment = equipment;

        // 5. IF YOU NEED TO CHECK FOR HEAD EQUIPMENT TYPE TO DISABLE CERTAIN BODY FEATURES
        player.playerBodyManager.DisableBody();

        // 6. LOAD HEAD EQUIPMENT MODELS
        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetWorkManager.isMale.Value);
        }


        player.playerStatManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetWorkManager.bodyEquipmentID .Value = equipment.itemID;

    }

    private void UnloadBodyEquipmentModels()
    {
        foreach (var model in rightShoulder)
        {
            model.SetActive(false);
        }

        foreach (var model in rightElbow)
        {
            model.SetActive(false);
        }

        foreach (var model in leftShoulder)
        {
            model.SetActive(false);
        }

        foreach (var model in leftElbow)
        {
            model.SetActive(false);
        }

        foreach (var model in backAccessories)
        {
            model.SetActive(false);
        }

        // MALE

        foreach (var model in maleBodies)
        {
            model.SetActive(false);
        }

        foreach (var model in maleRightUpperArms)
        {
            model.SetActive(false);
        }

        foreach (var model in maleLeftUpperArms)
        {
            model.SetActive(false);
        }

        // FEMALE

        foreach (var model in femaleBodies)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleRightUpperArms)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleLeftUpperArms)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableBody();
    }

    public void LoadHandEquipment(HandEquipmentItem equipment)
    {
        UnloadHandEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.handEquipmentID.Value = -1;

            player.playerInventoryManager.handEquipment = null;
            return;
        }
        // 4. SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.handEquipment = equipment;

        // 5. IF YOU NEED TO CHECK FOR HEAD EQUIPMENT TYPE TO DISABLE CERTAIN BODY FEATURES
        player.playerBodyManager.DisableArms();


        // 6. LOAD HEAD EQUIPMENT MODELS
        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetWorkManager.isMale.Value);
        }


        player.playerStatManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetWorkManager.handEquipmentID.Value = equipment.itemID;


    }

    private void UnloadHandEquipmentModels()
    {
        foreach (var model in maleRightLowerArms)
        {
            model.SetActive(false);
        }

        foreach (var model in maleLeftLowerArms)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleRightLowerArms)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleLeftLowerArms)
        {
            model.SetActive(false);
        }

        foreach (var model in maleLeftHands)
        {
            model.SetActive(false);
        }

        foreach (var model in maleRightHands)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleRightHands)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleLeftHands)
        {
            model.SetActive(false);
        }




        player.playerBodyManager.EnableArms();
    }

    public void LoadLegEquipment(LegEquipmentItem equipment)
    {
        UnloadLegEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.legEquipmentID.Value = -1;

            player.playerInventoryManager.legEquipment = null;
            return;
        }
        // 4. SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.legEquipment = equipment;

        // 5. IF YOU NEED TO CHECK FOR HEAD EQUIPMENT TYPE TO DISABLE CERTAIN BODY FEATURES
        player.playerBodyManager.DisableLowerBody();


        // 6. LOAD HEAD EQUIPMENT MODELS
        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetWorkManager.isMale.Value);
        }


        player.playerStatManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetWorkManager.legEquipmentID.Value = equipment.itemID;

    }

    private void UnloadLegEquipmentModels()
    {
        foreach (var model in maleLeftLegs)
        {
            model.SetActive(false);
        }

        foreach (var model in maleRightLegs)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleLeftLegs)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleRightLegs)
        {
            model.SetActive(false);
        }

        foreach (var model in maleHips)
        {
            model.SetActive(false);
        }


        foreach (var model in femaleHips)
        {
            model.SetActive(false);
        }


        foreach (var model in leftKnee)
        {
            model.SetActive(false);
        }


        foreach (var model in rightKnee)
        {
            model.SetActive(false);
        }

        foreach (var model in hipAccessories)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableLowerBody();

    }

    private void InitializeWeaponSlot()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
            {
                leftHandWeaponSlot = weaponSlot;

            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
            {
                leftHandShieldSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    // PROJECTILE
    public void LoadMainProjectileEquipment(RangedProjectileItem equipment)
    {
 
        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.mainProjectileID.Value = -1;

            player.playerInventoryManager.mainProjectile = null;
            return;
        }
        // SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.mainProjectile = equipment;


        if (player.IsOwner)
            player.playerNetWorkManager.mainProjectileID.Value = equipment.itemID;

    }

    public void LoadSecondaryProjectileEquipment(RangedProjectileItem equipment)
    {

        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.secondaryProjectileID.Value = -1;

            player.playerInventoryManager.secondaryProjectile = null;
            return;
        }
        // SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.secondaryProjectile = equipment;


        if (player.IsOwner)
            player.playerNetWorkManager.secondaryProjectileID.Value = equipment.itemID;

    }

    // QUICK SLOT
    public void LoadQuickSlotEquipment(QuickSlotItem equipment)
    {

        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetWorkManager.currentQuickSlotItemID.Value = -1;

            player.playerInventoryManager.currentQuickSlotItem = null;
            return;
        }
        // SET CURRENT HEAD EQUIPMENT IN PLAYER INVENTORY TO EQUIPMENT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.currentQuickSlotItem = equipment;


        if (player.IsOwner)
            player.playerNetWorkManager.currentQuickSlotItemID.Value = equipment.itemID;

    }

    //RIGHT WEAPON
    public void SwitchRightWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerNetWorkManager.isTwoHandingWeapon.Value = false;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false,false,true,true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.rightHandWeaponIndex += 1;

        if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex >2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponInRightHandSlots)
        {
            if (player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];


                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if(selectedWeapon ==  null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
      
    }

    public void LoadRightWeapon()
    {
        if(player.playerInventoryManager.currentRightHandWeapon != null)
        {
            rightHandWeaponSlot.UnloadWeapon();
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel); 
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
        }
    }


    //LEFT WEAPON
    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerNetWorkManager.isTwoHandingWeapon.Value = false;


        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.leftHandWeaponIndex += 1;

        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponInLeftHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponInLeftHandSlots)
        {
            if (player.playerInventoryManager.weaponInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                player.playerNetWorkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }

    public void LoadLeftWeapon()
    {

        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            if(leftHandWeaponSlot.currentWeaponModel !=null)
                leftHandWeaponSlot.UnloadWeapon();

            if (leftHandShieldSlot.currentWeaponModel != null)
                leftHandShieldSlot.UnloadWeapon();

            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

            switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
            {
                case WeaponModelType.Weapon:
                    leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                    break;
                case WeaponModelType.Shield:
                    leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                    break;
                default:
                    break;
            }

         
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    //TWO HAND
    public void UnTwoHandWeapon()
    {
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);


        //LEFT HAND
        if(player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
        {
            leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
        }
        else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
        {
            leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
        }
        //RIGHT HAND
        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void TwoHandRightWeapon()
    {
        //CHECK FOR UNTWOHANDABLE ITEM (LIKE UNARMAED) IF WE ARE ATTEMPTING TO  TWO HAND UNARMED , RETURN
        if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.instance.unarmedWeapon)
        {
            // IF  WE ARE RETURNING AND NOT TWO HANDING THE WEAPON , RESET BOOL STATUS
            if(player.IsOwner)
            {
                player.playerNetWorkManager.isTwoHandingRightWeapon.Value = false;
                player.playerNetWorkManager.isTwoHandingWeapon.Value = false;
            }

            return;
        }
        //UPDATE ANIMATION
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

        // PLACE THE NON-TWO HANDED WEAPON MODEL IN  THE BACK SLOT OR HIP SLOT
        backSlot.PlaceWeaponModelInUnequippedSot(leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

        // PLACE THE TWO HANDED WEAPON MODEL IN THE MAIN RH
        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);


        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void TwoHandLeftWeapon()
    {
        //CHECK FOR UNTWOHANDABLE ITEM (LIKE UNARMAED) IF WE ARE ATTEMPTING TO  TWO HAND UNARMED , RETURN
        if (player.playerInventoryManager.currentLeftHandWeapon == WorldItemDatabase.instance.unarmedWeapon)
        {
            // IF  WE ARE RETURNING AND NOT TWO HANDING THE WEAPON , RESET BOOL STATUS
            if (player.IsOwner)
            {
                player.playerNetWorkManager.isTwoHandingLeftWeapon.Value = false;
                player.playerNetWorkManager.isTwoHandingWeapon.Value = false;
            }

            return;
        }
        //UPDATE ANIMATION
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);

        // PLACE THE NON-TWO HANDED WEAPON MODEL IN  THE BACK SLOT OR HIP SLOT
        backSlot.PlaceWeaponModelInUnequippedSot(rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);

        // PLACE THE TWO HANDED WEAPON MODEL IN THE MAIN RH
        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);


        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon); 
    }

    // DAMAGE COLLIDER
    public void OpenDamageCollider()
    {
        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
        }
        else if (player.playerNetWorkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));

        }

        // PLAY SFX
    }

    public void CloseDamageCollider()
    {
        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        else if (player.playerNetWorkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();

        }

    }

    public void UnHideWeapons()
    {
        if (player.playerEquipmentManager.rightHandWeaponModel != null)
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(true);


        if (player.playerEquipmentManager.leftHandWeaponModel != null)
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(true);
    }
}
