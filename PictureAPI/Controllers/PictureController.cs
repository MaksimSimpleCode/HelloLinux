using HelloLinux.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using PictureAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PictureAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PictureController : ControllerBase
    {
        private static Logger _logger = LogManager.GetLogger("PictureController");
        PictureContext db;

        public PictureController(PictureContext context)
        {
            db = context;
        }

        //TODO Реализовать метод добавления картинки
        //Реализовать аутентификацию


        /// <summary>
        /// Get all pictures
        /// </summary>
        /// <remarks>
        /// remark...
        /// </remarks>
        /// <param>No</param>
        /// <returns>Returns array</returns>
        /// <response code="200">Success</response>
        /// <response code="401">No succes</response>
        // GET api/picture/all
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<FileContentResult>>> Get()
        
        {
            var pictures = await db.Pictures.ToListAsync();
            List<FileContentResult> contentResult = new List<FileContentResult>();
            foreach (var picture in pictures)
            {
                Convert.ToBase64String(picture.PictureData);

                contentResult.Add(File(picture.PictureData, "image/jpeg"));
            }
            _logger.Debug($"Запросили все картинки");
            return contentResult;
        }

        // GET api/picture/cf3d90d2-2b8d-4da9-8cf7-ab17d3e576e5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> Get(Guid id)
        {
            Picture picture = await db.Pictures.FirstOrDefaultAsync(x => x.Id == id);
            if (picture == null)
                return NotFound();
            _logger.Debug($"Запросили картинку: {picture.Id}");
            return File(picture.PictureData, "image/jpeg");
        }

        // GET api/picture/random
        [HttpGet]
        [Route("random")]
        public async Task<ActionResult<Picture>> Random()
        {
            Random rnd = new Random();
            var pictures = await db.Pictures.ToListAsync();
            Picture picture = pictures.OrderBy(x => rnd.Next()).Take(1).First();

            if (picture == null)
                return NotFound();
            _logger.Debug($"Запросили случайную картинку {picture.Id}");
            return File(picture.PictureData, "image/jpeg");
        }

        // Delete api/picture/delete/cf3d90d2-2b8d-4da9-8cf7-ab17d3e576e5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Picture>> Delete(Guid id)
        {
            Picture picture = db.Pictures.FirstOrDefault(x => x.Id == id);
            if (picture == null)
            {
                return NotFound();
            }
            db.Pictures.Remove(picture);

            await db.SaveChangesAsync();
            _logger.Debug($"Удалили картинку: {picture.Id}");
            return Ok(picture);
        }
    }
}
