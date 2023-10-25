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
        public string Position { get; private set; }
        public string Clubs_Sections { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        public UserData(string Nick, byte[] Image, string Name, string Post, string CS, string AddPhone, string AddEmail)
        { Nickname = Nick;  ProfilePicture = Image;FIO = Name; Position = Post; Clubs_Sections = CS; Phone = AddPhone; Email = AddEmail; }
    }
}
