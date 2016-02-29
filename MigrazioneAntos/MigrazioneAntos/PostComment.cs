using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrazioneAntos
{
    public class PostComment
    {
        public long id;
        public string url;
        public string html_url;
        public string body;
        public long author_id;
        public long post_id;
        public bool official;
        public int vote_sum;
        public int vote_count;
        public DateTime created_at;
        public DateTime updated_at;
    }
}
