﻿using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using ResetCore.Data.GameDatas.Xml;

namespace ResetCore.Data
{
    public class Source2Xml : ISource2
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

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Root");
            xDoc.Add(root);

            List<Dictionary<string, string>> rows = exReader.GetRows();
            for (int i = 0; i < rows.Count; i++)
            {
                XElement item = new XElement("item");
                root.Add(item);
                foreach (KeyValuePair<string, string> pair in rows[i])
                {
                    item.Add(new XElement(pair.Key, pair.Value));
                }

            }

            if (outputPath == null)
            {
                outputPath = PathConfig.GetLocalGameDataPath(PathConfig.DataType.Xml) 
                    + Path.GetFileNameWithoutExtension(reader.currentDataTypeName) + XmlData.m_fileExtention;
            }
            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            }

            xDoc.Save(outputPath);
            AssetDatabase.Refresh();
        }

        public void GenCS(IDataReadable reader)
        {
            string className = reader.currentDataTypeName;
            DataClassesGener.CreateNewClass(className, typeof(XmlData), reader.fieldDict);
        }
       
    }

}
