using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class MiniMap : MonoBehaviour
{
    public Transform player; // Transform của local Player
    public float checkInterval = 0.5f; // Thời gian kiểm tra lại Player (giây)

    private void Start()
    {
        // Bắt đầu tìm local Player
        StartCoroutine(FindLocalPlayer());
    }

    private IEnumerator FindLocalPlayer()
    {
        while (player == null)
        {
            // Kiểm tra xem NetworkManager đã khởi tạo và client đã kết nối chưa
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient && NetworkManager.Singleton.LocalClient != null)
            {
                // Lấy NetworkObject của local Player
                NetworkObject playerNetworkObject = NetworkManager.Singleton.LocalClient.PlayerObject;
                if (playerNetworkObject != null)
                {
                    player = playerNetworkObject.transform;
                  
                }
            }

            if (player == null)
            {
                Debug.LogWarning("Chưa tìm thấy local Player, thử lại sau " + checkInterval + " giây...");
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void LateUpdate()
    {
        if (player == null) return; // Thoát nếu chưa tìm thấy Player

        // Cập nhật vị trí minimap theo Player
        Vector3 newPos = player.position;
        newPos.y = transform.position.y; // Giữ độ cao của minimap
        transform.position = newPos;

       // transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

    }
}