using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Xml.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int bestScore;
    public string bestScoreName;    
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TMP_InputField nameField;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        name = nameField.text;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }    

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScoreName;        
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestScoreName = bestScoreName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {        
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestScore = data.bestScore;
            bestScoreName = data.bestScoreName;
            
            bestScoreText.SetText(bestScoreName + ": " + bestScore.ToString());
        }
    }
}
