using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : Controller
    {
        private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        public async Task<ActionResult<Aluno>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Erro ao obter alunos"); 
            }
        }

        [HttpGet("AlunoPorNome")]
        public async Task<ActionResult<Aluno>> GetAlunosByNome([FromQuery] string nome)
        {
            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);
                if (alunos == null)
                {
                    return NotFound($"Não Existem alunos com criterio {nome}");
                }
                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpGet("{id:int}",Name ="GetAluno")]
        public async Task<ActionResult<Aluno>> GetAluno(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno == null)
                {
                    return NotFound($"Não Existe aluno com od {id}");
                }
                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAluno(Aluno aluno)
        {
            try
            {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno);
            }
            catch 
            {
                return BadRequest("Request inválido");      
            }
        }

        [HttpPut]
        public async Task<ActionResult> Edit(int id,[FromBody] Aluno aluno)
        {
            try
            {
                if (aluno.Id == id)
                {
                    await _alunoService.UpdateAluno(aluno);
                    return Ok($"Aluno com id={id} foi atualizado com sucesso");
                }
                else
                {
                    return BadRequest("Dados inconsistentes");
                }
            }
            catch 
            {
                return BadRequest("Resquest inválio");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);

                if (aluno != null)
                {
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno de id={id} foi excluído com sucesso");
                }
                else
                {
                    return NotFound($"Aluno com id={id} nao encontrado");
                }
            }
            catch 
            {
                return BadRequest("Request invalido");
            }
        }
    }
}
