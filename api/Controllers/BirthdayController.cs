using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Dtos.Birthday;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace api.Controllers
{
    [Route("api/birthdays")]
    [ApiController]
    public class BirthdayController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public BirthdayController(ApplicationDBContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var birthdaysDto = await _context.Birthday
                .Where(b => (b.Date.Month == today.Month && b.Date.Day >= today.Day) || b.Date.Month-today.Month==1 || (b.Date.Month==12 && today.Month==1))
                .OrderBy(b => b.Date.Month)
                .ThenBy(b => b.Date.Day)
                .ToListAsync();

            return Ok(birthdaysDto);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(){
            var birthdays = await _context.Birthday.ToListAsync();
            var birthdaysDto = birthdays.Select(s => s.ToBirthdayDto());
            return Ok(birthdaysDto);
        }

        [HttpGet]
        [Route("search/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var birthday = await _context.Birthday.FindAsync(id);
            if (birthday==null)
            {
                return NotFound();
            }
            return Ok(birthday.ToBirthdayDto());
        }

        [HttpPost]
        [Route("new")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(string name, DateOnly date, IFormFile file)
        {
            CreateBirthdayRequestDto birthdayDto = new CreateBirthdayRequestDto();
            if (name == null)
            {
                return BadRequest("Birthday data is required.");
            }

            var supportedTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
            if (!supportedTypes.Contains(file.ContentType))
            {
                return BadRequest("Unsupported file type. Please upload an image file (JPEG, PNG, GIF, BMP, WEBP).");
            }

            var birthdayModel = birthdayDto.ToBirthdayFromCreateDto();

            birthdayModel.Name=name;
            birthdayModel.Date=date;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                birthdayModel.FileName = file.FileName;
                birthdayModel.FileData = memoryStream.ToArray();

            }


            _context.Birthday.Add(birthdayModel);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetById), new { id = birthdayModel.Id }, birthdayModel.ToBirthdayDto());
        }


        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, string? name = null, DateOnly? date = null, IFormFile? file = null)
        {
            
            var birthdayModel = await _context.Birthday.FirstOrDefaultAsync(x => x.Id == id);

            if (birthdayModel==null)
            {
                return NotFound();
            }

            if (name != null)
            {
                birthdayModel.Name=name;
            }

            if (date != null)
            {
                birthdayModel.Date=date.Value;
            }

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    birthdayModel.FileName = file.FileName;
                    birthdayModel.FileData = memoryStream.ToArray();

                }
            }
            
            await _context.SaveChangesAsync();

            return Ok(birthdayModel.ToBirthdayDto());

        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var birthdayModel = await _context.Birthday.FirstOrDefaultAsync(x => x.Id == id);

            if(birthdayModel==null)
            {
                return NotFound();
            }

            _context.Birthday.Remove(birthdayModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}