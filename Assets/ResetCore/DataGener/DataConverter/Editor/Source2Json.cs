﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using LitJson;
using System.IO;
using ResetCore.Json;
using ResetCore.Data;

namespace ResetCore.Data
{
    public class Source2Json :ISource2
    {
        public DataType dataType
        {
            get
            {
                return DataType.Normal;
            }
        }
        public void GenData(IDataReadable reader, string outputPath = null)
        {

            IDataReadable exReader = reader;

            List<Dictionary<string, string>> rows = exReader.GetRows();

            JsonData data = new JsonData();
            string arrayString = JsonMapper.ToJson(rows);
            Debug.Log(arrayString);

            JsonData jsonArray = JsonMapper.ToObject(arrayString);
            data[reader.currentDataTypeName] = jsonArray;


            if (outputPath == null)
            {
                outputPath = PathConfig.GetLocalGameDataPath(PathConfig.DataType.Json)
                    + Path.GetFileNameWithoutExtension(reader.currentDataTypeName) + Data.GameDatas.Json.JsonData.m_fileExtention;
            }
            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            }

            data.Save(outputPath);

            AssetDatabase.Refresh();
        }

        public void GenCS(IDataReadable reader)
        {
            string className = reader.currentDataTypeName;
            DataClassesGener.CreateNewClass(className, typeof(Data.GameDatas.Json.JsonData), reader.fieldDict);
        }
    }

}
