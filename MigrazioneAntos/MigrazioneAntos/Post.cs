using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrazioneAntos
{
    public class Post
    {
        public long id;
        public string url;
        public string html_url;
        public string title;
        public string details;
        public long author_id;
        public bool pinned;
        public bool featured;
        public bool closed;
        public string status;
        public int vote_sum;
        public int vote_count;
        public int comment_count;
        public int follower_count;
        public long topic_id;
        

        public DateTime created_at;
        public DateTime updated_at;
    }
}
