using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//todo
//, use encryption / hash AES, SHA1  . etc.
//yse nyktub=oke bacjyos
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData data;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("There is already a SaveManager in the Game. Removing SaveManager from " + name);
            Destroy(this);
        }
    }
    public void SavePlayerpref(string name)
    {

    }
    public void LoadPlayerpref(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {

        }
        else
        {
            data = new SaveData();
            SavePlayerpref(name);
        }
    }
    public void JsonSave(SaveData savedata,string filename,string filenameextention = "json")
    {
        string saveStatePath = Path.Combine(Application.persistentDataPath, filename + "." + filenameextention);
        File.WriteAllText(saveStatePath, JsonUtility.ToJson(savedata, true));
    }
    public void Save(SaveData savedata, string filename, string filenameextention = "dat")
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filename + "." + filenameextention);

        bf.Serialize(file, savedata);
        file.Close();
    }
    public SaveData Load(string filename, string filenameextention = "dat")
    { 
        if(File.Exists(Application.persistentDataPath + filename + "." + filenameextention))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename + "." + filenameextention, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            Debug.Log("Trying to Load non existing save file");
            return null;
        }
    }

    public class SaveData
    {

    }
}