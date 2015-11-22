using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;
using System.Diagnostics;



namespace townWinForm
{
    public class Vk
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        VkApi v = new VkApi();

        List<string> friends;

        public Vk()
        {
            v.Authorize("e28200544deedd6d87856d4671a596103192ea9122f99c1ab28017869f660e4205cd9728939dc074a551f", 47421616);
            
            //GetFriends();
        }

        public void GetFriends()
        {
            friends = new List<string>();
            //v.Messages.Send(68095528, false, DateTime.Now.ToString());
            var f = v.Friends.Get(47421616);
            
            foreach (var u in f)
            {
                User us = v.Users.Get(u.Id);
                friends.Add(us.FirstName + " " + us.LastName);
            }

            /*var a = v.Friends.Get(68095528, ProfileFields.All).ToList();

            for (int i = 0; i < a.Count; i++)
            {
                if (friends.Contains(a[i]))
                    a.RemoveAt(i);
                else friends.Add(a[i]);
            }*/
        }

        public string GetUserName()
        {
            /*
            int index = rand.Next(friends.Count);

            string res = friends[index];

            friends.RemoveAt(index);

            return res;
            */
            return "123";
        }
    }
}
