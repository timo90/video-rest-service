using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VideoLibrary;

namespace DAL
{
    public class DAL
    {
        private static string connString;

        public DAL(string _connString)
        {

            connString = _connString;
        }

        public int add(Video vid)
        {
            try
            {
                IEnumerable<Video> videos = getVideos();
                var latestVid = videos.OrderByDescending(s => s.id).FirstOrDefault();

                string csvLine = (latestVid.id + 1).ToString() + "," + vid.name + "," + vid.url + Environment.NewLine;
                byte[] csvLineBytes = Encoding.Default.GetBytes(csvLine);
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(csvLineBytes, 0, csvLineBytes.Length);
                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);
                    }

                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Write))
                    {
                        ms.WriteTo(file);
                    }
                }

                return (latestVid.id + 1);

            }
            catch (Exception ex)
            {
                //DO SOMETHING WITH THE ERROR
                string err = ex.Message.ToString();
                throw;
            }
        }

        public void update(Video vid)
        {
            try
            {
                List<Video> listVideos = new List<Video>();

                //Read in the csv records - store in a list
                using (var reader = new StreamReader(connString))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        Video newVideo = new Video();
                        newVideo.id = Convert.ToInt32(values[0]);
                        newVideo.name = values[1];
                        newVideo.url = values[2];

                        listVideos.Add(newVideo);
                    }
                }

                //Find the relevant record
                var editVideo = listVideos.FirstOrDefault(x => x.id == vid.id);

                //Remove the old record
                listVideos.Remove(editVideo);

                //Edit the old record
                if (editVideo.id == vid.id)
                {
                    editVideo.name = vid.name;
                    editVideo.url = vid.url;
                }
                //Add the edited record
                listVideos.Add(editVideo);

                //Inefficient way of doing it but it's ok for now
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listVideos.Count(); i++)
                {
                    sb.Append(listVideos[i]);
                    sb.Append(Environment.NewLine);
                }

                //Write the new videos to the csv again
                byte[] csvLineBytes = Encoding.Default.GetBytes(sb.ToString());
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(csvLineBytes, 0, csvLineBytes.Length);
                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);
                    }

                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Write))
                    {
                        ms.WriteTo(file);
                    }
                }

            }
            catch (Exception ex)
            {
                //DO SOMETHING WITH THE ERROR
                string err = ex.Message.ToString();
                throw;
            }
        }

        //Are you sure?
        //Sets the bool against the video to deleted rather than
        //actually removing the video
        public void delete(int id)
        {
            try
            {
                List<Video> listVideos = new List<Video>();

                //Read in the csv records - store in a list
                using (var reader = new StreamReader(connString))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        Video newVideo = new Video();
                        newVideo.id = Convert.ToInt32(values[0]);
                        newVideo.name = values[1];
                        newVideo.url = values[2];

                        listVideos.Add(newVideo);
                    }
                }

                //Find the relevant record
                var editVideo = listVideos.FirstOrDefault(x => x.id == id);

                //Remove the old record
                listVideos.Remove(editVideo);


                //Add the deleted record
                listVideos.Add(editVideo);

                //Inefficient way of doing it but it's ok for now
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listVideos.Count(); i++)
                {
                    sb.Append(listVideos[i]);
                    sb.Append(Environment.NewLine);
                }

                //Write the new videos to the csv again
                byte[] csvLineBytes = Encoding.Default.GetBytes(sb.ToString());
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(csvLineBytes, 0, csvLineBytes.Length);
                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);
                    }

                    using (FileStream file = new FileStream(connString, FileMode.Open, FileAccess.Write))
                    {
                        ms.WriteTo(file);
                    }
                }

            }
            catch (Exception ex)
            {
                //DO SOMETHING WITH THE ERROR
                string err = ex.Message.ToString();
                throw;
            }
        }


        public List<Video> getVideos()
        {
            try
            {
                List<Video> listVideos = new List<Video>();

                //Read in the csv records - store in a list
                using (var reader = new StreamReader(connString))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        VideoLibrary.Video newVideo = new Video();
                        int number;
                        bool intConvert = Int32.TryParse(values[0], out number);
                        if (intConvert)
                        {
                            newVideo.id = number;
                        }
                        else
                        {
                          newVideo.id = 0;  
                        }
                        newVideo.name = values[1];
                        newVideo.url = values[2];

                        if (number > 0)
                        {
                            listVideos.Add(newVideo);
                        }
                        
                    }
                }

                //Return the full list of videos
                return listVideos;
            }
            catch (Exception ex)
            {
                //DO SOMETHING WITH THE ERROR
                string err = ex.Message.ToString();
                throw;
            }
        }


        public Video getTheVideo(int id)
        {
            try
            {
                List<Video> listVideos = new List<Video>();

                //Read in the csv records - store in a list
                using (var reader = new StreamReader(connString))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        Video newVideo = new Video();
                        int number;
                        bool intConvert = Int32.TryParse(values[0], out number);
                        if (intConvert)
                        {
                            newVideo.id = number;
                        }
                        else
                        {
                            newVideo.id = 0;
                        }
                        newVideo.name = values[1];
                        newVideo.url = values[2];

                        listVideos.Add(newVideo);
                    }
                }

                //Find the relevant record
                Video theVideo = listVideos.FirstOrDefault(x => x.id == id);

                //Return the full list of videos
                return theVideo;
            }
            catch (Exception ex)
            {
                //DO SOMETHING WITH THE ERROR
                string err = ex.Message.ToString();
                throw;
            }
        }
    }
}
