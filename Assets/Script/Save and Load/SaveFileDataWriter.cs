using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter 
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // BEFORE WE CREATE A NEW FILE, WE MUST CHECK TO SEE IF ONE OF THIS CHARACTER SLOT ALREADY EXISTS (MAX 10 CHARACTER SLOTS)
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // USE TO DELETE  CHARACTER SAVE FILES
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // USED TO CREATE A SAVE FILE UPON STARTTING A NEW GAME
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        // MAKE A PATH TO SAVE THE FILE (A LOCATION ON THE MACHINE)
        string savePath = Path.Combine(saveDataDirectoryPath,saveFileName);

        try
        {
            // CREATE THE DIRECTORY THE FILE WILL BE WRITTEN TO, IF DOES NOT ALREDY EXIST
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

            // SERIALIZE THE C# GAME DATA OBJECT INTO JSON
            string dataToStore = JsonUtility.ToJson(characterData,true);

            //WRITE  THE FILE TO OUR SYSTEM
            using (FileStream stream = new FileStream (savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        } 
        catch (Exception e)
        {
            Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA , GAME NOT SAVED" + savePath + "\n" + e);
        }
    }

    // USED TO LOAD A SAVE FILE UPON LOADING A PREVIOUS GAME
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData =  null; 

        // MAKE A PATH TO LOAD THE FILE (A LOCATION ON THE MACHINE)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        if(File.Exists(loadPath))
        {
            try
            {
                string dataLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new  (stream))
                    {
                        dataLoad = reader.ReadToEnd();
                    }
                }

                // DESERIALIE THE DATA FROM JSON BACK TO UNTY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("BLANK" + e) ;
            }
        }
        return characterData;
    }

}
