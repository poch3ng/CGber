using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CGber
{
    public class Dac
    {
        public static IList<UserModel> User()
        {
            IList<UserModel> users = new List<UserModel>
            {
                new UserModel
                {
                    userId = "11324",
                    userName = "謝東儒 教授"
                },
                new UserModel
                {
                    userId = "106820039",
                    userName = "邱柏政"
                },
                new UserModel
                {
                    userId = "110820001",
                    userName = "吳奕萱"
                },
                new UserModel
                {
                    userId = "110820002",
                    userName = "葉芳宇"
                },
                new UserModel
                {
                    userId = "110820003",
                    userName = "范凱鈞"
                },
                new UserModel
                {
                    userId = "110820004",
                    userName = "黃新哲"
                },
                new UserModel
                {
                    userId = "110820005",
                    userName = "王柏雄"
                }
            };
            return users;
        }
    }
}

