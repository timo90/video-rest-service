using System;
namespace VideoLibrary
{
    public class Video
    {
        private int _id;
        private string _name;
        private string _url;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string url
        {
            get { return _url; }
            set { _url = value; }
        }

    }
}
