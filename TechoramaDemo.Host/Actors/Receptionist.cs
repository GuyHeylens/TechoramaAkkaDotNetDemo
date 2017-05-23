﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechoramaDemo.Messages;

namespace TechoramaDemo.Host.Actors
{
    public class Receptionist : UntypedActor
    {
        private readonly Dictionary<string, int> _roomCounts = new Dictionary<string, int>();

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Connect c when string.IsNullOrEmpty(c.UserName):
                    Sender.Tell(new Err("Username can't be null or empty."));
                    break;
                case Connect c:
                    Sender.Tell(new Welcome(c.UserName, _roomCounts));
                    break;
                case JoinRoom j when string.IsNullOrEmpty(j.RoomName) || 
                                     string.IsNullOrEmpty(j.UserName):
                    Sender.Tell(new Err("room name and user name can't be empty."));
                    break;
                case JoinRoom j when _roomCounts.ContainsKey(j.RoomName): // pre-existing room
                    var uriFriendly = Uri.EscapeUriString(j.RoomName);
                    var child = Context.Child(uriFriendly);
                    if (child.IsNobody())
                        child = Context.ActorOf(Props.Create(() => new RoomManager(j.RoomName, Self)), uriFriendly);
                    child.Forward(j);
                    if (_roomCounts.TryGetValue(j.RoomName, out var count))
                    {
                        _roomCounts[j.RoomName] = count + 1;
                    }
                    else
                    {
                        _roomCounts[j.RoomName] = 1;
                    }
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }
    }
}
