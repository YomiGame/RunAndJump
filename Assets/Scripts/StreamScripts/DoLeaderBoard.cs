using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using LitJson;

[DataContract]
public struct PlayerRankeData
    {
        [DataMember]
        public int socre;//存储分数
        [DataMember]
        public string name; //存储名字
    }
    
    public class DoLeaderBoard : MonoBehaviour
    {
        private string JsonPath; //json文件的路径
        
        private List<PlayerRankeData> jsonRankList;

        private JsonData jsonData;
        private void Start()
        {
            JsonPath = Application.persistentDataPath + Config.LeaderboardPath;
            jsonRankList = new List<PlayerRankeData>();
            jsonData = new JsonData();
            UpdataRankJson();

        }
        //保存
        public string SaveToBoard(int socre,string name)
        {
            
            PlayerRankeData _PlayerRankeData;
            _PlayerRankeData.name = name;   
            _PlayerRankeData.socre = socre;
            jsonRankList.Add(_PlayerRankeData);
            if (!File.Exists(JsonPath))
            {
                File.Create(JsonPath);
            }

            jsonRankList.Sort((PlayerRankeData p1, PlayerRankeData p2) => p2.socre.CompareTo(p1.socre));
            string jsonData = JsonMapper.ToJson(jsonRankList);
            Debug.Log(jsonData);
            File.WriteAllText(JsonPath, jsonData);
            Debug.Log("保存成功");
            return jsonData;
        }
        


        //取出
        public List<PlayerRankeData> RankJson()
        {
            return jsonRankList;
         }
        
        //清空缓存
        public void ClearRankJsonFile()
        {
          File.WriteAllText(JsonPath, "");
          //jsonRankList = new List<PlayerRankeData>();
          jsonRankList.Clear();
          
        }
        
        //更新数据
        public void UpdataRankJson()
        {
            if (!File.Exists(JsonPath))
            {
                
                Debug.Log("读取的文件不存在！");
                File.Create(JsonPath);
            }
            else
            {
                string json = File.ReadAllText(JsonPath);
                if (json != "")
                {
                    jsonData = JsonMapper.ToObject(json);
                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        PlayerRankeData _PlayerRankeData;
                        _PlayerRankeData.name = (string) jsonData[i]["name"];
                        _PlayerRankeData.socre = (int) jsonData[i]["socre"];
                        jsonRankList.Add(_PlayerRankeData);
                    }
                }
            }
        }
    }

