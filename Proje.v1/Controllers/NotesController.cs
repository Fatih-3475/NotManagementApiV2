using Microsoft.AspNetCore.Mvc;
using Proje.v1.Dtos;
using Proje.v1.Services;

namespace Proje.v1.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpGet]
        public ActionResult GetNotes([FromQuery] int page = 1, [FromQuery] int pageSize = 5,[FromQuery] string? search = null)
        {
            if (page < 1) return BadRequest("Page değeri 1 veya daha büyük olmalıdır.");
            if (pageSize < 1 || pageSize > 50) return BadRequest("PageSize değeri 1 ile 50 arasında olmalıdır.");
            var value = _noteService.GetNotes(page, pageSize,search);
            return Ok(value);
        } 

        [HttpPost]
        public IActionResult CreateNote(CreateNoteDto dto)
        {
            var newNote = _noteService.CreateNote(dto);

            return CreatedAtAction(
                nameof(GetNoteById),
                new { id = newNote.Id },
                newNote
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            var note = _noteService.GetNoteById(id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var result = _noteService.DeleteNote(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotes(int id, UpdateNoteDto dto)
        {
            var result = _noteService.UpdateNote(id, dto);

            if (!result)
                return NotFound("Not bulunamadı!");

            return Ok("Not güncellendi");
        }

        [HttpGet("search")]
        public IActionResult SearchNotes([FromQuery] string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Search boş olamaz!!!");

            var result = _noteService.SearchNotes(search);
            return Ok(result);
        }
    }
}