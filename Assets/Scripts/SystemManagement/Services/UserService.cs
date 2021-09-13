using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MLAPI;
using System.Security.Cryptography;
using System;

namespace CGber
{
    public class UserService
    {
        private UserDac dac;

        public UserService() 
        {
            dac = new UserDac();

        }

        /// <summary>
        /// Read user data by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel ReadUserByIdAndPasswd(string userId, string userPassword)
        {
            string shaPasswd = ShadowUserPassword(userPassword);

            return dac.ReadUserByIdAndPasswd(userId, shaPasswd);
        }

        private string ShadowUserPassword(string userPassword)
        {
            string salt = "c8769";

            // concat salt and password
            StringBuilder inp = new StringBuilder();
            inp.Append(salt);
            inp.Append(userPassword);

            SHA512 sha512 = new SHA512CryptoServiceProvider();
            byte[] source = Encoding.UTF8.GetBytes(inp.ToString());
            byte[] crypto = sha512.ComputeHash(source);

            StringBuilder sb = new StringBuilder();

            foreach (Byte b in crypto)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

