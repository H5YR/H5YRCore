using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5YR.Core.Models
{

    public class TweetModel
    {
        
        public string Content { get; set; }
        public string Username { get; set; }
        public string ScreenName { get; set; }
        public string Avatar { get; set; }
        public DateTime TweetedOn { get; set; }
        public int NumberOfTweets { get; set; }  
        public string ReplyToTweet { get; set; }
        public string Url { get; set; }

    }

}
