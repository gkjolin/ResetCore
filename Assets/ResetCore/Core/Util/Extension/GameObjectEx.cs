﻿using UnityEngine;
using System.Collections;

namespace ResetCore.Util
{
    public static class GameObjectEx
    {

        /// <summary>
        /// 重置物体的Transform
        /// </summary>
        /// <param name="go"></param>
        public static void ResetTransform(this GameObject go)
        {
            go.transform.position = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }

        /// <summary>
        /// 虫植物体的Transfrom
        /// </summary>
        /// <param name="tran"></param>
        public static void ResetTransform(this Transform tran)
        {
            tran.position = Vector3.zero;
            tran.eulerAngles = Vector3.zero;
            tran.localScale = Vector3.one;
            tran.gameObject.SetActive(true);
        }

        public static Vector3 NewRotateAround(this Transform tran, Vector3 pos, Vector3 euler)
        {
            Quaternion rotation = Quaternion.Euler(euler) * tran.localRotation;
            Vector3 newPosition = rotation * (tran.position - pos);
            return newPosition;
        }

        /// <summary>
        /// 获取所有的callback
        /// </summary>
        /// <param name="go"></param>
        public static GameObjectCallBack GetCallbacks(this GameObject go)
        {
            return go.GetOrCreateComponent<GameObjectCallBack>();
        }

        /// <summary>
        /// 查找指定祖先节点下的对象
        /// </summary>
        /// <param name="ancestorName">祖先节点对象</param>
        /// <param name="name">对象名称</param>
        /// <returns>对象</returns>
        public static GameObject FindObjectInChild(this GameObject node, string name)
        {
            Transform ancestorNode = node.transform;
            for (int i = 0; i < ancestorNode.childCount; i++)
            {
                GameObject tmp;
                if (ancestorNode.GetChild(i).name == name)
                    return ancestorNode.GetChild(i).gameObject;
                else
                    tmp = FindObjectInChild(ancestorNode.GetChild(i).gameObject, name);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }

        /// <summary>
        /// 查找指定祖先节点下的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetComponentInChild<T>(this GameObject node) where T : Component
        {
            Transform ancestorNode = node.transform;
            for (int i = 0; i < ancestorNode.childCount; i++)
            {
                T tmp;
                tmp = ancestorNode.GetChild(i).gameObject.GetComponent<T>();
                if (tmp != null)
                    return tmp;
                else
                    tmp = GetComponentInChild<T>(ancestorNode.GetChild(i).gameObject);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }

    }

}
