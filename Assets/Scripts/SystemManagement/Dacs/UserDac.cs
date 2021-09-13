using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;
using System.Linq;

namespace CGber
{
    public class UserDac
    {
        private Dac dac;

        public UserDac() 
        {
            dac = new Dac();
        }

        public UserModel ReadUserByIdAndPasswd(string userId, string userPassword)
        {
            IList<UserModel> user = dac.ReadUser();

            //return user.Where(u => u.userId == userId && u.userPassword == "123").FirstOrDefault();

            return user.Where(u => u.userId == userId && u.userPassword == userPassword).FirstOrDefault();
        }
    }
}

