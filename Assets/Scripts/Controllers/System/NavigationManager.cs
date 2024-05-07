using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NavigationManager : MonoBehaviour
{
    private string previousScene;

    void Start()
    {
        previousScene = SceneManager.GetActiveScene().name;
        //SoundManager.instance.PlaySound(false);
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadSceneAsync(sceneId);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    #region GAME MUSIC
    public void ResetSoundAndNavigate(string sceneName)
    {
        AudioManager.instance.PlayMusic("MusicTheme");
        SceneManager.LoadSceneAsync(sceneName);        
    }

    public void ToggleAndLoadMusic(string sceneName)
    {
        AudioManager.instance.ToggleMusic();
        SceneManager.LoadSceneAsync(sceneName);        
    }
    #endregion

    #region QUIZ GAME
    public void LoadQuizRandom()
    {
        int index = Random.Range(0, 2);
        PlayerPrefs.SetInt("QuizGameIndex", index);
        SceneManager.LoadSceneAsync("QuizGame");
    }

    public void LoadQuizScene(int index)
    {
        PlayerPrefs.SetInt("QuizGameIndex", index);
        SceneManager.LoadSceneAsync("QuizGame");
    }
    #endregion

    #region ACTIVITY GAME
    public void LoadActivityPicker()
    {
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        PlayerPrefs.SetString("ActivityGameSelected", buttonText);
        SceneManager.LoadSceneAsync("ActivityPicker");
    }

    public void LoadActivityScene()
    {
        var selectedGame = PlayerPrefs.GetString("ActivityGameSelected", "KnowYourLabel");
        var gameModule = selectedGame.Replace(" ", string.Empty).Replace("!", string.Empty) + "Game";
        SceneManager.LoadSceneAsync(gameModule);
    }
    #endregion

    #region LESSON PAGE
    public void LoadLessonScene(int index)
    {
        PlayerPrefs.SetInt("SelectedLessonIndex", index);
        SceneManager.LoadSceneAsync("LessonViewer");        
    }

    public void LoadWikaLessonScene(int index)
    {
        PlayerPrefs.SetInt("SelectedLessonIndex", index);
        SceneManager.LoadSceneAsync("WikaKatangianLessonViewer");
    }

    public void LoadDaigdigLessonScene(int index)
    {
        PlayerPrefs.SetInt("SelectedLessonIndex", index);
        SceneManager.LoadSceneAsync("PisikalNaDaigdigLessonViewer");
    }
    #endregion
}
