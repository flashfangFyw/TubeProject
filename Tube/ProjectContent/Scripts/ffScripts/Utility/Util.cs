﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace ffDevelopmentSpace
{
    public class Util
    {
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component
        {
            return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        public static void CloseParticleLoop(GameObject gameobject)
        {
            ParticleSystem[] ps = gameobject.GetComponentsInChildren<ParticleSystem>();

            ParticleSystem particle;
            for (int i = 0; i < ps.Length; i++)
            {
                particle = ps[i];
                if (particle)
                {
                    particle.loop = false;
                }
            }
        }

        public static void CloseParticleLoopAndDestory(GameObject gameobject, float time = 3)
        {
            if (gameobject == null) return;
            CloseParticleLoop(gameobject);
            if (gameobject) GameObject.Destroy(gameobject, time);
        }

        // public static GameObject CreatElement(string assetName, Transform Parent = null, string layerName = "Default")
        // {
        //     if (assetName == "") return null;
        //     GameObject prefab = ResourceManager.GetInstance().LoadAsset(assetName, assetName);
        //     if (prefab == null)
        //     {
        //Debuger.Log("prefab is null");
        //return null;
        //     }

        //     return CreatElement(prefab, Parent, assetName, layerName);
        // }

        // public static GameObject CreatElement(string pkgName, string assetName, Transform Parent = null, string layerName = "Default")
        // {
        //     if (pkgName == "" || assetName == "") return null;
        //     GameObject prefab = ResourceManager.GetInstance().LoadAsset(pkgName, assetName);
        //     if (prefab == null)
        //     {
        //         Debuger.Log("perfab is null ,name is =" + assetName+"   pkgName="+pkgName);
        //         return null;
        //         //           print("prefab is null");
        //     }

        //     return CreatElement(prefab, Parent,assetName, layerName);
        // }
        public static GameObject CreatElement(GameObject prefab, Transform Parent = null, string assetName = "", bool worldPositionStays=true)
        {
            //GameObject prefab = ResourceManager.GetInstance().LoadAsset(pkgName, assetName);

            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            if (go == null)
            {
                Debug.Log("=====================================================go is null ");
                return null;
                //           print("prefab is null");
            }
            go.name = assetName;
            if (Parent) go.transform.SetParent(Parent, worldPositionStays);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            //go.transform.Rotate(0, 0, 0);
            Renderer[] renders = go.GetComponentsInChildren<Renderer>();
            Renderer render;//= go.GetComponentInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                render = renders[i];
                if (render)
                {
                    int nMaterialLen = render.sharedMaterials.Length;
                    for (int j = 0; j < nMaterialLen; j++)
                    {
                        render.sharedMaterials[j].shader = Shader.Find(render.sharedMaterials[j].shader.name);
                    }
                }
            }
            //renders = go.GetComponentsInChildren<Text>();
            return go;
        }
        public static GameObject CreatElement(GameObject prefab, Transform Parent = null, string assetName = "", string layerName = "Default")
        {
            //GameObject prefab = ResourceManager.GetInstance().LoadAsset(pkgName, assetName);

            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            if (go == null)
            {
                Debug.Log("=====================================================go is null ");
                return null;
                //           print("prefab is null");
            }
            go.name = assetName;
            foreach (Transform tran in go.GetComponentsInChildren<Transform>())
            {//遍历当前物体及其所有子物体
                tran.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
            go.layer = LayerMask.NameToLayer(layerName);
            if (Parent) go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            //go.transform.Rotate(0, 0, 0);
            Renderer[] renders = go.GetComponentsInChildren<Renderer>();
            Renderer render;//= go.GetComponentInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                render = renders[i];
                if (render)
                {
                    int nMaterialLen = render.sharedMaterials.Length;
                    for (int j = 0; j < nMaterialLen; j++)
                    {
                        render.sharedMaterials[j].shader = Shader.Find(render.sharedMaterials[j].shader.name);
                    }
                }
            }
            //renders = go.GetComponentsInChildren<Text>();
            return go;
        }
        public static void setMaterialShip(GameObject ship)
        {
            Renderer[] renders = ship.GetComponentsInChildren<Renderer>();
            Renderer render;//= go.GetComponentInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                render = renders[i];
                if (render)
                {
                    int nMaterialLen = render.materials.Length;//render.sharedMaterials.Length;
                    for (int j = 0; j < nMaterialLen; j++)
                    {
                        //render.sharedMaterials[j].shader = Shader.Find(render.sharedMaterials[j].shader.name);
                        if (render.materials[j].shader == Shader.Find("Custom/CustomHalfLambert"))
                        {
                            Texture tex = render.materials[j].GetTexture("_myTexture");
                            render.materials[j].shader = Shader.Find("Unlit/Texture");
                            render.materials[j].mainTexture = tex;
                        }
                        ////.SetTexture();
                        //render.sharedMaterials[j].shader = Shader.Find("Unlit/Texture");
                        //render.sharedMaterials[j].mainTexture = tex;
                    }
                }
            }
        }
        //public static Sprite CreateSprite(string pkgName, string assetName)
        //{
        //    if (pkgName == "" || assetName == "") return null;
        //    Sprite prefab = ResourceManager.GetInstance().LoadSprite(pkgName, assetName);
        //    if (prefab == null)
        //    {
        //        Debuger.LogError(assetName+"prefab is null");
        //        return null;
        //    }
        //    Sprite go = GameObject.Instantiate(prefab) as Sprite;
        //    return go;
        //}

        public static void addTextMaterials(GameObject obj)
        {
            Text[] renders = obj.GetComponentsInChildren<Text>();
            Text render;//= go.GetComponentInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                render = renders[i];
                if (render)
                {
                    render.material.shader = Shader.Find(render.material.shader.name);
                }
            }
        }

        public static void addMaterials(GameObject obj)
        {
            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
            Renderer render;//= go.GetComponentInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                render = renders[i];
                if (render)
                {
                    int nMaterialLen = render.sharedMaterials.Length;
                    for (int j = 0; j < nMaterialLen; j++)
                    {
                        render.sharedMaterials[j].shader = Shader.Find(render.sharedMaterials[j].shader.name);
                    }
                }
            }
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string Encode(string message)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        public static string Decode(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.GetEncoding("utf-8").GetString(bytes);
        }

        /// <summary>
        /// 判断数字
        /// </summary>
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0) return false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str[i])) { return false; }
            }
            return true;
        }
        public static bool PointInPolygon(Vector3 position, List<Vector3> pointList)
        {
            int i, j = 0;
            bool c = false;
            float testx = position.x;
            float testy = position.z;
            for (i = 0, j = 4 - 1; i < 4; j = i++)
            {
                if (((pointList[i].z > testy) != (pointList[j].z > testy)) &&
                 (testx < (pointList[j].x - pointList[i].x) * (testy - pointList[i].z) / (pointList[j].z - pointList[i].z) + pointList[i].x))
                    c = !c;
            }
            return c;
        }
        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go, float t = 0)
        {
            if (go == null) return;
            //Debuger.Log("start child num=" + go.childCount);
            int n = go.childCount;
            for (int i = n - 1; i >= 0; i--)
            {
                //Debuger.Log("child name =" + go.GetChild(i).gameObject.name);
                GameObject.Destroy(go.GetChild(i).gameObject, t);
                //Destroy(go.GetChild(i).gameObject, t);
            }
            //Debuger.Log("end child num=" + go.childCount);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                string game = Const.AppName.ToLower();
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/" + game + "/";
                }
                //return Application.dataPath + "/" + Const.AssetDirname + "/";
                return "C:/" + Const.AssetDirname + "/";
            }
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets";
                    break;

                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw";
                    break;

                default:
                    path = Application.dataPath + "/" + Const.AssetDirname;
                    break;
            }
            return path;
        }

        /// <summary>
        /// 是否是登录场景
        /// </summary>
        public static bool isLogin
        {
            get { return Application.loadedLevelName.CompareTo("login") == 0; }
        }

        /// <summary>
        /// 是否是城镇场景
        /// </summary>
        public static bool isMain
        {
            get { return Application.loadedLevelName.CompareTo("main") == 0; }
        }

        /// <summary>
        /// 判断是否是战斗场景
        /// </summary>
        public static bool isFight
        {
            get { return Application.loadedLevelName.CompareTo("fight") == 0; }
        }

        private static float time;
        public static void onTimeStart()
        {
            //		time = DateTime.UtcNow.ToFileTimeUtc ();
            time = Time.realtimeSinceStartup;
        }

        public static void onTimeEnd(string str = "")
        {
            //		long test = (DateTime.UtcNow.ToFileTimeUtc () - time) / 1000;
            float test = (Time.realtimeSinceStartup - time) * 1000;
            Debug.Log("测试经过时间(毫秒)：" + test + " [" + str + "]");
        }

        public static Color parseHexString(string strHexColor)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(strHexColor, out color))
            {
                return color;
            }
            return Color.black;
        }

        public static int GetNextRandomNumber(int index)
        {
            return random_num_buffer[index % random_num_buffer.Length];
        }

        private static int[] random_num_buffer ={
    7609, 9236, 8641, 6234, 104,  7876, 2817, 8634, 5099, 7455, 2677, 7661,
    1234, 9689, 4108, 1953, 6985, 8948, 3829, 5832, 73,   2277, 9469, 2116,
    1138, 5160, 8996, 5652, 7603, 5870, 7746, 6289, 548,  5341, 7085, 7016,
    5810, 2447, 5222, 155,  359,  3567, 4183, 4581, 5272, 2630, 8750, 9051,
    3695, 2846, 7434, 6774, 3062, 5311, 5822, 2888, 5134, 5440, 9610, 2449,
    8823, 7629, 9746, 4466, 9793, 253,  6537, 7206, 8930, 6919, 198,  881,
    3080, 7838, 2468, 223,  4853, 956,  7580, 1514, 469,  3036, 2054, 9767,
    4972, 2920, 895,  12,   775,  5302, 4429, 8852, 2626, 9013, 8160, 8401,
    2942, 35,   6129, 565,  5406, 1473, 8517, 1322, 1914, 8241, 1045, 3344,
    5877, 488,  2098, 1916, 9135, 1936, 2690, 9739, 173,  4704, 9225, 6546,
    2547, 893,  8113, 9591, 9697, 9105, 6427, 7974, 8165, 6101, 6036, 2067,
    7517, 8767, 920,  176,  5053, 3851, 8928, 6590, 8202, 5480, 4648, 2687,
    3509, 6739, 7315, 1212, 3571, 5146, 4450, 866,  9677, 7520, 6821, 4723,
    3319, 5059, 1773, 3316, 1989, 3076, 2203, 8378, 5184, 7286, 4423, 1744,
    277,  6746, 5308, 7039, 9247, 7597, 672,  266,  5621, 9232, 1935, 4369,
    1833, 2853, 1316, 2342, 9458, 6392, 3784, 3822, 7363, 217,  425,  634,
    2564, 765,  1104, 609,  8308, 5920, 6702, 9738, 6326, 3407, 7119, 4820,
    7222, 1805, 4835, 6261, 3989, 5420, 8977, 5539, 8903, 9285, 8668, 4250,
    9499, 1278, 5526, 743,  4175, 3845, 2473, 8994, 1971, 5532, 7972, 6855,
    2417, 4631, 8070, 912,  6655, 1176, 3890, 5904, 9188, 1328, 9171, 24,
    7688, 5626, 4686, 2590, 1404, 7094, 2731, 8793, 3934, 2816, 6966, 9939,
    1009, 4671, 9081, 3655, 1594, 6729, 203,  1049, 1852, 4304, 4583, 2388,
    3335, 3320, 6078, 7165, 7213, 4177, 9902, 9163, 3992, 7690, 846,  769,
    8729, 9557, 1819, 4460, 8621, 319,  6479, 1053, 1396, 7194, 297,  5122,
    8585, 2923, 6813, 2443, 990,  1070, 3454, 5893, 4903, 5260, 9956, 2227,
    1150, 26,   714,  9479, 493,  1781, 2030, 1466, 5444, 8742, 8926, 2919,
    7676, 7181, 1068, 2066, 2884, 561,  1029, 3306, 9772, 9032, 6357, 7396,
    8097, 872,  9366, 7234, 4160, 11,   8246, 4246, 6318, 8048, 1881, 9675,
    5186, 3031, 144,  9693, 7449, 5144, 4372, 7424, 4441, 7557, 2312, 3159,
    4850, 4833, 9319, 6680, 2593, 5693, 2267, 9582, 3267, 566,  4859, 1920,
    302,  8115, 7602, 8974, 8458, 4559, 8620, 2771, 1532, 3172, 9401, 4586,
    3945, 9233, 2288, 6282, 5501, 4285, 2240, 3965, 9965, 4290, 6110, 1645,
    2764, 2282, 6490, 3536, 6186, 1519, 6780, 8312, 5723, 3138, 6510, 3734,
    7320, 8477, 5555, 507,  311,  9439, 9268, 8212, 8682, 1582, 1388, 2908,
    68,   7373, 5158, 5312, 7329, 7400, 6241, 5086, 7018, 7129, 2540, 1653,
    7188, 3876, 4470, 5660, 887,  3081, 4736, 3362, 4398, 6933, 543,  7417,
    6607, 6,    4601, 167,  9039, 4935, 8588, 5127, 7217, 8549, 1706, 1455,
    9556, 1267, 625,  3847, 7850, 1580, 6696, 1625, 5066, 4086, 4055, 6569,
    8066, 8451, 6464, 8492, 9050, 8779, 3541, 3770, 9151, 8118, 1127, 9669,
    5046, 9762, 3602, 1494, 2597, 5989, 1915, 2755, 2106, 7246, 9411, 5527,
    8708, 3004, 3030, 4883, 9844, 4829, 571,  6993, 466,  602,  3671, 9217,
    4809, 4724, 9019, 1017, 3247, 7132, 4235, 1968, 6931, 2808, 2315, 599,
    2290, 463,  486,  528,  7394, 4344, 9208, 8057, 9375, 6390, 9141, 9010,
    4230, 2876, 6953, 5037, 4590, 79,   6903, 8535, 4730, 38,   2550, 4712,
    3001, 3914, 9111, 3515, 6579, 5624, 616,  3369, 7733, 4343, 4067, 4209,
    5034, 4721, 6030, 852,  125,  2144, 5039, 9433, 2703, 9152, 2145, 9338,
    169,  9132, 7476, 3913, 527,  6142, 5557, 4892, 4464, 7479, 27,   4713,
    7499, 3662, 9904, 90,   6074, 6184, 5782, 2100, 9056, 137,  3183, 8684,
    9140, 7451, 2901, 3440, 4251, 2676, 2061, 8932, 8027, 3045, 9505, 2273,
    1307, 2820, 2171, 711,  3302, 4226, 1919, 143,  8873, 6502, 8164, 5102,
    9510, 6529, 9332, 288,  7827, 4509, 5282, 831,  552,  7929, 1823, 9099,
    6539, 9071, 6863, 7503, 5834, 8065, 6481, 8807, 8188, 1079, 5364, 1415,
    7489, 4271, 544,  2494, 7511, 830,  7648, 9784, 4219, 7902, 7958, 8961,
    3363, 5000, 184,  1187, 2461, 757,  821,  9884, 4888, 7160, 127,  2666,
    394,  5762, 9567, 8157, 6945, 1932, 6694, 3699, 9634, 1795, 7519, 4791,
    5619, 7174, 239,  8142, 1117, 7008, 3372, 1696, 7262, 9638, 815,  243,
    9691, 8822, 1878, 9326, 1683, 4104, 9808, 8888, 1596, 7575, 9004, 4284,
    5950, 4907, 8655, 3263, 8397, 6042, 7724, 9038, 3021, 1300, 2692, 4580,
    8939, 7201, 4543, 4846, 5901, 9650, 5887, 9552, 4623, 8292, 2403, 4227,
    1817, 2796, 4457, 7987, 3810, 3622, 5725, 3504, 3787, 5884, 1843, 3650,
    1555, 3906, 3002, 8595, 5806, 855,  481,  7335, 6792, 8623, 1363, 4311,
    5298, 9507, 5623, 4031, 7402, 7942, 9189, 5925, 5808, 2401, 9777, 1807,
    814,  7464, 6052, 5607, 9101, 2894, 7416, 3658, 3577, 5636, 7538, 1461,
    6971, 1325, 7543, 6123, 4974, 3607, 9327, 5837, 9889, 9128, 4957, 4955,
    8640, 8005, 4705, 7536, 4279, 4994, 5244, 9229, 1681, 6041, 7032, 8234,
    355,  8303, 1620, 3079, 979,  5546, 1752, 8255, 5609, 3972, 6103, 4473,
    8863, 2855, 1202, 9602, 9800, 5639, 4032, 141,  6092, 7414, 4004, 9024,
    878,  195,  2392, 1440, 5867, 1254, 2970, 4252, 5721, 5831, 3126, 3771,
    2926, 9419, 4499, 9320, 8747, 768,  3659, 8089, 1370, 4983, 1522, 4992,
    3099, 7244, 3833, 4565, 168,  3543, 5264, 1702, 8098, 5047, 6116, 8210,
    7970, 3569, 6316, 2972, 6111, 350,  1828, 2063, 6922, 2957, 7055, 6865,
    6682, 9284, 8603, 5910, 1764, 9170, 694,  4024, 1838, 3823, 894,  8938,
    5987, 6486, 1188, 4354, 3098, 7240, 8435, 5166, 5151, 2050, 5199, 3187,
    8264, 6094, 835,  4719, 8869, 7412, 9491, 3012, 1725, 7621, 7977, 2402,
    8756, 8061, 3378, 1119, 9363, 7892, 6757, 5072, 6652, 6633, 7397, 7422,
    3951, 8774, 1689, 8151, 9997, 4793, 6038, 540,  1835, 4277, 7485, 9303,
    5853, 8925, 3689, 8590, 7649, 4837, 7101, 7997, 2181, 7716, 521,  7686,
    6080, 4815, 4971, 6366, 3738, 8233, 9064, 4129, 1732, 3877, 114,  5734,
    7853, 3119, 7750, 9477, 7658, 4122, 5840, 8784, 3353, 5331, 1711, 5213,
    8669, 3448, 9172, 5014, 9406, 8813, 4929, 3193, 5556, 4780, 151,  794,
    8500, 8426, 9833, 1080, 2192, 640,  9620, 264,  3211, 7931, 1508, 2471,
    3911, 2032, 133,  8177, 5735, 9561, 6733, 1663, 782,  9288, 5409, 6827,
    5917, 4805, 7205, 1799, 7851, 7109, 8247, 2992, 3909, 8398, 336,  9397,
    6667, 5307, 3994, 8799, 8359, 6132, 3221, 1765, 2624, 1058, 6309, 6706,
    8614, 193,  165,  6638, 7555, 4798, 2486, 1391, 6242, 3921, 7829, 1796,
    6254, 2988, 7468, 2391, 3403, 4418, 687,  5082, 1139, 9874, 7966, 1864,
    8235, 4413, 9676, 9605, 3103, 2143, 8336, 4393, 214,  9639, 8690, 3570,
    6370, 1279, 242,  1719, 7179, 8439, 5054, 5284, 3325, 7920, 8902, 4826,
    1248, 1282, 9580, 5990, 901,  8470, 351,  9184, 4100, 6946, 1277, 408,
    8591, 2950, 6076, 9661, 6321, 3350, 1007, 1429, 9178, 5749, 8604, 8236,
    8982, 409,  9609, 3639, 4040, 1184, 3991, 3321, 3370, 107,  9255, 8541,
    6009, 6885, 5040, 2453, 9144, 3878, 5149, 2952, 2529, 5489, 8414, 4562,
    3629, 5696, 9346, 5883, 6026, 4151, 3124, 7296, 6932, 9473, 4381, 6059,
    1148, 4268, 201,  604,  7153, 2164, 3702, 5765, 9102, 4476, 2421, 5605,
    1442, 3757, 723,  8768, 63,   6395, 8498, 808,  6894, 6422, 3553, 1709,
    7903, 9069, 6315, 4370, 7715, 2678, 101,  7937, 6350, 36,   2254, 2763,
    3068, 5263, 404,  9757, 2248, 6557, 8570, 8062, 4616, 491,  6853, 3582,
    795,  4338, 1046, 2384, 9055, 3222, 2195, 617,  2971, 5891, 8396, 3398,
    175,  4416, 851,  500,  6430, 8565, 2746, 2549, 9927, 1261, 9761, 5265,
    2188, 7034, 9020, 4941, 8273, 5062, 932,  3922, 7440, 3000, 7130, 8465,
    5491, 6285, 2052, 9097, 8243, 9456, 8616, 3958, 1770, 3884, 4241, 4687,
    6525, 8261, 9543, 612,  2087, 5366, 1647, 5569, 6504, 3141, 9070, 6540,
    1816, 8293, 4923, 1730, 4224, 7606, 4333, 1477, 9641, 2579, 5178, 4996,
    3123, 8488, 2710, 5348, 4945, 9790, 1588, 4696, 7413, 8560, 8090, 5327,
    76,   8345, 5999, 2232, 8753, 9496, 6575, 8418, 3844, 2380, 9194, 5436,
    5816, 6779, 5079, 1036, 4341, 5138, 6134, 4296, 7943, 9155, 549,  5742,
    5941, 890,  8282, 4462, 9646, 8040, 7046, 2998, 2126, 9653, 5844, 1553,
    1868, 3412, 4862, 6452, 272,  2867, 8999, 752,  9590, 1451, 6015, 5872,
    2628, 2347, 2251, 3196, 2198, 2464, 8004, 7944, 3220, 9815, 8033, 9243,
    9616, 5015, 8058, 6423, 2918, 9412, 5902, 7082, 8328, 7000, 2387, 9529,
    1836, 7157, 9654, 1074, 6912, 8227, 4534, 4604, 4695, 4438, 2524, 5467,
    9028, 5347, 4520, 6881, 236,  6469, 5584, 5354, 6085, 9572, 1691, 6573,
    9321, 7033, 2969, 3011, 461,  8219, 8751, 2472, 5123, 3904, 3599, 6862,
    3636, 9389, 6577, 7867, 8084, 4668, 4483, 5505, 6209, 5280, 9442, 6814,
    1642, 8358, 126,  9826, 6833, 9426, 7332, 996,  7308, 1505, 7799, 6323,
    5760, 1137, 2538, 7001, 2320, 9017, 9657, 7600, 9541, 8605, 9305, 7310,
    5534, 8178, 8159, 5798, 1699, 5628, 1911, 2035, 4936, 5523, 8445, 2427,
    5589, 5986, 5771, 3063, 8955, 3604, 4978, 7569, 6372, 3715, 8215, 9215,
    1639, 4156, 942,  6098, 6222, 5404, 3686, 6659, 4717, 3277, 4709, 8221,
    3746, 2306, 4316, 5888, 2170, 1327, 8893, 8302, 8169, 4442, 4295, 8375,
    435,  9917, 4263, 8205, 8593, 1064, 995,  9681, 6756, 2962, 2699, 2745,
    5471, 5008, 3234, 3213, 6880, 6424, 139,  3330, 6897, 5645, 7638, 2866,
    3698, 4979, 4677, 6941, 7330, 1357, 6367, 4549, 2368, 577,  8754, 2027,
    2382, 2259, 4795, 6453, 9720, 1332, 3391, 4228, 2570, 6983, 4641, 3661,
    3894, 6720, 309,  5458, 7276, 7066, 2375, 6118, 3223, 2436, 6084, 1097,
    6381, 3315, 4613, 8382, 5369, 1211, 5909, 8825, 8180, 9781, 3843, 8186,
    6336, 6143, 9462, 8137, 2868, 2122, 6407, 3424, 3470, 3865, 1227, 1430,
    5778, 4073, 7856, 1705, 8861, 7917, 9915, 9304, 986,  7232, 5710, 4533,
    1552, 4469, 3776, 5182, 2141, 5955, 9351, 2685, 9352, 1341, 3656, 5176,
    2344, 2545, 849,  3735, 4448, 1913, 6858, 1943, 9173, 1812, 3198, 9413,
    1330, 1957, 950,  2651, 7957, 2372, 7547, 315,  8636, 9729, 33,   1231,
    2663, 2930, 8833, 9417, 558,  1892, 2854, 3953, 3160, 3186, 6787, 2070,
    6859, 8920, 8624, 4275, 8554, 6749, 8947, 2431, 2612, 508,  5838, 4536,
    3641, 7857, 1574, 3919, 9907, 8258, 3943, 620,  4218, 208,  533,  6719,
    9357, 5702, 7796, 8172, 5833, 9087, 2585, 8389, 7277, 358,  4758, 9307,
    379,  4082, 1690, 3899, 1175, 3782, 1035, 3150, 9356, 3055, 5296, 8983,
    5738, 3712, 7229, 360,  5701, 6061, 8697, 3416, 2333, 2600, 1973, 1573,
    2444, 8427, 4585, 6668, 9911, 4989, 7952, 7655, 4863, 7763, 5324, 9578,
    8366, 8280, 8146, 2592, 9674, 5824, 2361, 6459, 1712, 3374, 8206, 6497,
    5024, 7834, 5466, 9795, 485,  3820, 3963, 6631, 8787, 3924, 7703, 7403,
    6317, 8190, 3917, 8053, 8237, 1095, 1630, 3525, 3254, 7533, 6688, 5509,
    2160, 2216, 938,  4267, 2999, 2781, 8163, 5537, 2149, 3082, 690,  4498,
    2010, 3523, 4171, 9868, 8516, 7472, 5666, 9760, 2202, 100,  8127, 5080,
    4291, 8116, 647,  436,  7137, 6978, 3512, 1609, 3990, 1581, 905,  7696,
    3761, 3666, 5239, 5785, 5685, 9946, 7979, 4269, 7477, 9369, 3798, 5275,
    7907, 3308, 910,  861,  4302, 9249, 1668, 7366, 6100, 7437, 9358, 6707,
    3467, 2614, 6888, 6674, 8419, 9109, 3805, 1245, 5426, 7562, 5694, 3888,
    2730, 5543, 1930, 7281, 9515, 15,   7261, 366,  9047, 4208, 2217, 6373,
    3642, 4245, 1563, 7301, 6294, 1815, 4161, 228,  303,  9745, 1969, 1250,
    8718, 4050, 6426, 7771, 2615, 6514, 5929, 2474, 3449, 6093, 9893, 3811,
    7623, 3644, 2689, 6135, 8326, 7725, 4048, 5538, 4942, 4963, 6483, 631,
    5209, 5728, 2860, 1161, 7469, 5238, 9252, 7848, 2435, 3754, 8343, 8705,
    9227, 7072, 5878, 4420, 3089, 9514, 9966, 805,  6494, 3583, 8363, 7344,
    9068, 8017, 9273, 3518, 2048, 3777, 8579, 9160, 5745, 8829, 6067, 5684,
    1004, 1949, 4743, 8596, 5363, 2929, 9940, 8886, 3218, 2910, 2836, 3439,
    2536, 2623, 9331, 5690, 1907, 3112, 9848, 9329, 1093, 6431, 7825, 4849,
    760,  3545, 5410, 3212, 262,  4947, 6089, 3861, 581,  3880, 6728, 6775,
    5401, 5038, 5452, 8876, 7830, 8283, 2433, 2482, 8839, 701,  3260, 564,
    1348, 6591, 8019, 5356, 8198, 8152, 2457, 3614, 2793, 5468, 1528, 3465,
    2995, 2271, 1741, 3070, 5081, 6700, 8713, 4394, 1055, 4953, 7395, 4070,
    5464, 3739, 4551, 3758, 7938, 720,  9060, 2078, 7091, 5758, 3657, 4080,
    3314, 7821, 5866, 7305, 2137, 2201, 6210, 9175, 8077, 992,  3997, 5670,
    970,  8660, 9465, 7048, 1999, 8324, 3605, 5212, 5083, 5593, 4121, 5552,
    3529, 7893, 7682, 6371, 5743, 268,  4872, 8189, 7839, 2852, 9455, 4647,
    6278, 5732, 3005, 4830, 2178, 3560, 8887, 2029, 5204, 7548, 1743, 4564,
    1358, 4361, 5542, 9832, 3301, 3593, 7218, 2517, 2073, 530,  7077, 9490,
    9478, 9153, 5850, 1155, 3494, 1192, 4711, 1092, 9315, 5511, 2543, 7149,
    6854, 3486, 4629, 2756, 430,  3881, 1767, 7365, 8408, 8459, 2343, 4669,
    6149, 4529, 6710, 5273, 3615, 265,  2656, 7668, 5854, 8339, 247,  3102,
    5129, 6351, 8402, 882,  7567, 8013, 1880, 4028, 7178, 4391, 7660, 6019,
    9447, 4599, 8832, 6923, 6969, 4767, 419,  5027, 3763, 7576, 8094, 3311,
    8941, 4278, 8140, 7787, 3236, 3806, 896,  6839, 3827, 7980, 9503, 736,
    2873, 99,   3431, 1986, 7936, 6131, 6028, 1687, 8586, 9659, 5423, 1507,
    9995, 4114, 3937, 8290, 284,  9725, 7185, 8135, 8381, 1774, 9817, 1649,
    219,  8096, 8598, 8574, 9952, 3752, 4479, 1797, 9312, 8814, 6283, 5248,
    3329, 7263, 1601, 9348, 8260, 9977, 8257, 785,  1124, 263,  4132, 2310,
    6257, 3663, 761,  4922, 662,  4084, 7739, 2500, 947,  6646, 6594, 6634,
    6670, 6871, 7522, 6368, 2071, 8566, 8688, 1221, 7406, 7513, 4627, 6081,
    2606, 1487, 9694, 1232, 3100, 7989, 18,   7905, 5724, 6290, 2965, 275,
    145,  9048, 6665, 7693, 2647, 594,  5678, 3987, 6391, 9840, 6926, 2229,
    340,  8020, 2328, 9211, 4760, 9743, 1445, 4090, 5110, 5775, 4770, 3764,
    2747, 9867, 9535, 8906, 9978, 2492, 1229, 6864, 8691, 3256, 4928, 3766,
    4910, 1230, 7875, 8967, 5103, 8256, 8315, 9362, 4110, 2638, 5343, 212,
    6221, 1151, 5379, 3435, 3635, 8489, 1022, 6900, 5137, 7393, 7290, 7264,
    7336, 7075, 7346, 1344, 6963, 6330, 4059, 4701, 6544, 1640, 8664, 8950,
    7930, 8291, 8544, 5680, 5220, 5124, 4214, 7473, 2961, 4124, 1664, 2115,
    9435, 3968, 2355, 8809, 6545, 9045, 5385, 3491, 9824, 2275, 8475, 2377,
    5665, 7822, 7227, 4315, 6473, 4541, 9627, 9272, 6239, 8672, 6501, 8646,
    4617, 9900, 4518, 3280, 5434, 7679, 865,  8025, 5090, 1560, 551,  8854,
    1974, 8670, 1476, 1772, 5130, 7431, 5961, 4618, 4046, 8651, 4720, 6857,
    2925, 4506, 2568, 4003, 318,  2112, 5528, 4801, 2025, 3857, 6308, 5337,
    9404, 9974, 5215, 2166, 993,  4085, 5794, 5424, 7792, 2772, 9980, 9388,
    5789, 2352, 4371, 3741, 8581, 5115, 7807, 2163, 8082, 3690, 8262, 1922,
    4201, 341,  7152, 7840, 2465, 3274, 3291, 5493, 6073, 492,  7321, 4540,
    856,  6937, 2334, 6065, 3380, 4676, 5328, 4364, 6847, 5469, 4421, 1500,
    9121, 4113, 8105, 6788, 2896, 296,  939,  7041, 8537, 2552, 7581, 8959,
    6752, 6182, 5221, 645,  477,  1727, 3603, 8773, 3339, 9100, 5232, 8167,
    3619, 4288, 2017, 8092, 2948, 3721, 5109, 1844, 279,  5287, 3775, 1249,
    8288, 6584, 4538, 7895, 7869, 6011, 7990, 8100, 9260, 7071, 8972, 4202,
    7587, 510,  8632, 1065, 9644, 567,  4561, 4431, 4512, 3728, 9631, 9732,
    1220, 9234, 7358, 5461, 4965, 9248, 4926, 6068, 8897, 8038, 1666, 5683,
    6523, 1446, 4157, 4128, 1170, 5438, 8587, 9742, 4368, 7698, 3034, 8000,
    3975, 4600, 3496, 6228, 4864, 8666, 5381, 6519, 9090, 2668, 2349, 4270,
    8134, 6755, 3170, 6386, 5484, 1266, 438,  5297, 4655, 310,  945,  718,
    1850, 5108, 7673, 886,  7341, 6062, 2019, 9078, 6731, 897,  5242, 445,
    2058, 1386, 6681, 7496, 5153, 4258, 9779, 5767, 7106, 6396, 1112, 4748,
    2885, 8306, 850,  5503, 9665, 166,  683,  1783, 4222, 3371, 6712, 1847,
    4392, 1564, 9467, 7372, 9104, 3028, 96,   9119, 2148, 9489, 7368, 7204,
    4484, 5208, 8191, 7456, 6669, 5821, 121,  5344, 6345, 592,  5235, 2829,
    4962, 6559, 7427, 3273, 2462, 9186, 8740, 1861, 8803, 5972, 1707, 9668,
    1206, 4666, 3687, 9193, 9354, 9704, 2722, 4313, 576,  9317, 3722, 7578,
    5720, 871,  3665, 695,  668,  7711, 7124, 2036, 6444, 4005, 4838, 5597,
    8835, 8794, 7146, 3402, 7928, 9148, 5590, 8350, 2242, 7087, 6018, 7909,
    4939, 8268, 7721, 6650, 3237, 5013, 7900, 4425, 5766, 5952, 3262, 6146,
    1021, 6801, 8675, 9250, 5065, 4384, 484,  2520, 3597, 4137, 3472, 3033,
    8259, 1944, 5817, 6226, 7450, 3072, 211,  3556, 4152, 6176, 5391, 6001,
    2953, 4908, 3755, 9583, 4282, 6636, 2397, 5197, 2200, 3116, 7068, 1887,
    7298, 1710, 8056, 1780, 5751, 5596, 9063, 8410, 1910, 8850, 4762, 0,
    6996, 7241, 1280, 8364, 8461, 5374, 1465, 8848, 248,  9948, 9035, 3850,
    774,  9372, 545,  6561, 240,  2241, 8474, 8130, 2136, 1768, 4865, 6828,
    1525, 6763, 3022, 9955, 2725, 3326, 6875, 4513, 1157, 1361, 3373, 5769,
    6906, 6247, 6721, 8352, 541,  7802, 5236, 763,  2423, 2814, 2886, 909,
    2014, 8136, 1840, 5214, 6762, 7631, 1083, 7566, 6641, 9325, 4858, 6218,
    621,  3759, 4804, 7742, 3889, 5903, 40,   3083, 8406, 734,  4358, 7585,
    9420, 1810, 5154, 3395, 5460, 5759, 1966, 5502, 9123, 3469, 7478, 7556,
    3164, 8548, 3573, 6790, 8120, 6886, 1598, 2903, 2093, 241,  7059, 4164,
    5549, 3923, 5804, 7021, 4077, 7919, 332,  3276, 4286, 583,  119,  9726,
    9910, 1342, 8002, 6771, 9231, 2811, 138,  6244, 4516, 918,  3289, 6505,
    220,  9536, 7852, 9544, 3595, 8800, 4011, 4869, 5936, 2637, 9282, 1784,
    5540, 4692, 981,  7020, 3025, 6902, 1318, 618,  5029, 8287, 2566, 2169,
    5922, 4074, 5389, 6815, 848,  2603, 6287, 8060, 9926, 1153, 4630, 2899,
    2591, 5516, 8393, 6088, 7275, 1837, 388,  7100, 5858, 1454, 6174, 2408,
    9337, 1246, 4988, 4154, 9962, 276,  786,  9266, 8103, 2667, 4415, 9005,
    482,  1114, 498,  8447, 2411, 8622, 1428, 3939, 5737, 522,  5125, 9179,
    9656, 4021, 7364, 6867, 7224, 3275, 7114, 3396, 6349, 5991, 4550, 2739,
    6389, 6274, 4913, 4754, 9169, 578,  7712, 4144, 933,  4307, 1726, 4776,
    5449, 1676, 3947, 4819, 8423, 2946, 4326, 1952, 4365, 7963, 5874, 6626,
    2394, 7284, 8973, 9257, 9972, 2260, 9890, 2018, 8943, 5326, 2707, 5060,
    1715, 1163, 9708, 9387, 1121, 357,  2768, 2859, 9774, 2662, 2157, 9226,
    6319, 5885, 4548, 2761, 8940, 7647, 496,  9611, 696,  5568, 6192, 7223,
    2556, 330,  4053, 6169, 2469, 8659, 2278, 2879, 5156, 3016, 7340, 4572,
    3926, 798,  2092, 7922, 4310, 8661, 3309, 3058, 1427, 9286, 2522, 3816,
    5169, 987,  2439, 1346, 3621, 7927, 3883, 8067, 9011, 1144, 4895, 118,
    8976, 6437, 3053, 4198, 5091, 6165, 88,   4606, 1806, 3153, 5663, 2099,
    8958, 5159, 8395, 8831, 676,  7057, 7343, 2332, 9000, 8653, 9308, 1748,
    9903, 3294, 389,  3351, 6664, 2546, 4612, 1276, 2151, 3140, 3668, 5340,
    5145, 6614, 6718, 8007, 6253, 1032, 5496, 3489, 6235, 3505, 3192, 9481,
    9925, 3950, 5699, 3852, 7117, 298,  2525, 5310, 5351, 4519, 9702, 9992,
    9423, 4058, 5521, 8213, 4402, 6461, 3310, 6484, 3724, 5820, 1238, 9585,
    3542, 1459, 7545, 6170, 1219, 1662, 5057, 8183, 1426, 8879, 8874, 6560,
    1194, 9085, 4419, 8356, 6358, 1524, 1108, 1372, 5117, 8171, 1489, 9180,
    9953, 5973, 7135, 230,  9607, 550,  5403, 8686, 2235, 513,  5818, 1529,
    4339, 3508, 6563, 3624, 2243, 7894, 2072, 9049, 7789, 8299, 637,  2863,
    2652, 3285, 3059, 8024, 3244, 9723, 3444, 1813, 4505, 4622, 174,  2742,
    7933, 4802, 1441, 6010, 589,  9614, 6927, 2887, 2338, 7969, 2732, 8868,
    2840, 3283, 826,  7512, 3202, 569,  4982, 9484, 2005, 95,   7036, 1376,
    8674, 238,  4366, 5349, 7687, 8714, 7199, 8012, 7026, 7259, 1182, 9929,
    4089, 1513, 5707, 6410, 506,  8320, 2176, 2807, 636,  4609, 7883, 1319,
    7899, 1761, 8785, 5152, 1418, 4946, 6904, 4095, 9166, 8630, 8405, 7287,
    6761, 2283, 9382, 6474, 3500, 3685, 1983, 1995, 731,  6179, 8922, 3334,
    8968, 3858, 6585, 537,  5889, 5268, 5342, 6811, 5245, 1106, 754,  9158,
    5637, 4350, 9310, 8769, 9300, 2985, 2340, 7304, 5571, 2399, 6805, 8542,
    9440, 2582, 9281, 8448, 71,   9483, 4980, 6737, 1509, 8387, 2133, 7568,
    5254, 5779, 5880, 7935, 2900, 1295, 4244, 8824, 5777, 8989, 5688, 9324,
    1660, 5664, 444,  2726, 2723, 5334, 3832, 3130, 8612, 8657, 3896, 4638,
    9897, 4492, 1456, 3377, 1885, 2775, 3648, 4317, 1901, 6676, 9919, 5859,
    7962, 749,  3409, 4169, 2636, 8607, 1000, 4553, 2184, 7029, 4943, 6547,
    8521, 9660, 2389, 8731, 1408, 1940, 3637, 7267, 3979, 2322, 5251, 791,
    9622, 2480, 1398, 8680, 1141, 2862, 1900, 3405, 6157, 8775, 3998, 9773,
    3191, 4390, 2825, 6337, 7052, 6744, 4194, 2650, 8277, 399,  9834, 568,
    2089, 915,  1389, 2096, 6767, 4192, 2534, 2398, 5588, 457,  1486, 817,
    7280, 6499, 2951, 7367, 9798, 8495, 4155, 7608, 3967, 1877, 7705, 274,
    8409, 7122, 1902, 3652, 6341, 9875, 5647, 9973, 4168, 1542, 7706, 5510,
    624,  2872, 885,  1368, 6776, 6872, 9008, 4451, 2743, 7842, 1632, 4408,
    9569, 5428, 4812, 3365, 1166, 2693, 1213, 4071, 2912, 3413, 6299, 7887,
    2269, 3789, 702,  2385, 75,   1801, 9031, 2378, 2895, 729,  5019, 7369,
    7084, 1273, 2541, 8883, 9168, 4569, 8712, 2053, 4106, 9684, 5267, 2371,
    2045, 5658, 2530, 9937, 4256, 8891, 6107, 4375, 9944, 8385, 475,  5494,
    7692, 1937, 3706, 4653, 5860, 3166, 5196, 6125, 6296, 182,  8737, 3304,
    6443, 2717, 3054, 1722, 54,   7093, 5190, 2460, 7481, 2608, 3960, 2302,
    5074, 6548, 2706, 2753, 6925, 6122, 2249, 6288, 2033, 6554, 7377, 1355,
    6703, 2528, 4159, 2943, 1496, 3586, 4026, 6057, 1191, 1536, 1384, 3949,
    5106, 392,  413,  2379, 9643, 8644, 9468, 3874, 876,  6777, 6819, 2595,
    6070, 4975, 3281, 7670, 9147, 4399, 3423, 9292, 3948, 4162, 9494, 706,
    2367, 9938, 8578, 6829, 5413, 9065, 2518, 8289, 1670, 4010, 6017, 8701,
    8485, 1183, 9885, 9007, 588,  6445, 9765, 4204, 4847, 2459, 5667, 7874,
    9821, 5228, 843,  381,  9968, 1130, 8029, 727,  2370, 1962, 2809, 6250,
    6406, 1303, 6175, 582,  1718, 2622, 1210, 8354, 6612, 3209, 8473, 6699,
    1502, 2589, 8064, 7509, 984,  1631, 2172, 4474, 2040, 3328, 6379, 8944,
    7312, 1197, 5219, 8901, 3243, 4081, 6467, 3480, 2264, 5581, 6724, 1422,
    5456, 5963, 2056, 7908, 4727, 2412, 6915, 9941, 1734, 7946, 6553, 4495,
    1186, 2161, 9830, 5918, 5257, 9801, 9082, 4215, 384,  1746, 2168, 9018,
    7967, 8694, 6404, 1498, 3568, 5646, 6970, 7835, 2864, 251,  5281, 7723,
    377,  2617, 941,  4199, 6689, 2135, 788,  7243, 6212, 8523, 8021, 6791,
    6032, 8757, 7462, 1738, 2189, 2007, 6956, 111,  2359, 2015, 5657, 1787,
    7695, 4334, 5763, 8741, 1998, 1975, 3075, 8788, 1048, 4356, 8652, 4000,
    7067, 1591, 117,  412,  870,  4231, 4906, 3109, 3620, 7518, 4012, 9804,
    9131, 2209, 67,   4233, 8301, 87,   6298, 8374, 6995, 6075, 1047, 108,
    7813, 1110, 6530, 6735, 1321, 1859, 197,  9072, 1367, 4135, 5033, 4827,
    1131, 9843, 1568, 6375, 3538, 2633, 7191, 7921, 1366, 3747, 6148, 3134,
    8508, 329,  1016, 8828, 4020, 4588, 8519, 2586, 8179, 3479, 5487, 2996,
    6151, 6600, 7249, 440,  6571, 7050, 6384, 1337, 8106, 32,   8600, 2675,
    4123, 4967, 2828, 4694, 6572, 9430, 6850, 7973, 7727, 4566, 8469, 7493,
    6837, 9497, 2081, 646,  4301, 1928, 1272, 6683, 3717, 5604, 2376, 9265,
    9463, 2584, 5210, 7031, 9652, 2351, 8270, 957,  9574, 7764, 7836, 9263,
    1926, 5632, 2270, 7740, 3228, 553,  6974, 6295, 9498, 432,  364,  4825,
    39,   5615, 2990, 4189, 1291, 7386, 5519, 1181, 2561, 8693, 1994, 6930,
    6647, 7212, 8055, 5473, 9983, 3074, 4741, 2075, 4621, 1648, 1637, 2252,
    5729, 1680, 7235, 9296, 1757, 4844, 9457, 5277, 7375, 7483, 1925, 9403,
    5180, 3985, 2257, 9464, 446,  7272, 3240, 487,  9916, 1165, 8885, 6958,
    8896, 6549, 4662, 5899, 6327, 968,  3983, 2891, 4485, 2892, 9122, 447,
    4839, 6072, 1688, 6025, 1399, 9033, 7350, 4619, 4076, 2916, 2430, 4757,
    5309, 502,  692,  1371, 3381, 3115, 8536, 3804, 4197, 9370, 3841, 5006,
    6435, 9909, 8679, 5674, 5335, 62,   4147, 8276, 6468, 5111, 8504, 2759,
    5320, 4693, 5399, 7501, 4118, 417,  3313, 2407, 93,   459,  2670, 7318,
    5155, 5800, 7327, 6007, 7161, 9314, 4537, 4002, 5206, 3748, 6949, 2117,
    1259, 1701, 6268, 877,  7948, 8030, 4851, 2577, 9933, 7728, 1899, 8899,
    4182, 9283, 2309, 793,  3195, 3376, 5384, 346,  3086, 3942, 1938, 7932,
    5780, 8870, 473,  1779, 8039, 2968, 3765, 9480, 4729, 9278, 524,  5875,
    6488, 2587, 3912, 4952, 6219, 1034, 8347, 4854, 4535, 9846, 8143, 1340,
    8834, 9587, 347,  9812, 7770, 2088, 997,  4889, 6045, 9041, 6440, 4683,
    7053, 9957, 9870, 316,  9513, 976,  8649, 8867, 7060, 9683, 4030, 4949,
    7846, 2373, 3282, 5750, 2329, 4841, 6058, 5314, 4240, 8390, 1831, 3255,
    3250, 8836, 3050, 7521, 102,  9899, 9287, 7038, 6022, 5128, 9827, 1424,
    4999, 6197, 9964, 7003, 4488, 9365, 8987, 5045, 2047, 4797, 4912, 4503,
    1198, 9342, 5959, 4831, 7079, 261,  7897, 5333, 9261, 8995, 7428, 9003,
    8454, 1468, 3552, 5533, 6398, 9839, 1338, 3317, 160,  7202, 3014, 8075,
    3455, 7410, 2021, 8716, 7269, 4336, 6824, 2055, 7849, 7186, 2233, 4,
    2134, 6640, 4909, 1260, 7824, 3084, 4150, 8238, 9768, 6914, 1636, 1315,
    4916, 6105, 3649, 8685, 2861, 9942, 1745, 3559, 335,  6433, 9863, 7288,
    6960, 3038, 1867, 3414, 8528, 218,  4715, 2129, 8346, 6803, 9584, 5068,
    3162, 6252, 591,  8563, 7436, 380,  6662, 8895, 6482, 7531, 797,  9094,
    9216, 4127, 3018, 8035, 6347, 1236, 2641, 2413, 3819, 4615, 9177, 9625,
    4173, 7154, 1842, 6194, 2490, 9865, 2214, 1561, 1948, 5943, 6588, 1970,
    7613, 2989, 1173, 5565, 7702, 1537, 7177, 7210, 2532, 2527, 2744, 3792,
    8978, 2084, 4567, 7526, 6928, 1713, 120,  7873, 7362, 4765, 4937, 3535,
    13,   2484, 2477, 7353, 286,  1242, 2984, 8483, 6840, 3584, 7221, 880,
    7797, 8,    9192, 1872, 3756, 853,  8254, 6576, 3336, 293,  8316, 3780,
    2210, 8527, 8338, 3176, 1521, 5141, 4873, 3345, 6047, 8547, 8758, 9831,
    1020, 5026, 4964, 7891, 8028, 9994, 9640, 6269, 2928, 6419, 6503, 4684,
    6596, 9116, 9023, 7971, 3347, 967,  3944, 9838, 6684, 1233, 4969, 6769,
    8964, 1634, 345,  190,  5323, 84,   5089, 2786, 3679, 9361, 3933, 9006,
    258,  6727, 422,  559,  7419, 3453, 661,  4976, 4787, 9613, 5754, 7510,
    3300, 5,    2535, 8802, 124,  2237, 3457, 1567, 6160, 4732, 8765, 7070,
    2544, 5675, 5474, 3703, 3727, 9482, 8584, 8362, 8368, 9991, 4359, 4146,
    8699, 6882, 8460, 1654, 1724, 2599, 8044, 1739, 2396, 9753, 3420, 680,
    7744, 4651, 6265, 8394, 3422, 4257, 4400, 3796, 9472, 7968, 292,  5970,
    7255, 7421, 6964, 4544, 6181, 6781, 6189, 6099, 8442, 1599, 7717, 5563,
    9436, 3341, 6999, 7418, 1960, 1168, 2211, 7252, 5703, 1364, 6725, 2958,
    6380, 9299, 9002, 9700, 3801, 4401, 3742, 601,  5415, 9240, 903,  5633,
    8924, 6830, 3049, 8805, 9269, 8199, 4439, 3879, 3793, 5928, 5398, 1253,
    9860, 8877, 4043, 1059, 9245, 8384, 3669, 295,  6992, 960,  4446, 453,
    2794, 7782, 9302, 8111, 8530, 6998, 7877, 6354, 456,  8478, 8413, 8744,
    573,  4352, 879,  8422, 8154, 8073, 7881, 5408, 7172, 4524, 7601, 3514,
    1052, 9157, 7118, 7662, 5193, 615,  2869, 7960, 3286, 7992, 6471, 5638,
    8633, 5826, 6133, 4821, 9145, 3767, 1226, 6908, 5482, 5304, 4292, 5345,
    4626, 7780, 3258, 7951, 5388, 704,  3866, 1196, 9390, 4017, 2173, 3174,
    6013, 3406, 7442, 5346, 2097, 8576, 391,  4877, 8207, 8010, 6054, 6807,
    7898, 8966, 7643, 5525, 3678, 926,  2140, 4016, 7441, 4034, 6660, 7265,
    6216, 674,  6678, 2291, 4449, 9270, 1951, 9993, 8063, 7116, 3217, 501,
    5336, 74,   8181, 9755, 3284, 630,  1481, 534,  6329, 8166, 4759, 4610,
    3938, 6229, 1603, 7870, 185,  2024, 1120, 1884, 4027, 5317, 8683, 2304,
    1686, 8776, 807,  4552, 1758, 7984, 4731, 1750, 2601, 8875, 5269, 2695,
    7086, 3705, 9852, 6023, 5933, 1622, 1407, 516,  3513, 3484, 4884, 5788,
    8440, 3241, 1997, 596,  6924, 9873, 2526, 8602, 3902, 2632, 8329, 9037,
    614,  7630, 7791, 2262, 1335, 1754, 25,   4477, 832,  9093, 842,  6224,
    6124, 7910, 2383, 3931, 5564, 1012, 280,  5226, 759,  6106, 6286, 6984,
    2945, 1569, 3343, 5305, 3008, 9548, 8449, 9364, 6644, 2871, 3670, 664,
    6673, 4351, 2268, 3499, 3935, 4642, 7399, 7445, 9592, 7474, 906,  9410,
    6656, 3323, 5733, 3856, 2616, 9612, 4330, 6589, 344,  8144, 82,   1793,
    4355, 7843, 1677, 5150, 2679, 9242, 6374, 3154, 4885, 7540, 1869, 4029,
    7534, 4840, 2004, 7415, 2654, 7525, 415,  474,  2002, 4517, 4843, 3085,
    7537, 6962, 7454, 4986, 3232, 9764, 8647, 2006, 2560, 2750, 6671, 5442,
    4857, 9558, 4924, 46,   6171, 542,  2104, 4211, 2261, 5650, 5744, 5748,
    2513, 5617, 1403, 1094, 4938, 9422, 2935, 47,   2386, 8859, 1471, 8878,
    7446, 2311, 2303, 771,  4210, 4380, 888,  5043, 7765, 8882, 2515, 1834,
    5583, 9642, 6035, 5554, 8050, 6416, 1122, 2588, 8526, 4091, 9996, 1543,
    8480, 1558, 7863, 7090, 4960, 2823, 8894, 450,  1678, 1612, 1651, 6784,
    1381, 780,  5020, 2848, 2539, 9475, 2405, 8042, 9825, 5979, 5508, 1369,
    131,  66,   8724, 9935, 6568, 1697, 8830, 44,   8340, 8321, 2452, 5294,
    8176, 9015, 8016, 838,  7914, 3095, 8795, 3133, 3769, 9114, 8365, 5985,
    5726, 4993, 9276, 8860, 529,  1504, 4281, 9150, 4985, 2279, 6003, 7103,
    7136, 3550, 9621, 6989, 9637, 8380, 7805, 1667, 9517, 7939, 9392, 464,
    5774, 4196, 5644, 3882, 535,  5457, 1854, 2741, 6365, 6495, 6701, 7605,
    9796, 2301, 8662, 4083, 9791, 9396, 1091, 4771, 5672, 244,  1979, 2455,
    6947, 5894, 9806, 1769, 21,   8049, 8335, 2422, 1853, 2127, 8937, 2789,
    5547, 3790, 1132, 2083, 4079, 1889, 9251, 7292, 839,  8284, 6090, 3006,
    4785, 4337, 700,  3436, 5996, 4109, 952,  7978, 224,  1488, 8721, 7965,
    3626, 5913, 6155, 8107, 810,  5075, 4584, 3393, 6613, 5960, 3359, 3691,
    9244, 8728, 3800, 1674, 7387, 4808, 1490, 6154, 6705, 6743, 6713, 1288,
    5010, 4882, 6942, 5255, 9385, 538,  7888, 1296, 7794, 733,  1933, 8681,
    2672, 574,  1425, 9220, 9295, 9025, 4377, 6736, 2729, 509,  5315, 7,
    8643, 2516, 3578, 2815, 5237, 5635, 489,  9828, 3825, 2882, 8739, 2850,
    6616, 3206, 600,  5329, 3127, 4966, 4639, 5417, 2296, 2594, 6145, 9988,
    7783, 348,  9629, 4734, 6587, 2890, 7030, 9043, 1643, 1737, 8577, 9476,
    8817, 5613, 3118, 6456, 7590, 7595, 361,  6144, 4260, 6178, 1863, 9520,
    16,   7488, 6271, 2607, 7040, 6625, 5958, 4706, 789,  460,  1655, 3324,
    449,  512,  1811, 1392, 4867, 5861, 5291, 232,  353,  9190, 4274, 2218,
    9162, 3897, 8626, 5906, 1162, 9542, 5969, 4778, 8197, 9715, 7778, 7916,
    4097, 5630, 675,  4497, 9415, 8927, 8417, 2554, 3447, 4078, 1171, 3740,
    8538, 7627, 5488, 943,  5655, 2790, 3190, 5682, 5978, 8763, 2244, 812,
    1294, 6618, 6883, 7382, 5641, 5439, 7187, 2393, 4589, 4951, 5776, 2174,
    2788, 3434, 9573, 2318, 3331, 3231, 3108, 7176, 611,  9187, 4699, 3611,
    8749, 8615, 4205, 3207, 2778, 2415, 5862, 4665, 2978, 2323, 6401, 819,
    693,  6355, 8095, 5709, 3023, 3651, 652,  9009, 2009, 4044, 2783, 7528,
    3713, 9341, 9532, 5107, 9771, 7524, 2426, 5573, 7429, 5815, 7006, 1015,
    1517, 470,  9623, 5114, 5518, 235,  523,  2418, 7709, 5531, 9822, 4813,
    6944, 2331, 5431, 983,  8506, 7964, 1931, 2287, 1721, 6164, 3551, 908,
    1965, 2152, 7076, 8931, 3293, 7800, 587,  3471, 841,  4172, 2681, 2618,
    1142, 6624, 4707, 6639, 2228, 1605, 7352, 1208, 4407, 7683, 109,  2964,
    2441, 5974, 2982, 8031, 7325, 3145, 150,  2065, 2038, 8656, 7158, 281,
    1027, 505,  3677, 2649, 9421, 2131, 3203, 14,   9687, 6162, 4293, 4126,
    4232, 1410, 6910, 2265, 1453, 1406, 7195, 4200, 7058, 5659, 2119, 2963,
    3295, 3208, 8820, 4216, 3719, 4768, 959,  2086, 31,   6260, 2498, 8912,
    7322, 1592, 4237, 8855, 5704, 2487, 9628, 9130, 6890, 9794, 6310, 7254,
    7126, 8319, 2177, 4645, 572,  8569, 9373, 1439, 8789, 7773, 6256, 5056,
    4066, 1320, 1189, 8403, 4902, 5499, 2479, 367,  7864, 5202, 7777, 9949,
    9534, 2193, 6180, 401,  3429, 45,   8376, 3558, 2792, 8509, 8455, 8222,
    2682, 4563, 3093, 8884, 7553, 2956, 1077, 6690, 6892, 4781, 9113, 4092,
    3785, 1675, 5654, 8761, 186,  5558, 4458, 8494, 2123, 9678, 9293, 2576,
    2353, 2326, 3633, 1394, 1717, 400,  563,  162,  7718, 4876, 7762, 1042,
    4013, 8796, 904,  2821, 9780, 8625, 1518, 1326, 7880, 2345, 7351, 42,
    9201, 1209, 3253, 5944, 1851, 1539, 9763, 3531, 3797, 4207, 5570, 1460,
    5714, 1785, 7383, 1540, 9730, 3731, 2167, 7080, 1352, 478,  790,  9336,
    715,  5018, 2298, 3773, 3009, 3993, 8295, 3481, 755,  9383, 5980, 2299,
    584,  8399, 1251, 6831, 7370, 3129, 1129, 9776, 6079, 9895, 3842, 8991,
    9523, 3609, 5142, 7577, 7141, 829,  6911, 4625, 7565, 7047, 17,   9246,
    9333, 989,  9564, 9758, 6008, 924,  6300, 6156, 6136, 2610, 1025, 7145,
    58,   7482, 7044, 5957, 9414, 3590, 9967, 4320, 1485, 1351, 1606, 3854,
    916,  1317, 8627, 5030, 3893, 7498, 9487, 7544, 9747, 3788, 5876, 5453,
    580,  6246, 5497, 1988, 5063, 5078, 4539, 1377, 9108, 8704, 9887, 9878,
    9898, 6876, 147,  3962, 9106, 7674, 1934, 9095, 8337, 3215, 784,  5490,
    3356, 758,  2801, 2834, 9734, 5627, 9947, 931,  2806, 2645, 6795, 7677,
    3428, 5689, 4064, 9522, 8441, 2766, 2456, 7500, 969,  7886, 4412, 2733,
    7324, 1084, 4640, 9921, 4772, 178,  4068, 5116, 387,  1694, 2360, 1808,
    9531, 4510, 6150, 4685, 5856, 3224, 2374, 5229, 8858, 1729, 6147, 6651,
    9230, 8618, 6199, 8334, 1118, 8187, 2020, 2284, 5025, 2665, 2454, 1499,
    6158, 2611, 9129, 7311, 4528, 8557, 6112, 2256, 8216, 8286, 8248, 9905,
    7140, 1463, 8407, 5132, 5003, 4417, 7175, 7704, 6343, 382,  539,  1659,
    7745, 6040, 4875, 6201, 5916, 433,  4591, 1763, 4607, 5262, 5642, 1385,
    2475, 9297, 3408, 2712, 2605, 5911, 6128, 3026, 891,  8196, 9648, 4749,
    6716, 3384, 6629, 3980, 3375, 9083, 4681, 7913, 1535, 2845, 3750, 6119,
    5835, 9883, 4120, 3492, 6196, 7062, 1190, 7380, 3229, 9501, 4463, 1955,
    6382, 7420, 6153, 7748, 7260, 4088, 1523, 3415, 3474, 7614, 8192, 644,
    5185, 1565, 1436, 6887, 8629, 4099, 9930, 362,  4454, 2012, 2082, 7982,
    8908, 5432, 5300, 317,  6935, 4158, 4611, 5405, 5687, 3587, 2798, 3506,
    6649, 5338, 6879, 8837, 3401, 8715, 189,  374,  8046, 8916, 8960, 4718,
    6478, 7941, 8665, 1981, 5757, 2683, 2483, 9647, 2162, 9086, 864,  471,
    4008, 1006, 2949, 5579, 7761, 1177, 4547, 2156, 7452, 1832, 5551, 2795,
    2400, 180,  7278, 4321, 9986, 8522, 1123, 1658, 9617, 4817, 1685, 1199,
    9696, 8078, 6841, 9428, 3462, 1360, 1154, 8043, 4435, 9408, 7644, 1435,
    6851, 7757, 1512, 3338, 6231, 7828, 4382, 8980, 3466, 1345, 9492, 462,
    3144, 8783, 9405, 250,  9597, 7355, 6198, 2696, 1657, 7494, 8404, 2669,
    4744, 1393, 8014, 2698, 7911, 3836, 6364, 4340, 3131, 7774, 6248, 7666,
    5454, 8088, 97,   1314, 7983, 386,  7215, 472,  5643, 1039, 2442, 773,
    7697, 8360, 153,  8468, 2194, 1447, 191,  4389, 3268, 1081, 1733, 7163,
    294,  1515, 1954, 6331, 1215, 337,  6338, 5093, 7011, 3786, 9786, 9872,
    8811, 937,  4620, 4522, 2185, 8086, 5183, 7837, 1939, 6602, 1978, 2197,
    9120, 9603, 8550, 2856, 8979, 7583, 1839, 4367, 7447, 8081, 3903, 8457,
    9549, 3817, 4212, 6449, 6661, 4811, 7507, 1903, 431,  2330, 3264, 7826,
    7572, 8929, 2142, 6988, 9672, 8204, 6263, 5133, 8313, 7786, 8561, 3548,
    3984, 7819, 6808, 5373, 7924, 6817, 1541, 1628, 8231, 6305, 5599, 7684,
    4247, 2434, 1893, 927,  4948, 5042, 3165, 5476, 6005, 9079, 8226, 7734,
    499,  7283, 8746, 4752, 962,  1826, 5355, 7200, 4213, 536,  9029, 8582,
    6491, 8145, 8369, 9149, 3287, 9537, 2013, 781,  3107, 8546, 2634, 8971,
    4670, 5187, 4745, 7539, 2827, 4806, 2770, 3988, 656,  9466, 5246, 2128,
    4652, 5935, 7996, 6979, 9982, 1339, 1846, 9222, 3441, 3704, 2578, 8619,
    4038, 2782, 1262, 6836, 6102, 6619, 5755, 8689, 889,  6095, 9495, 1896,
    3813, 3872, 862,  6722, 1638, 5397, 8110, 3837, 4468, 4072, 9814, 8450,
    3477, 6698, 3711, 5890, 9235, 1824, 4133, 4973, 6512, 8182, 9115, 8230,
    3233, 9550, 7182, 5768, 8388, 383,  3774, 6825, 342,  6954, 1736, 3676,
    9813, 7392, 72,   8900, 7144, 7271, 1633, 2898, 9990, 5230, 5946, 1879,
    2519, 8843, 9615, 8471, 9594, 5741, 78,   2680, 4789, 7720, 3065, 2865,
    7012, 6091, 4556, 7348, 3986, 1421, 6527, 603,  8377, 2571, 4667, 5382,
    1400, 5669, 4422, 7475, 8174, 1405, 1723, 6450, 9630, 5223, 5318, 9971,
    1223, 1501, 6039, 9858, 1382, 5339, 4094, 8309, 6597, 2994, 7010, 5932,
    1918, 5790, 9512, 5365, 3868, 7781, 7297, 8353, 6139, 9778, 4236, 9448,
    4934, 4428, 7228, 8575, 5585, 4508, 426,  1906, 813,  4445, 8564, 4991,
    1362, 6582, 7772, 5357, 1771, 4751, 3873, 4065, 4141, 1790, 8225, 9098,
    6758, 4480, 7726, 9444, 3660, 9256, 9562, 3333, 7861, 8866, 1397, 1087,
    4632, 7112, 7640, 5350, 6049, 8985, 8792, 7120, 7081, 5517, 3608, 3152,
    3017, 2121, 8001, 9559, 7738, 7171, 9666, 4879, 7956, 6262, 5483, 4139,
    8513, 5967, 6340, 5610, 7184, 9165, 7250, 2107, 4131, 9343, 7480, 2977,
    3920, 9353, 4832, 3121, 4598, 4816, 7586, 3459, 5446, 4944, 3043, 8131,
    7872, 1804, 6818, 8371, 2446, 9359, 3526, 1125, 3907, 975,  8611, 2661,
    4714, 6621, 7755, 221,  4383, 1731, 6056, 7588, 9254, 9525, 4997, 8054,
    8865, 3417, 1102, 9077, 9727, 9688, 9394, 5536, 7858, 7784, 8573, 6730,
    4898, 3539, 5983, 1641, 6344, 2225, 233,  4149, 3178, 9619, 1062, 8307,
    1309, 5914, 5256, 4764, 3495, 2736, 2245, 5561, 2074, 776,  597,  1492,
    1011, 7349, 6905, 923,  339,  3024, 4828, 2991, 5376, 9975, 2509, 6714,
    215,  6293, 5041, 8008, 6334, 8355, 1538, 2350, 7714, 9279, 8965, 6417,
    9418, 520,  4560, 560,  8880, 1023, 9816, 9881, 1822, 7915, 982,  4107,
    6943, 8052, 333,  7860, 4794, 1992, 3490, 1158, 6077, 2496, 152,  7975,
    2501, 2810, 1082, 2813, 8608, 1549, 6666, 2758, 1478, 3239, 3835, 5611,
    4755, 7142, 7374, 7323, 3349, 1169, 7756, 7089, 1615, 6826, 2165, 3830,
    6200, 1136, 3270, 9046, 606,  2619, 1566, 2139, 7549, 4069, 4444, 1646,
    2508, 8844, 1577, 6630, 5857, 4248, 7189, 7303, 619,  7470, 8503, 5653,
    9425, 3113, 5140, 5012, 3020, 6601, 7862, 8998, 8323, 6203, 2833, 1595,
    3185, 5727, 7168, 10,   1775, 7183, 9183, 2718, 6159, 2983, 4409, 5529,
    4761, 136,  1412, 2987, 940,  4636, 8128, 7998, 9344, 6208, 2478, 8022,
    2263, 1531, 7504, 6628, 7268, 8936, 6402, 6190, 792,  8786, 3366, 2838,
    2936, 9526, 3358, 689,  70,   6307, 8770, 3487, 8444, 6785, 7523, 6842,
    6259, 9749, 9606, 3870, 9707, 2565, 8505, 1584, 7150, 6968, 7113, 6403,
    5829, 8766, 6570, 7256, 2824, 9842, 3871, 7233, 4501, 4504, 5942, 816,
    2542, 900,  365,  1308, 6006, 8520, 7225, 4427, 9402, 6454, 7148, 3389,
    5289, 8856, 7882, 3859, 2187, 3999, 1583, 2146, 5545, 4387, 2621, 6741,
    930,  9493, 745,  5321, 9488, 9850, 4915, 3433, 7208, 5848, 6679, 5506,
    3456, 1964, 1895, 467,  5097, 9291, 7173, 4774, 3849, 8476, 9323, 1894,
    6868, 226,  2711, 6161, 9769, 4494, 1225, 5217, 3386, 5879, 3955, 7291,
    6632, 5791, 6055, 1073, 4904, 9271, 8609, 3664, 651,  4481, 8781, 2826,
    1214, 9223, 2510, 8047, 6414, 5303, 3802, 5797, 8696, 9545, 7342, 1265,
    6552, 3618, 9624, 9407, 2293, 7098, 5949, 6429, 8853, 2369, 6723, 4500,
    1003, 4130, 3096, 1031, 4579, 2313, 7405, 2090, 283,  844,  3432, 8818,
    1550, 9453, 282,  1862, 2132, 1985, 2292, 2818, 7589, 1257, 4115, 519,
    1002, 5119, 5871, 7659, 6697, 6620, 6760, 7650, 2495, 2625, 7527, 2835,
    52,   6708, 2406, 6325, 6753, 3194, 6709, 1284, 6359, 5216, 4593, 7273,
    3410, 6172, 7401, 2897, 6754, 3838, 7107, 9210, 9350, 3105, 7389, 8083,
    7955, 5278, 270,  5614, 2799, 728,  6138, 9568, 5811, 6843, 7681, 5718,
    8421, 9692, 9124, 5544, 1858, 7331, 3094, 7096, 1067, 6297, 7409, 6686,
    4223, 562,  4925, 3925, 7231, 2077, 9167, 5087, 3346, 3961, 3242, 1809,
    5841, 7408, 6846, 1575, 1870, 658,  1387, 3238, 6346, 1275, 1614, 3015,
    8778, 2348, 92,   3588, 899,  935,  1103, 222,  4570, 9384, 1444, 4763,
    6921, 803,  6270, 8392, 7713, 7024, 3601, 7889, 5843, 1457, 3709, 6511,
    8532, 1703, 998,  9733, 3557, 977,  9711, 6820, 801,  376,  6215, 9818,
    3778, 8755, 6412, 4363, 7139, 7793, 5629, 3617, 6230, 8862, 2039, 3097,
    4956, 681,  2839, 4984, 6556, 4894, 8872, 6672, 59,   713,  1019, 2365,
    4452, 9981, 4901, 1204, 7737, 3594, 1041, 5032, 7816, 9016, 4932, 4181,
    9579, 7133, 5390, 2489, 9485, 874,  6877, 9280, 1516, 2488, 3863, 3288,
    1076, 3010, 4595, 4346, 7811, 6513, 1912, 7551, 8762, 9922, 999,  2664,
    7270, 9431, 7646, 9062, 5772, 3155, 6245, 3122, 5425, 207,  1443, 6240,
    4039, 2705, 4650, 4325, 4153, 6177, 7906, 5908, 308,  8804, 1331, 1570,
    6050, 6835, 3463, 6447, 9950, 949,  5572, 6566, 7801, 9026, 4799, 161,
    3623, 2114, 5162, 5477, 884,  427,  5447, 3645, 3807, 255,  4773, 5550,
    3297, 2034, 7108, 3978, 8436, 2704, 1224, 7788, 9368, 356,  5479, 1972,
    2445, 158,  7622, 4054, 4940, 5706, 1200, 7769, 9021, 1244, 9146, 7198,
    7465, 8072, 89,   6311, 4702, 2573, 325,  4482, 3092, 5172, 5058, 8129,
    1530, 2354, 1554, 8383, 1682, 2410, 9339, 5761, 4557, 3521, 883,  8507,
    6397, 7037, 3442, 3503, 3507, 2125, 7859, 8437, 8992, 8919, 9998, 4238,
    6086, 8568, 1101, 5982, 1379, 5648, 8322, 3488, 3332, 8898, 6043, 4414,
    9876, 6322, 686,  4276, 6191, 6610, 8654, 6457, 3772, 5559, 5061, 4410,
    787,  6605, 9598, 1258, 6385, 8601, 7390, 416,  9209, 1310, 7950, 555,
    9044, 7064, 825,  2234, 730,  103,  9936, 398,  9670, 9486, 1825, 4264,
    682,  4750, 1128, 4708, 4486, 4025, 6302, 1228, 9142, 4018, 5402, 4163,
    4184, 1306, 3723, 7663, 6455, 2220, 9334, 7616, 1472, 9871, 5924, 2124,
    1800, 6675, 9553, 655,  9551, 2563, 2653, 6277, 9181, 3632, 1635, 5830,
    5700, 5463, 7453, 827,  6874, 7632, 5445, 395,  804,  8559, 1043, 7653,
    6207, 3071, 8663, 4921, 1534, 9030, 7099, 6476, 654,  6981, 6387, 4042,
    9859, 3616, 9443, 7471, 5954, 7495, 5370, 2481, 1865, 3555, 5799, 9059,
    5295, 4489, 2581, 6339, 3450, 9685, 3007, 6852, 5443, 8525, 7985, 8637,
    892,  2280, 7904, 8613, 142,  8914, 4697, 8229, 7487, 1616, 2976, 5101,
    3272, 9518, 7435, 5839, 742,  2466, 9788, 8294, 6104, 6742, 6583, 8275,
    1235, 8112, 8015, 7505, 6301, 9770, 5662, 7266, 2154, 8889, 3064, 2724,
    3460, 5845, 4689, 9740, 3959, 301,  5276, 7407, 514,  3357, 1240, 4578,
    8845, 9091, 3078, 5485, 5175, 4738, 4746, 9298, 6020, 8909, 8904, 4554,
    8085, 5846, 7959, 858,  271,  836,  511,  3120, 6167, 3974, 1263, 2980,
    4349, 2314, 3562, 6940, 458,  490,  3175, 973,  4178, 6048, 1383, 418,
    6534, 7604, 3654, 7809, 6849, 5868, 8214, 2555, 9857, 1942, 9754, 9066,
    8432, 5021, 9416, 9595, 2883, 2938, 2221, 3073, 3061, 259,  902,  8988,
    8153, 8610, 5781, 5421, 1756, 6823, 659,  7279, 497,  6126, 9164, 9845,
    7056, 1140, 8981, 4272, 3411, 8736, 3732, 6565, 3299, 249,  504,  2673,
    7316, 2103, 7147, 9589, 5475, 1751, 5553, 4426, 8158, 6789, 4180, 613,
    5290, 9703, 6975, 8970, 439,  3307, 7207, 833,  4475, 5271, 9516, 8518,
    6848, 5181, 1976, 4259, 5805, 7121, 3681, 669,  7803, 171,  4403, 3216,
    6168, 3929, 4927, 3971, 6832, 9866, 5842, 8491, 4532, 2448, 800,  991,
    9682, 3146, 3675, 3219, 9932, 7766, 4057, 5148, 3037, 2339, 9461, 5459,
    8240, 7871, 8915, 2875, 9228, 5907, 2102, 8446, 8482, 2238, 116,  6751,
    3546, 2250, 925,  9671, 2613, 3425, 1193, 6137, 7027, 6000, 5504, 7754,
    9075, 2274, 5414, 8725, 2253, 9103, 278,  5218, 1172, 6542, 9896, 396,
    9892, 6475, 3210, 8589, 4102, 1789, 6598, 7164, 6800, 4735, 4608, 2629,
    9519, 205,  7767, 9879, 2562, 4739, 635,  7806, 9309, 4555, 4930, 3534,
    7634, 9088, 9931, 1458, 6866, 2596, 3279, 9705, 4322, 4014, 688,  2191,
    6220, 8175, 7051, 3205, 6109, 9313, 673,  3528, 3916, 5360, 1905, 5126,
    8045, 5211, 4810, 3905, 1088, 6783, 2041, 7443, 6518, 4981, 8892, 8327,
    8184, 9877, 5847, 2409, 9040, 5937, 2975, 7884, 2497, 7855, 9381, 7307,
    3290, 8710, 352,  1414, 5416, 5055, 1967, 7611, 8963, 2308, 3831, 7561,
    8957, 423,  7926, 43,   1493, 3981, 9923, 6432, 822,  1051, 2159, 4914,
    6692, 5001, 8698, 5462, 2068, 202,  9125, 6595, 6834, 4249, 9451, 2062,
    639,  2390, 7582, 868,  3057, 5823, 4526, 9290, 3501, 6185, 5092, 4890,
    6114, 2751, 963,  4478, 6799, 3137, 5717, 300,  8168, 9963, 2511, 4733,
    3760, 1037, 6046, 2997, 9633, 4796, 2934, 8533, 7022, 595,  1876, 363,
    7025, 1908, 8124, 628,  28,   5698, 556,  643,  85,   8759, 9701, 4525,
    4582, 4471, 6425, 6750, 5984, 7293, 2327, 7490, 3901, 8540, 5051, 5007,
    94,   2842, 6623, 8921, 1044, 7619, 6506, 4855, 9335, 3869, 2255, 5601,
    919,  1269, 3427, 6521, 9437, 5352, 837,  252,  3394, 9951, 2064, 7013,
    5394, 6796, 1656, 7610, 7559, 8304, 4784, 9712, 2109, 1993, 7710, 2362,
    56,   8493, 22,   8946, 9906, 3271, 7758, 1390, 3673, 410,  8434, 7111,
    2646, 480,  1766, 2752, 678,  7795, 8252, 4217, 9803, 873,  5500, 187,
    5512, 6121, 859,  5377, 6798, 9527, 4871, 9014, 6393, 3718, 799,  8871,
    9080, 5492, 6745, 5266, 2458, 3387, 9198, 2537, 4063, 2206, 8415, 6313,
    9118, 2179, 7083, 3485, 1304, 397,  3257, 130,  740,  8249, 7535, 1133,
    3044, 7433, 2153, 726,  9267, 9036, 961,  4388, 8514, 7043, 7425, 2847,
    1950, 5939, 421,  4880, 7514, 9311, 5956, 2044, 7448, 5530, 6562, 4874,
    1354, 4119, 4273, 8006, 322,  9238, 9446, 2363, 517,  2051, 9213, 1882,
    1798, 7694, 1072, 9084, 9258, 4782, 8148, 4116, 3683, 106,  1203, 4105,
    4542, 3087, 6972, 9894, 4174, 2791, 3547, 5313, 9470, 4318, 2721, 6306,
    2694, 1576, 6485, 972,  3530, 5088, 3612, 6845, 3821, 9506, 6948, 8597,
    4490, 5495, 5270, 7730, 402,  6990, 6173, 4814, 3725, 1061, 7885, 671,
    8771, 5594, 6281, 2512, 1929, 5049, 4052, 3517, 3251, 2205, 5293, 2939,
    6704, 6273, 7138, 724,  7054, 6378, 6604, 9073, 626,  7385, 149,  8934,
    6603, 9604, 5113, 8220, 666,  7671, 2737, 1996, 3148, 4165, 1544, 3475,
    4574, 1149, 1145, 1610, 8420, 6578, 8676, 3101, 6466, 4527, 7808, 1556,
    5023, 4234, 9698, 4111, 7023, 2231, 9577, 741,  3136, 5478, 8481, 8361,
    8592, 8342, 6051, 7732, 6959, 8806, 3419, 9864, 3803, 2223, 1431, 9810,
    1256, 4103, 2841, 7741, 192,  1156, 4487, 8161, 3891, 5773, 7285, 7115,
    2812, 9199, 2754, 2346, 2702, 5412, 5067, 5634, 2147, 324,  3035, 5070,
    2503, 867,  6500, 6304, 1349, 390,  455,  3497, 9205, 8511, 707,  7491,
    7591, 48,   7326, 1604, 6608, 772,  2858, 1619, 4725, 246,  1526, 4186,
    3908, 3674, 7460, 7248, 7768, 1078, 5705, 2493, 9593, 4678, 6069, 5981,
    2933, 1991, 6470, 994,  6458, 2819, 5253, 4303, 598,  5395, 3443, 1673,
    8490, 7550, 3516, 3169, 8890, 8091, 3840, 9138, 5201, 4995, 4803, 7381,
    254,  2870, 5649, 7823, 958,  7642, 9601, 6024, 3510, 1159, 5203, 1343,
    8194, 4931, 3226, 3630, 6487, 7211, 2451, 8790, 7398, 6520, 2785, 8599,
    105,  9540, 4308, 531,  4496, 6558, 9134, 2101, 7953, 7097, 5231, 7004,
    7159, 2424, 1841, 179,  2,    7426, 7626, 6223, 8512, 3188, 1419, 753,
    170,  4148, 1855, 5233, 5358, 6695, 4587, 9836, 8242, 8907, 1040, 5198,
    5084, 7220, 8484, 9797, 5535, 7388, 9454, 8341, 3638, 6356, 443,  5227,
    2094, 4332, 5064, 8911, 6214, 8209, 6896, 6822, 3052, 5631, 3862, 7940,
    8479, 1378, 6434, 9663, 5819, 1401, 7866, 854,  7625, 9360, 3565, 3066,
    3966, 1071, 4037, 5896, 9,    1464, 6292, 7014, 934,  7615, 3834, 1313,
    1791, 4716, 5076, 9274, 717,  154,  4502, 55,   7991, 1286, 1217, 8639,
    4834, 8567, 4424, 913,  3245, 9533, 6515, 6899, 3,    4289, 3710, 1147,
    6516, 7035, 627,  5167, 4911, 1546, 1086, 3781, 2196, 2655, 4049, 1239,
    5764, 4646, 8702, 7934, 9207, 8990, 6115, 1001, 7104, 9449, 1820, 1600,
    196,  8502, 5582, 5322, 6551, 6251, 7844, 6786, 8433, 719,  1290, 8462,
    6004, 5715, 1100, 650,  6280, 3266, 2364, 2642, 5437, 8993, 1411, 7088,
    1469, 7981, 4790, 132,  3634, 8032, 585,  3936, 3744, 5011, 6369, 2300,
    5168, 6810, 3697, 2986, 6108, 1207, 6141, 642,  5096, 1749, 7045, 6492,
    2118, 8673, 8543, 7438, 452,  3340, 6917, 5895, 1924, 6759, 1963, 1164,
    5422, 4056, 4319, 5722, 3799, 1174, 8910, 1611, 7360, 9945, 1624, 3445,
    3824, 2533, 2940, 8208, 8271, 5747, 1216, 8093, 9459, 8251, 9811, 9744,
    7508, 9706, 5836, 3640, 2467, 834,  9110, 7162, 3493, 2917, 6627, 1402,
    783,  2438, 9645, 8760, 7430, 8203, 9710, 4766, 2404, 6637, 9970, 5940,
    2381, 4045, 8185, 5367, 7015, 5207, 2602, 9655, 9195, 8692, 7463, 3117,
    9127, 6034, 1886, 9143, 7317, 586,  6183, 5577, 6117, 7954, 4206, 9969,
    5418, 7639, 5731, 1857, 1479, 6580, 2182, 9588, 4300, 4710, 2199, 9626,
    3458, 393,  1438, 2305, 9259, 5353, 2046, 2450, 2523, 3173, 1312, 8223,
    9022, 2583, 3204, 6617, 6794, 653,  2110, 6267, 6893, 5598, 1126, 5616,
    8727, 6264, 8372, 4396, 3418, 8310, 9959, 8722, 5612, 77,   6936, 6348,
    4900, 8211, 4434, 7790, 3135, 1704, 9452, 2521, 210,  5179, 2285, 1,
    1116, 8253, 4661, 8431, 6980, 7515, 1432, 4868, 7810, 9658, 4145, 4700,
    2609, 4096, 4467, 8857, 7664, 2635, 8497, 4455, 6232, 112,  1179, 9156,
    476,  9445, 4347, 7432, 4660, 1089, 6279, 9239, 3197, 8416, 2281, 3964,
    3627, 3278, 2749, 7334, 5372, 3688, 7333, 7069, 2105, 8156, 6973, 4643,
    2974, 735,  2773, 2993, 6901, 2627, 7925, 2700, 6363, 4800, 3533, 5691,
    4075, 8244, 9837, 4596, 3898, 7571, 4597, 30,   2325, 2927, 9608, 6411,
    5923, 4374, 9718, 6400, 5608, 9690, 737,  3808, 744,  7019, 2639, 2877,
    622,  8913, 2659, 750,  3561, 1437, 5807, 4558, 9380, 8452, 6748, 2186,
    3918, 5433, 3667, 8558, 8412, 4395, 9566, 6581, 2780, 7302, 7411, 2321,
    8026, 3143, 6541, 6016, 2180, 722,  4243, 4242, 6564, 4185, 6920, 9651,
    3714, 5640, 1096, 7637, 518,  8102, 7812, 5869, 4328, 610,  4001, 9054,
    7675, 5120, 7563, 6533, 3179, 5770, 6773, 503,  3032, 649,  3161, 9204,
    2023, 9565, 898,  3106, 4775, 2914, 53,   5988, 2215, 8617, 6063, 5739,
    8349, 6460, 4309, 4437, 1883, 5139, 7258, 6957, 7151, 3511, 8815, 3498,
    3478, 4461, 7759, 6934, 5592, 2341, 747,  1107, 4051, 6691, 1180, 703,
    237,  8068, 732,  8333, 5249, 7607, 7376, 3915, 1057, 4023, 1621, 4845,
    164,  5002, 4575, 8300, 1105, 6654, 6648, 7384, 8695, 9429, 6609, 9748,
    64,   2551, 6361, 7371, 4167, 3814, 3464, 756,  6738, 8580, 3114, 6442,
    2881, 3794, 2777, 4376, 9891, 5576, 4842, 4360, 1311, 6804, 1033, 8510,
    7444, 6405, 5677, 4891, 225,  5170, 2604, 8278, 2769, 9441, 2272, 3104,
    6140, 6642, 9432, 3305, 4265, 269,  4592, 1511, 5017, 5378, 291,  6291,
    7457, 8411, 8723, 6929, 4179, 8499, 3574, 4221, 8224, 7219, 6496, 1474,
    8650, 4769, 9759, 9735, 796,  9999, 2514, 4060, 806,  7542, 6524, 257,
    1557, 4220, 2857, 6217, 3839, 5812, 9934, 4637, 708,  8719, 3977, 2728,
    5697, 1252, 3892, 1958, 1572, 3749, 1450, 2760, 2226, 479,  4970, 4635,
    4852, 1759, 1285, 5022, 7378, 9374, 7700, 5171, 6376, 122,  91,   3941,
    9679, 3952, 3653, 1527, 5882, 3828, 6793, 7007, 1548, 260,  657,  9294,
    2575, 3355, 6352, 971,  9391, 5813, 7532, 8034, 3476, 2502, 1562, 526,
    7282, 2491, 547,  2003, 6205, 7391, 8551, 4266, 3815, 4405, 929,  9133,
    6418, 3182, 3385, 5121, 5716, 3519, 7359, 3189, 4465, 629,  7598, 7095,
    9649, 2057, 4545, 8069, 227,  7074, 7814, 4323, 2504, 6033, 5995, 8841,
    9377, 9901, 8951, 5711, 424,  5864, 4878, 9599, 3736, 8274, 985,  9724,
    8496, 290,  1462, 1195, 1373, 1243, 6889, 9554, 7105, 406,  4440, 2686,
    1292, 8400, 4379, 2476, 307,  1334, 5736, 2658, 2714, 8812, 863,  3019,
    4571, 1448, 2204, 3342, 7560, 3322, 129,  8425, 6898, 8735, 4848, 3125,
    4294, 2620, 9809, 777,  9521, 685,  7633, 2631, 1480, 4756, 5886, 8311,
    2701, 2548, 2893, 2697, 3199, 2727, 1347, 4459, 5173, 9206, 7459, 6884,
    8123, 8717, 4511, 4193, 2357, 1571, 6982, 7654, 2874, 951,  6377, 3158,
    50,   8314, 405,  4603, 9203, 3631, 7017, 3482, 9395, 289,  5188, 2026,
    7785, 554,  8648, 8726, 1014, 9979, 2643, 6388, 2150, 7439, 8018, 1134,
    9074, 751,  8486, 3826, 9802, 5574, 8772, 7995, 414,  9882, 57,   2079,
    8953, 1946, 7594, 9528, 4314, 3200, 9262, 4918, 9427, 3745, 4990, 8263,
    2779, 9861, 8954, 5234, 7628, 5851, 3088, 1693, 1365, 4722, 8193, 2716,
    7818, 5746, 590,  2432, 7678, 7237, 2924, 670,  1849, 5368, 6782, 5787,
    6044, 9960, 1302, 2979, 2001, 9841, 4602, 2230, 7976, 2688, 5455, 6332,
    3399, 9662, 69,   181,  7593, 9667, 7798, 6163, 1547, 1008, 7635, 3110,
    3060, 7239, 9159, 4253, 9799, 5407, 5498, 5470, 9386, 4138, 2335, 9306,
    2922, 3400, 8797, 2981, 1028, 9474, 5671, 8838, 3716, 4433, 966,  8819,
    494,  6567, 4287, 4166, 6211, 716,  134,  8269, 7002, 6227, 1247, 6446,
    9736, 2911, 9908, 7731, 80,   1590, 7573, 4675, 2011, 434,  6394, 623,
    2207, 6740, 183,  6438, 5448, 6991, 2734, 1720, 5809, 5712, 6236, 9500,
    8709, 7061, 3762, 5849, 110,  1380, 3753, 7251, 9849, 6965, 5752, 8453,
    1075, 9034, 4229, 3354, 7719, 3692, 8201, 2598, 6734, 3003, 2506, 5873,
    6465, 633,  5435, 7999, 3646, 306,  4255, 4093, 4101, 3544, 5994, 2640,
    1778, 2569, 140,  206,  2889, 608,  7467, 5912, 7994, 1289, 9289, 922,
    4411, 9699, 2095, 9378, 1956, 5587, 9920, 6809, 3437, 2674, 525,  2740,
    1941, 2419, 944,  8344, 2644, 5513, 9713, 8881, 811,  1776, 6816, 1608,
    2289, 3628, 194,  1980, 2037, 2499, 6498, 3973, 1449, 3048, 7652, 6986,
    6658, 1241, 3563, 5387, 8373, 4112, 5241, 824,  4690, 1607, 6031, 5881,
    7005, 1160, 5976, 9943, 2660, 648,  2800, 1987, 3171, 7689, 8305, 6237,
    5814, 1626, 83,   4378, 5095, 6994, 6870, 6599, 5625, 3887, 6225, 8583,
    5964, 5225, 9823, 8743, 1255, 4886, 7092, 7461, 8456, 7879, 974,  5252,
    3855, 8464, 305,  4663, 4682, 7986, 3252, 1551, 3177, 4856, 5177, 2212,
    7486, 5036, 7466, 3269, 1692, 3379, 9563, 8104, 3969, 8917, 3580, 3885,
    5931, 7361, 4860, 8101, 2276, 321,  4432, 3367, 5905, 7672, 2324, 163,
    1287, 4656, 320,  3337, 2470, 37,   4507, 4191, 5325, 1753, 1904, 2358,
    9161, 3610, 8945, 5052, 1356, 9053, 3527, 7180, 3473, 828,  2932, 762,
    9913, 1669, 2028, 6480, 6303, 7497, 3946, 2937, 663,  6939, 1115, 7736,
    1353, 9835, 428,  5330, 3067, 9869, 8348, 3156, 1650, 6606, 6812, 6535,
    8642, 7309, 5441, 2395, 5035, 1533, 2091, 3327, 8076, 1010, 4605, 2944,
    5713, 7776, 4019, 4897, 9680, 1782, 9731, 9750, 1897, 3042, 7216, 1484,
    9880, 6895, 3791, 1545, 1684, 3181, 5393, 4329, 442,  7820, 9787, 1593,
    5285, 8217, 4406, 411,  3364, 4747, 809,  1359, 3853, 1627, 5548, 914,
    2247, 3932, 2776, 7131, 123,  5930, 1423, 5286, 5595, 3430, 3167, 7337,
    3982, 5104, 1274, 2258, 6663, 5486, 1716, 6635, 369,  3693, 1497, 8628,
    6878, 5105, 593,  3452, 7988, 3246, 7242, 245,  9218, 6472, 4672, 7123,
    4456, 778,  4176, 8079, 5962, 4087, 1652, 4691, 135,  1299, 936,  6193,
    4919, 6550, 1113, 5602, 6856, 7832, 5031, 8678, 1672, 7423, 2684, 2797,
    209,  4726, 6522, 7338, 6764, 5411, 7749, 7073, 3248, 5863, 5380, 7815,
    3142, 978,  9989, 4062, 7313, 4657, 907,  4187, 5567, 2414, 8195, 748,
    953,  7166, 8487, 6772, 6861, 5451, 8671, 3041, 2832, 2022, 9076, 9139,
    8645, 9264, 5194, 1264, 9737, 9581, 9504, 7049, 8109, 7102, 9379, 8285,
    9398, 5541, 5135, 8720, 3051, 378,  8367, 7617, 2294, 3426, 1578, 4385,
    1301, 128,  6687, 2804, 8228, 4959, 4299, 5948, 3729, 2822, 2266, 6312,
    1005, 6507, 1679, 6439, 2559, 2113, 980,  6021, 2904, 8501, 3954, 6891,
    234,  2016, 1874, 1579, 6838, 8764, 7833, 9393, 5695, 1283, 3046, 5945,
    1803, 1665, 385,  2905, 2437, 6765, 6726, 9301, 6916, 1671, 9722, 7554,
    1873, 8923, 9096, 7753, 8571, 2909, 9067, 5927, 5386, 845,  9219, 5283,
    3596, 3318, 4143, 8250, 739,  1827, 3227, 4674, 5947, 6333, 7868, 2849,
    4225, 9400, 5243, 607,  7009, 2208, 4968, 5603, 7484, 6152, 2008, 725,
    9471, 7701, 9961, 667,  4283, 6508, 343,  1917, 5827, 8905, 3201, 4298,
    847,  3502, 3864, 49,   6611, 61,   7063, 7502, 546,  9322, 6421, 4188,
    2709, 8443, 231,  9673, 515,  1794, 3726, 3956, 6797, 1109, 8706, 370,
    3520, 4521, 1026, 1814, 4362, 7196, 4342, 5897, 770,  3576, 229,  7143,
    2913, 9600, 8808, 6622, 9349, 8011, 2130, 1589, 2183, 273,  1597, 2069,
    4742, 188,  7947, 8524, 4728, 299,  6441, 4531, 4824, 5147, 3312, 4015,
    9686, 8572, 3701, 9196, 9752, 9984, 9819, 2000, 6711, 6951, 3292, 5279,
    5730, 5618, 5250, 5915, 8330, 3682, 5299, 327,  5620, 7127, 285,  2175,
    60,   9241, 8846, 5472, 8635, 9912, 6233, 3091, 5419, 1324, 6012, 5161,
    8265, 1695, 9136, 721,  8325, 2042, 5098, 4998, 7636, 2851, 8430, 2941,
    1786, 6717, 5100, 5679, 8162, 8424, 7238, 6586, 4822, 9570, 9057, 2507,
    7314, 8816, 8125, 4659, 3680, 146,  7230, 1323, 3390, 373,  8949, 3860,
    2428, 403,  5802, 5048, 6275, 8141, 5784, 334,  157,  3643, 8515, 5753,
    5009, 7845, 779,  3029, 6955, 2356, 8074, 6677, 6027, 6555, 5578, 8041,
    9576, 3168, 4306, 6918, 7699, 6087, 9347, 4006, 5164, 840,  5163, 5427,
    4680, 5259, 5524, 7354, 2049, 4033, 4783, 2222, 3397, 2648, 5481, 2878,
    1792, 6952, 3867, 199,  7831, 4007, 6517, 9918, 6258, 4633, 2485, 1923,
    1613, 7306, 5192, 641,  9202, 7612, 6766, 5951, 7319, 9789, 3779, 6436,
    1510, 8826, 5651, 1336, 4035, 8281, 1146, 746,  4373, 1714, 1848, 148,
    4117, 6420, 8732, 8114, 5676, 1618, 3039, 8087, 3613, 3672, 6187, 9530,
    6413, 8545, 3128, 1024, 3522, 8777, 4348, 9221, 4679, 6615, 8296, 4453,
    4280, 1056, 5520, 738,  9126, 6053, 9450, 3056, 3040, 5606, 8117, 9154,
    2837, 5301, 2880, 764,  8149, 948,  7657, 5803, 3421, 7669, 8733, 6536,
    159,  9728, 4530, 7708, 8331, 3352, 4654, 7203, 6451, 9371, 638,  9376,
    1201, 3743, 4125, 5131, 7541, 4634, 8428, 1030, 6320, 9191, 7170, 2366,
    2713, 7624, 4887, 2031, 5292, 5783, 4649, 6528, 41,   3549, 2757, 5430,
    6272, 8975, 8121, 632,  3940, 1520, 1890, 1470, 7155, 5189, 6997, 3483,
    802,  2959, 6987, 6715, 4673, 1135, 9434, 9924, 4041, 965,  5136, 468,
    2138, 7945, 3733, 8531, 7722, 1038, 407,  9954, 557,  98,   9719, 3013,
    1098, 156,  8638, 9176, 9355, 177,  6976, 532,  5375, 6693, 8555, 579,
    8606, 5165, 6592, 3360, 6977, 4777, 2708, 6574, 1559, 7347, 2915, 375,
    3700, 4136, 3708, 7645, 4436, 3461, 3927, 8984, 6768, 1268, 7707, 8119,
    8745, 3684, 605,  6531, 9586, 2080, 2960, 9367, 441,  7245, 3600, 6276,
    9502, 710,  3591, 4190, 1417, 7253, 7552, 3265, 172,  9632, 2295, 9618,
    1740, 328,  8791, 6543, 3214, 368,  4644, 7128, 2831, 5801, 7042, 1350,
    8297, 7516, 81,   1205, 4098, 8810, 7299, 8003, 4262, 8232, 5993, 5953,
    5396, 1728, 1433, 8463, 1982, 1947, 2108, 8155, 1788, 6127, 1050, 7743,
    6913, 2574, 8952, 3077, 1984, 8933, 9856, 3392, 2219, 7890, 7209, 354,
    8200, 1222, 1708, 9721, 1467, 8370, 29,   1167, 9741, 8051, 5073, 9596,
    4261, 8266, 9829, 5071, 1069, 5938, 6477, 7169, 5855, 8317, 911,  6860,
    6428, 3451, 3720, 5522, 1409, 4134, 4807, 2213, 5400, 988,  7865, 7300,
    2921, 4515, 2076, 2787, 7294, 1374, 7110, 1698, 8218, 4753, 699,  6938,
    665,  5692, 2738, 6324, 684,  4327, 8036, 3524, 4861, 698,  4239, 4614,
    8009, 3647, 7691, 9042, 7530, 5371, 6064, 3995, 7558, 5085, 9664, 3382,
    5966, 4472, 2715, 9862, 3900, 6907, 7345, 7236, 4514, 7760, 6243, 9508,
    3468, 6188, 9985, 9224, 1237, 1054, 86,   1452, 7339, 5591, 8703, 4170,
    9275, 313,  4961, 6082, 8711, 8357, 7226, 7685, 9197, 2767, 7492, 9636,
    9575, 3812, 5900, 1742, 2043, 2317, 5069, 4305, 5656, 5566, 7656, 3996,
    1090, 5118, 1856, 9345, 20,   7596, 7729, 9807, 5786, 8594, 9511, 8677,
    5306, 1617, 6869, 9756, 3298, 5361, 7193, 65,   7404, 4917, 1333, 3438,
    7667, 5968, 1503, 8466, 5112, 3348, 1281, 9714, 1475, 5143, 7665, 1111,
    8071, 4896, 2657, 8997, 5681, 2580, 6399, 9027, 6249, 6120, 9328, 6266,
    7192, 705,  1586, 7274, 9001, 4386, 2973, 5191, 5934, 2557, 6645, 1413,
    7028, 6066, 1066, 9092, 5316, 256,  5200, 3795, 4786, 5586, 1829, 4142,
    679,  6526, 4779, 7878, 338,  8821, 2691, 5668, 7356, 7949, 7775, 8059,
    2553, 766,  5247, 5793, 115,  1185, 860,  8318, 7506, 3261, 6653, 1585,
    1506, 9785, 4628, 5600, 4297, 6255, 4698, 3027, 7529, 3303, 9709, 3581,
    349,  7751, 4324, 954,  331,  6462, 6463, 9509, 2720, 2774, 4740, 6130,
    5898, 3047, 7804, 9399, 1760, 1818, 8126, 9635, 2765, 1700, 9089, 8840,
    1888, 4987, 4203, 34,   4577, 5450, 6747, 5465, 5828, 5224, 875,  2966,
    3707, 7156, 3730, 267,  6806, 8139, 9854, 5005, 5560, 6408, 1961, 4893,
    1587, 3368, 8534, 7295, 2572, 4357, 7379, 1298, 9538, 3149, 5997, 3930,
    9805, 5332, 8023, 6204, 8730, 4573, 9560, 9851, 1860, 1909, 2531, 3139,
    5673, 2802, 9460, 7546, 7896, 6493, 4576, 9214, 2319, 9117, 8332, 9212,
    465,  2955, 1623, 4009, 4688, 9237, 6213, 3625, 2954, 4958, 8667, 8279,
    7328, 1945, 314,  9058, 1483, 964,  4950, 4664, 3970, 2158, 6206, 6284,
    8467, 946,  7190, 5383, 2224, 3592, 6029, 1959, 8562, 9185, 3818, 4061,
    3537, 9914, 5756, 9792, 8658, 1871, 451,  3737, 2336, 9107, 5977, 2843,
    1395, 1099, 5028, 9409, 9539, 712,  7735, 3566, 6489, 8298, 709,  823,
    4823, 6037, 3184, 7257, 8351, 7841, 820,  3589, 8782, 2060, 23,   6657,
    2748, 8386, 5514, 1977, 3388, 3783, 9200, 5622, 8849, 8734, 454,  8851,
    9174, 8962, 9277, 9546, 5992, 3151, 6060, 3976, 1495, 3249, 5050, 1927,
    8700, 7817, 4331, 2947, 2316, 8272, 6509, 2190, 5740, 5205, 3768, 6873,
    4195, 3585, 4404, 8827, 4737, 1270, 2967, 3579, 8631, 2463, 4353, 4905,
    113,  1802, 371,  1845, 6909, 7574, 8842, 5174, 4443, 1143, 4899, 4022,
    4870, 7247, 8438, 1777, 2059, 5926, 9886, 9182, 5921, 660,  51,   9571,
    2111, 8556, 437,  312,  8472, 3532, 326,  5575, 3564, 3180, 6362, 323,
    5016, 2830, 8122, 2671, 5261, 7579, 9976, 921,  4493, 3895, 575,  1875,
    767,  4881, 9888, 3157, 1830, 6778, 4658, 420,  7167, 4977, 4624, 2416,
    3694, 6950, 1218, 8037, 9751, 5240, 5852, 2286, 5044, 9820, 677,  4047,
    2805, 5965, 1482, 9847, 1755, 7912, 9340, 1013, 8752, 8801, 9782, 9424,
    1018, 5157, 7923, 6002, 7289, 5359, 3383, 287,  6732, 1921, 3540, 8969,
    1420, 7214, 6083, 7197, 7599, 2420, 5392, 9987, 6593, 495,  7584, 4546,
    1305, 5796, 8147, 2907, 4335, 4397, 8150, 9855, 2337, 4491, 8429, 9555,
    9695, 3575, 8138, 4568, 3296, 8529, 2297, 6360, 3163, 3598, 6844, 3404,
    869,  7747, 8918, 5971, 8133, 7564, 6770, 8864, 2803, 917,  6238, 6328,
    2735, 2558, 8935, 3886, 1821, 6335, 7901, 4866, 4345, 7993, 5795, 8239,
    9766, 5077, 8391, 5580, 7592, 6538, 3572, 4836, 4594, 5004, 1866, 9137,
    1293, 3751, 8170, 7641, 4523, 3809, 8379, 483,  2567, 5319, 8748, 9547,
    7570, 8798, 8539, 6202, 2246, 8132, 1178, 5288, 9716, 955,  9316, 3090,
    372,  304,  8780, 5562, 4954, 4312, 200,  818,  5362, 1644, 6685, 5892,
    9330, 4430, 5195, 1297, 429,  7458, 7134, 1762, 3111, 3696, 3554, 1085,
    8553, 4447, 6961, 5429, 5825, 1375, 928,  4792, 9061, 7065, 5919, 5708,
    3259, 5865, 1891, 7847, 9717, 1629, 7779, 5094, 3225, 8942, 5258, 1747,
    5719, 8956, 6166, 5686, 8687, 6532, 5515, 2307, 3132, 2120, 204,  8080,
    6342, 7752, 7918, 8986, 3446, 4036, 4254, 7078, 9438, 1271, 4703, 3230,
    5975, 2425, 9012, 570,  2762, 2085, 5274, 5661, 448,  3848, 9783, 2931,
    2784, 4920, 2505, 3875, 2844, 857,  4788, 3910, 1063, 6448, 5792, 697,
    8847, 3846, 1602, 4818, 6383, 1491, 6096, 7680, 4140, 6113, 6097, 2719,
    8552, 8245, 2906, 3606, 6643, 8099, 9853, 2902, 1735, 8707, 3069, 5998,
    1060, 9524, 7125, 6071, 6967, 1152, 9318, 3147, 8173, 8738, 213,  3361,
    1898, 7651, 6195, 1329, 19,   8108, 6802, 6415, 1434, 7854, 3235, 1661,
    2155, 9958, 2429, 2239, 2440, 2236, 5507, 9052, 6353, 1416, 8267, 1990,
    9928, 6014, 4933, 7961, 7620, 9112, 7357, 216,  7618, 9253, 3928, 3957,
    6409, 691,  6314, 9775};

        public static Vector3 StringToVec3(string vec)
        {
            if (string.IsNullOrEmpty(vec)) return Vector3.zero;
            string[] vals = vec.Trim('(', ')', '\0').Split(',');
            Vector3 vec3 = new Vector3();
            vec3.Set(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));
            return vec3;
        }

        public static string SecendsToTimeString(int totalTimes)
        {
            int hour = totalTimes / 3600;
            int min = (totalTimes - hour * 3600) / 60;
            int secs = (totalTimes - hour * 3600) % 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, secs);
        }

        public static DateTime StampToDateTime(long timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        public static double DateTimeToStamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (time - startTime).TotalSeconds;
        }

        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }

        public static string getHtmlStr(string str, string color)
        {
            return "<color=" + color + ">" + str + "</color>";
        }
    }

    public class RandomNumber
    {
        public RandomNumber(int seeds) { this.seeds = seeds; }

        public static int Min() { return 0; }
        public static int Max() { return 9999; }

        public int GetNextInt()
        {
            int result = Util.GetNextRandomNumber(this.seeds);
            ++seeds;
            return result;
        }

        public double GetNextDouble()
        {
            double result = GetNextInt();
            return result / Max();
        }

        //给定概率[0.0, 1.0]
        //判断是否成功, true表示成功
        public bool Success(double success)
        {
            return this.GetNextInt() <= (success * Max());
        }

        //给定区间[MIN,MAX]
        //返回一个随机数
        public int Random(int min, int max)
        {
            double r = GetNextDouble() * (max - min) + min;
            return (int)r;
        }

        public int seeds;
    }

    //64进制数
    public class Radix64
    {
        public Radix64(string str)
        {
            this.base64 = new StringBuilder(str);
        }

        public void Set(int index, bool v)
        {
            this.Resize(index);
            int div = index / kRadix;
            int mod = index % kRadix;
            int number = GetInt(base64[div]);
            if (v) number |= 1 << mod;
            else number &= ~(1 << mod);

            base64[div] = GetChar(number);
        }

        public bool Get(int index)
        {
            if (base64.Length * kRadix < index) return false;
            int div = index / kRadix;
            int mod = index % kRadix;
            int value = GetInt(base64[div]);
            return (value & (1 << mod)) != 0 ? true : false;
        }
        public override string ToString()
        {
            return this.base64.ToString();
        }
        public static int GetInt(char c)
        {
            Init();
            return kMap2Int[(int)c];
        }
        public static char GetChar(int i)
        {
            return kBase64[i];
        }
        private void Resize(int index)
        {
            if (base64.Length * kRadix > index) return;
            base64.Append(GetChar(0), index / kRadix + 1 - base64.Length);
        }

        private static void Init()
        {
            if (kMap2Int[0] != 0) return;
            for (int i = 0; i < 64; ++i)
            {
                kMap2Int[(int)kBase64[i]] = i;
            }
            kMap2Int[0] = 1;
        }

        private static readonly int kRadix = 6;
        private static int[] kMap2Int = new int[128];
        private static readonly string kBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        private StringBuilder base64;
    }
}