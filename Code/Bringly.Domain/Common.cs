﻿using Bringly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain.Common
{
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string MessageText { get; set; }
    }

}