using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebService.Controllers
{

    public class WebServiceController : ApiController
    {
        public DAL.DAL dal;
        private string connString;
        //private VideoLibrary.Video video;
        private IEnumerable<VideoLibrary.Video> videos;

        // GET: api/webservice
        public string GetVideos()
        {
            //Change to your pref
            //id,name,url
            connString = @"/Files/videos.csv";
            dal = new DAL.DAL(connString);

            videos = dal.getVideos();
            string json = new JavaScriptSerializer().Serialize(videos);
            return json;
        }

        // GET: api/webservice/#
        [ResponseType(typeof(WebServiceController))]
        public IHttpActionResult GetVideo(int id)
        {
            connString = @"/Files/videos.csv";
            dal = new DAL.DAL(connString);

            VideoLibrary.Video vid = dal.getTheVideo(id);
            if (vid == null)
            {
                return NotFound();
            }

            return Ok(vid);
        }


        // PUT: api/webservice/#
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVideo(int id, VideoLibrary.Video video)
        {
            connString = @"/Files/videos.csv";
            dal = new DAL.DAL(connString);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != video.id)
            {
                return BadRequest();
            }

            try
            {
                dal.update(video);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/webservice
        [ResponseType(typeof(WebServiceController))]
        public IHttpActionResult PostVideo(Object model)
        {
            connString = @"/Files/videos.csv";
            dal = new DAL.DAL(connString);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoLibrary.Video video = new VideoLibrary.Video();

            var jsonString = model.ToString();
            video = JsonConvert.DeserializeObject<VideoLibrary.Video>(jsonString);

            int newID = dal.add(video);

            return CreatedAtRoute("DefaultApi", new { id = newID }, video);
        }

        // DELETE: api/webservice/#
        [ResponseType(typeof(WebServiceController))]
        public IHttpActionResult DeleteVideo(int id)
        {
            connString = @"/Files/videos.csv";
            dal = new DAL.DAL(connString);

            VideoLibrary.Video vid = dal.getTheVideo(id);
            if (vid == null)
            {
                return NotFound();
            }

            dal.delete(id);

            return Ok(vid);
        }
    }
}