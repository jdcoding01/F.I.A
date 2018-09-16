// Copyright (C) 2018, Alphi#9839
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public class integrityCheck : MonoBehaviour {
    
	void Start () {
        string path = @"C:/Program Files (x86)/Steam/steamapps/common/AdventureQuest3D/aq3d/aq3d_Data/Managed/";
        //AQ3D dlls path
        string json;
        using (StreamReader r = new StreamReader(path + "../tokens.bin"))
        {
            json = r.ReadToEnd();
        }
        dynamic arr = JsonConvert.DeserializeObject(json);
        DirectoryInfo d = new DirectoryInfo(path);
        FileInfo[] Files = d.GetFiles("*.dll");
        int integrityCheck = 0;
        int successCheck = arr.Count;
        foreach (FileInfo file in Files)
        {
            string ipath = path + file.Name;
            string hash = GetChecksum(ipath);
            if (Array.IndexOf(arr, hash) >= 0)
            {
                integrityCheck++;
            }
        }
        if (integrityCheck == successCheck)
        {
            // success all files are unmodified
        }
        else
        {
            // some files were modified!!!!
        }
    }
    void Update () {
		
	}
    private static string GetChecksum(string file)
    {
        using (FileStream stream = File.OpenRead(file))
        {
            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
    }
    private static string GetChecksumBuffered(Stream stream)
    {
        using (var bufferedStream = new BufferedStream(stream, 1024 * 32))
        {
            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(bufferedStream);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
    }
}
// Copyright (C) 2018, Alphi#9839