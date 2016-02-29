using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrazioneAntos
{
    public class ArticleAttachment
    {
        public int id;
        public int article_id;
        public string file_name;
        public string content_url;
        public string content_type;
        public long size;
        public bool inline;
  }
}   