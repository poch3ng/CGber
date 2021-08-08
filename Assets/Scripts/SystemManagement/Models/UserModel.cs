using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGber
{
    public class UserModel
    {
        public string userId { get; set; }

        public string userName { get; set; }

        public string userPassword { get; set; }

        public ulong? clientId { get; set; }
    }
}