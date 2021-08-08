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

        public static IList<UserModel> ReadUser()
        {
            IList<UserModel> user = Dac.User();
            return user;
        }

        public static UserModel ReadByUserId(string userId)
        {
            IList<UserModel> user = Dac.User();

            return user.Where(u => u.userId == userId).FirstOrDefault();
        }
    }
}

