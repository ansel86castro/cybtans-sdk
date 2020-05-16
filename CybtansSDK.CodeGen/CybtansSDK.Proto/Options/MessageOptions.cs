﻿#nullable enable

using CybtansSdk.Proto.AST;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CybtansSdk.Proto.Options
{

    public class MessageOptions : ProtobufOption
    {
        public MessageOptions() : base(OptionsType.Message)
        {
        }

        [Field("base")]
        public string? Base { get; set; }

        [Field("deprecated")]
        public bool Deprecated { get; set; }
    }
}
