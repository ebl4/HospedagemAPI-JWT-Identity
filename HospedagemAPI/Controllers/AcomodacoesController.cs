using Business;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospedagemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class AcomodacoesController : ControllerBase
    {
        private AcomodacaoService _service;

        public AcomodacoesController(AcomodacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Acomodacao> ListarTodos()
        {
            return _service.ListarTodos();
        }

        [HttpPost]
        public Resultado Post([FromBody] Acomodacao acomodacao)
        {
            return _service.Incluir(acomodacao);
        }
    }
}
