using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Tên Scene Game của m (Phải gõ đúng y xì)")]
    public string gameSceneName = "Level 2"; 

    // Gắn vào nút Play (Chơi ván mới hoàn toàn)
    public void PlayNewGame()
    {
        // Xóa trí nhớ save cũ và ép level về 0
        PlayerPrefs.SetInt("SavedLevel", 0); 
        CemeteryHouseInteract.currentLevel = 0; 

        // Load vào Scene game
        SceneManager.LoadScene(gameSceneName);
    }

    // Gắn vào nút Resume / Load Game ở ngoài Main Menu
    public void ContinueGame()
    {
        // Kiểm tra xem máy có file save cũ không, có thì bốc ra xài
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            CemeteryHouseInteract.currentLevel = PlayerPrefs.GetInt("SavedLevel");
        }
        else
        {
            CemeteryHouseInteract.currentLevel = 0;
        }
        SceneManager.LoadScene(gameSceneName);
    }

    // Gắn vào nút Quit
    public void QuitGame()
    {
        Debug.Log("Game đã tắt (Chỉ hoạt động khi build ra file exe)");
        Application.Quit();
    }
}