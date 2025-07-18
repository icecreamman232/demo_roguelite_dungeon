using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;


namespace SGGames.Script.Dungeon
{
    public class RoomRewardGenerator
    {
        private static int S_ITEM_ROOM_INDEX = 2; 
        private static int S_WEAPON_ROOM_INDEX = 5;
        private static int S_MIN_CURRENCY_NUMBER = 1;
        private static int S_MAX_CURRENCY_NUMBER = 2;
        private List<Global.RoomRewardType> m_resultList = new(9);
        
        public List<Global.RoomRewardType> ResultList => m_resultList;

        public void GenerateRoomReward(int maxReward)
        {
            ResetGenerator();
            FillDefaultValue(maxReward);
            
            //Guaranteed reward room
            FillItemRoom(); //There is always 1 room
            FillWeaponRoom(); //There is always 1 room
            FillSpecialRoom();//There is always 1 room
            FillFinalRoom();//There is always 1 room
            
            //Random reward room
            FillCoinRoom(maxReward); //There are random 1-2 rooms
            FillBombRoom(maxReward);//There are random 1-2 rooms
            FillKeyRoom(maxReward);//There are random 1-2 rooms
            FillRemainingRoom(maxReward);
        }

        private void ResetGenerator()
        {
            m_resultList.Clear();
        }
        
        private void FillDefaultValue(int maxReward)
        {
            for (int i = 0; i < maxReward; i++)
            {
                m_resultList.Add(Global.RoomRewardType.None);
            }
        }

        private void FillItemRoom()
        {
            m_resultList[S_ITEM_ROOM_INDEX] = Global.RoomRewardType.Item;
            
        }
        
        private void FillSpecialRoom()
        {
            var specialRoomType = Random.Range((int)Global.RoomRewardType.Random,
                (int)Global.RoomRewardType.WeaponSmith + 1);
            
            //Hand pick room to be special room here
            var indexList = new List<int>()
            {
                3,6,7,
            };
            
            var choseIndex = Random.Range(0, indexList.Count);
            m_resultList[indexList[choseIndex]] = (Global.RoomRewardType)specialRoomType;
        }

        private void FillWeaponRoom()
        {
            m_resultList[S_WEAPON_ROOM_INDEX] = Global.RoomRewardType.Weapon;
        }

        private void FillFinalRoom()
        {
            m_resultList[^1] = Global.RoomRewardType.Item;
        }

        private void FillCoinRoom(int maxReward)
        {
            var randomNumber = Random.Range(S_MIN_CURRENCY_NUMBER, S_MAX_CURRENCY_NUMBER + 1);
            while (randomNumber > 0)
            {
                var randomIndex = Random.Range(0, maxReward);
                if (m_resultList[randomIndex] != Global.RoomRewardType.None) continue;
                m_resultList[randomIndex] = Global.RoomRewardType.Coin;
                randomNumber--;
            }
        }
        
        private void FillKeyRoom(int maxReward)
        {
            var randomNumber = Random.Range(S_MIN_CURRENCY_NUMBER, S_MAX_CURRENCY_NUMBER + 1);
            while (randomNumber > 0)
            {
                var randomIndex = Random.Range(0, maxReward);
                if (m_resultList[randomIndex] != Global.RoomRewardType.None) continue;
                m_resultList[randomIndex] = Global.RoomRewardType.Key;
                randomNumber--;
            }
        }
        
        private void FillBombRoom(int maxReward)
        {
            var randomNumber = Random.Range(S_MIN_CURRENCY_NUMBER, S_MAX_CURRENCY_NUMBER + 1);
            while (randomNumber > 0)
            {
                var randomIndex = Random.Range(0, maxReward);
                if (m_resultList[randomIndex] != Global.RoomRewardType.None) continue;
                m_resultList[randomIndex] = Global.RoomRewardType.Bomb;
                randomNumber--;
            }
        }

        private void FillRemainingRoom(int maxReward)
        {
            for (int i = 0; i < maxReward; i++)
            {
                if (m_resultList[i] != Global.RoomRewardType.None) continue;
                var roomType = Random.Range((int)Global.RoomRewardType.Coin, (int)Global.RoomRewardType.Item);
                m_resultList[i] = (Global.RoomRewardType)roomType;
            }
        }
    }
}

