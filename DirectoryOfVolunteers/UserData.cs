using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryOfVolunteers
{
    class UserData
    {
        public string Nickname { get; private set; }
        public string FIO{ get; private set; }
        public byte[] ProfilePicture { get; private set; }

        public UserData(string Nick, byte[] Image, string Name)
        { Nickname = Nick;  ProfilePicture = Image;FIO = Name; }
    }
}
