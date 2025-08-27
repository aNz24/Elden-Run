using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [HideInInspector] public PlayerManager localPlayer;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager  playerUIHudManager;
    [HideInInspector] public PlayerUIPopUpManager  playerUIPopUpManager;
    [HideInInspector] public PlayerUICharacterMenuManager  playerUICharacterMenuManager;
    [HideInInspector] public PlayerUIEquipmentManager  playerUIEquipmentManager;
    [HideInInspector] public PlayerUISiteOfGarceManager  playerUISiteOfGarceManager;
    [HideInInspector] public PlayerUITeleportLocationManager playerUITeleportLocationManager;
    [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;
    [HideInInspector] public PlayerUILevelUpManager playerUILevelUpManager;

    [Header("UI FLags")]
    public bool menuWindowIsOpen=false;
    public bool popUpWindowIsOpen=false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
        playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
        playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
        playerUISiteOfGarceManager = GetComponentInChildren<PlayerUISiteOfGarceManager>();
        playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
        playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
        playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();


    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //
            NetworkManager.Singleton.Shutdown();

            NetworkManager.Singleton.StartClient();
        }
    }

    public void CloseAllMenuWindow()
    {
        playerUICharacterMenuManager.CloseMenuAfterFixedFrame();    
        playerUIEquipmentManager.CloseMenuAfterFixedFrame();
        playerUISiteOfGarceManager.CloseMenuAfterFixedFrame();
        playerUITeleportLocationManager.CloseMenuAfterFixedFrame();
        playerUILevelUpManager.CloseMenuAfterFixedFrame();
    }
}
