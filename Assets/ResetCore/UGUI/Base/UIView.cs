﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using ResetCore.Util;
using System.Reflection;

namespace ResetCore.UGUI
{
    public class UIView
    {
        
        //前缀
        public static readonly string genableSign = "g-";
        //自定义UIView命名空间
        public static readonly string uiViewNameSpace = "ResetCore.UGUI.Class";
        //UIView储存位置
        public static readonly string uiViewScriptPath = "Scripts/UI/View";

        private Dictionary<string, Component> comDict;

        private MonoBehaviour rootComponent;

        private Type rootType;

        //有效的组件
        public static readonly List<Type> uiCompTypeList = new List<Type>()
        {
            typeof(Image),
            typeof(Button),
            typeof(Text),
        };

        public static readonly string goName = "go";
        public static readonly Dictionary<Type, string> comNameDict = new Dictionary<Type, string>()
        {
            { typeof(Image), "img" },
            { typeof(Button), "btn" },
            { typeof(Text), "txt" },
        };

        //初始化
        public void Init(MonoBehaviour root)
        {
            rootComponent = root;
            rootType = root.GetType();
            comDict = new Dictionary<string, Component>();
            GetUIComponents(root);
        }


        /// <summary>
        /// 获取UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetUI<T>(string name) where T : Component
        {
            if (comDict.ContainsKey(name))
            {
                return (comDict[name]) as T;
            }
            else
            {
                Debug.logger.LogError("组件获取错误" , name + "不存在");
                return null;
            }
        }

        /// <summary>
        /// 直接通过名字获取（用到反射，不要再Update中进行调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetUIByName<T>(string name) where T : Component
        {
            string finalName = name + "_" + typeof(T).Name;
            if (comDict.ContainsKey(finalName))
            {
                return (comDict[finalName]) as T;
            }
            else
            {
                Debug.logger.LogError("组件获取错误", finalName + "不存在");
                return null;
            }
        }
        

        /// <summary>
        /// 将必要组件加入
        /// </summary>
        /// <param name="go"></param>
        private void GetUIComponents(MonoBehaviour root)
        {
            Type rootType = root.GetType();
            Type uiViewType = this.GetType();

            root.gameObject.transform.DoToSelfAndAllChildren((tran) =>
            {
                GameObject go = tran.gameObject;
                if (!go.name.StartsWith(genableSign))
                {
                    return;
                }
                var coms = go.GetComponents<Component>();
                string comGoName = go.name.Replace(genableSign, string.Empty);
                //遍历所有组件
                foreach (var com in coms)
                {
                    Type comType = com.GetType();
                    if (uiCompTypeList.Contains(comType) && com.gameObject.name.StartsWith(genableSign))
                    {
                        

                        //加入键值
                        StringBuilder builder = new StringBuilder();
                        string name = builder.Append(comNameDict[comType]).Append(comGoName).ToString();
                        if (!comDict.ContainsKey(name))
                        {
                            comDict.Add(name, com);
                        }
                        else
                        {
                            Debug.LogError("重名！" + name);
                        }

                        //生成按钮回调
                        if (com is Button)
                        {
                            Button btn = com as Button;
                            MethodInfo method = rootType.GetMethod("On" + comGoName);
                            if (method != null)
                            {
                                UIEventListener.Get(btn.gameObject).onClick = (btnGo) =>
                                {
                                    method.Invoke(rootComponent, new object[0]);
                                };
                            }
                            else
                            {
                                Debug.logger.Log("未发现函数");
                            }
                        }

                        //赋予组件值
                        PropertyInfo prop = uiViewType.GetProperty(name);
                        if (prop != null)
                        {
                            prop.SetValue(this, com, Const.EmptyArg);
                        }
                    }
                }
                //添加GameObject变量
                string goVarName = goName + comGoName;
                PropertyInfo goProp = uiViewType.GetProperty(goVarName);
                if (goProp != null)
                {
                    goProp.SetValue(this, go, Const.EmptyArg);
                }
            });
        }

       
    }

}

