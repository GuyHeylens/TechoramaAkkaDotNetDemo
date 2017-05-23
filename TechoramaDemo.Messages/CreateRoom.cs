﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class CreateRoom
    {
        public CreateRoom(string roomName, string userName)
        {
            RoomName = roomName;
            UserName = userName;
        }

        public string UserName { get; }

        public string RoomName { get; }
    }
}
