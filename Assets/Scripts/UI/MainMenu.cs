using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Gọi hàm này để bắt đầu game (chuyển sang màn chơi số 1)
    public void PlayGame()
    {
        // Nhớ đảm bảo Scene của màn chơi được đặt ở vị trí số 1 trong Build Settings
        SceneManager.LoadScene(1);
    }

    // Gọi hàm này để thoát game
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!");
        Application.Quit(); // Lệnh này chỉ hoạt động khi Build game ra file .exe hoặc .apk
    }
}
