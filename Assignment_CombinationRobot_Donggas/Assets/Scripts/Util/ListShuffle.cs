//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Shuffle
//{
//    static public class ShuffleList
//    {
//        public List<T> GetShuffleList<T>(List<T> list)
//        {
//            int shuffleCount = list.Count / 2 + 1;
//            for (int i = 0; i < shuffleCount; ++i)
//            {
//                int ranNum = Random.Range(0, i);

//                T temp = list[i];
//                list[i] = list[ranNum];
//                list[ranNum] = temp;
//            }

//            return list;
//        }
//    }
//}