using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace CGber
{
    public class Dac
    {
        public Dac() { }

        public IList<UserModel> ReadUser()
        {
            using (StreamReader r = new StreamReader("Assets/Scripts/SystemManagement/Dacs/user_shadow.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<UserModel>>(json);
            }
        }
        
    }
}

