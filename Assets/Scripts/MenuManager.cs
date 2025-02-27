using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;       
    public GameObject settingsPanel;   
    public Slider volumeSlider;        
    public Toggle musicToggle;         
    public Dropdown resolutionDropdown;

    private AudioSource backgroundMusic;
    private Resolution[] resolutions;

    void Start()
    {
        // This ensures settings panel is hidden initially
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);

        // Initialize volume
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            AudioListener.volume = savedVolume;
            volumeSlider.value = savedVolume;
        }

        
        if (PlayerPrefs.HasKey("Music"))
        {
            bool isMusicOn = PlayerPrefs.GetInt("Music") == 1;
            musicToggle.isOn = isMusicOn;
            if (backgroundMusic != null)
                backgroundMusic.mute = !isMusicOn;
        }

        // Populate resolution dropdown
        // Get available resolutions, set current resolution, and add options to dropdown
        // Save selected resolution in PlayerPrefs
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionDropdown.options.Add(new Dropdown.OptionData(option));

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listeners
        // SetVolume, SetMusic, and SetResolution are called when the respective UI elements change
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicToggle.onValueChanged.AddListener(SetMusic);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("TicTacToe"); // Transition to TicTacToe scene
    }

    public void OpenSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void SetMusic(bool isMusicOn)
    {
        if (backgroundMusic != null)
            backgroundMusic.mute = !isMusicOn;

        PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);
    }

    private void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
