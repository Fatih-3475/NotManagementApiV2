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


            var result = _noteService.SearchNotes(search);
            return Ok(result);
        }
    }
}