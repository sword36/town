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
        VkApi v = new VkApi();

        ReadOnlyCollection<User> friends;

        public Vk()
        {
            v.Authorize("e28200544deedd6d87856d4671a596103192ea9122f99c1ab28017869f660e4205cd9728939dc074a551f", 47421616);
            
            GetFriends();
        }

        public void GetFriends()
        {
            v.Messages.Send(68095528, false, DateTime.Now.ToString());
            friends = v.Friends.Get(47421616, ProfileFields.All);
        }
    }
}
