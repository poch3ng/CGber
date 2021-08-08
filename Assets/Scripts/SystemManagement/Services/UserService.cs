using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MLAPI;

namespace CGber
{
    public class UserService
    {
        public UserService() { }

        /// <summary>
        /// Read user data by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel ReadUserById(string userId)
        {
            return UserDac.ReadByUserId(userId);
        }
    }
}

