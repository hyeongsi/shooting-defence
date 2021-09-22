﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileManager
{
    static FileManager instance = null;

    public static FileManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FileManager();
            }

            return instance;
        }
    }

    public string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T JsonToObject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        if(!Directory.Exists(createPath))
        {
            Directory.CreateDirectory(createPath);
        }

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public T LoadJsonFile<T>(string loadPath, string fileName)
    {
        try
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();

            string jsonData = System.Text.Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
        catch(Exception)
        {
            return default;     // null
        }
    }

    public List<T> LoadCsvFile<T>(string loadPath, string fileName)
    {
        List<T> listData = new List<T>();


        return listData;
    }
}
