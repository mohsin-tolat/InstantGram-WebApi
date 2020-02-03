using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBModels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class ActivityDto
    {
        public PostDto Post { get; set; }

        public UserDto ActivityDoneByUser { get; set; }

        public DateTime ActivityDoneOn { get; set; }

        public bool IsLike { get; set; }

        public bool IsComment { get; set; }
    }
}