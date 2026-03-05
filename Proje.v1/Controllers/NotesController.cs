
using Microsoft.AspNetCore.Mvc;
using Proje.v1.Data;
using Proje.v1.Models;

namespace Proje.v1.Controllers
{
        [ApiController]
        [Route("api/notes")]
        public class NotesController:ControllerBase
        {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }
            [HttpGet]
            public IActionResult GetNotes()
            {
                var result = _context.Notes
                    .Where(x => !x.IsArchived)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                return Ok(result);
            }

           [HttpPost]
           public IActionResult CreateNote(CreateNoteDto dto)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest("Title boş olamaz");

                if (string.IsNullOrWhiteSpace(dto.Content) || dto.Content.Length < 5)
                    return BadRequest("Content en az 5 karakter olmalı");

                var newNote = new Note
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    IsArchived = false,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Notes.Add(newNote);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetNotes),
                    new { id = newNote.Id },
                    newNote
                );
            }
            [HttpGet("{id}")]
             public IActionResult GetNoteById(int id)
             {
              var note = _context.Notes.FirstOrDefault(x => x.Id == id && !x.IsArchived);
                if (note == null)
                    return NotFound();
                return Ok(note);
                }

            [HttpDelete("{id}")]
            public IActionResult DeleteNote(int id)
            { 
            var note = _context.Notes.FirstOrDefault(x => x.Id == id);
                if (note == null) 
                 return NotFound();

                note.IsArchived = true;
            _context.SaveChanges();
            return NoContent();
            }

            [HttpGet("search")]
            public IActionResult SearchNotes([FromQuery] string? search)
            {
                if (string.IsNullOrWhiteSpace(search))
                    return BadRequest("Search boş olamaz!!!");
                var result = _context.Notes
                    .Where(x => !x.IsArchived  && x.Title != null && x.Title.Contains(search) )
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                return Ok(result);
            }
        }
    }
