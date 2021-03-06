using Microsoft.AspNetCore.Mvc;
using ProAgil.Repository;
using ProAgil.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ProAgil.WebAPI.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        public readonly IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var eventos = await _repo.GetAllEventosAsync(true);

                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }

        }
        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources" , "Imagens");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0) {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\""," ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }
            return BadRequest("Erro ao tentar realizar upload");
        }
        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {

            try
            {
                var evento = await _repo.GetAllEventosAsyncById(EventoId, true);
                var results = _mapper.Map<EventoDto>(evento);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {

            try
            {
                var eventos = await _repo.GetAllEventosAsyncByTema(tema, true);
                
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repo.Add(evento);
                if (await _repo.SaveChangerAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }
            return BadRequest();
        }
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {

            try
            {
                var evento = await _repo.GetAllEventosAsyncById(EventoId, false);
                if (evento == null) return NotFound();


                var idlotes = new List<int>();
                var idRedesSociais = new List<int>();

                model.Lotes.ForEach(item => idlotes.Add(item.Id));
                model.RedesSociais.ForEach(item => idRedesSociais.Add(item.Id));

                
                
                var lotes = evento.Lotes.Where(lote => !idlotes.Contains(lote.Id)).ToArray();

                var redesSociais = evento.RedesSociais.Where(rede => !idlotes.Contains(rede.Id)).ToArray();

                if(lotes.Length > 0) _repo.DeleteRange(lotes);

                if(redesSociais.Length > 0) _repo.DeleteRange(redesSociais);

                _mapper.Map(model, evento);
                _repo.Update(evento);

                if (await _repo.SaveChangerAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }

            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }
        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {

            try
            {

                var evento = await _repo.GetAllEventosAsyncById(EventoId, false);
                if (evento == null) return NotFound();

                _repo.Delete(evento);

                if (await _repo.SaveChangerAsync())
                {
                    return Ok();
                }
            }

            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }
    }
}