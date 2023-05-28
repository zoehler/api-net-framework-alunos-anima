using Negocio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public partial class ProdutosController : ApiController
    {
        // GET api/produtos
        public IEnumerable<Produto> Get()
        {
            return Produto.Get();
        }

        // GET api/produtos/{id}
        public Produto Get(int id)
        {
            return Produto.GetById(id);
        }

        // POST api/produtos
        public HttpResponseMessage Post(Produto produto)
        {
            produto.Salvar();

            var response = Request.CreateResponse(HttpStatusCode.Created, produto);
            response.Headers.Location = new Uri(Request.RequestUri + "/" + produto.Id);
            return response;
        }

        // PUT api/produtos/{id}
        public HttpResponseMessage Put(int id, Produto produto)
        {
            
            var existingProduto = Produto.GetById(id);
            if (existingProduto != null)
            {
                produto.Salvar();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto not found.");
            }
        }

        // DELETE api/produtos/{id}
        public HttpResponseMessage Delete(int id) 
        {
            var produto = Produto.GetById(id);
            if (produto != null) 
            {
                Produto.Excluir(id);
                return Request.CreateResponse(HttpStatusCode.OK); 
            } 
            else 
            { 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto not found.");
            } 
        }

        // GET api/produtos/{id}/precos
        [HttpGet]
        [Route("api/produtos/{id}/precos")]
        public IEnumerable<Preco> GetPrecos(int id)
        {
            return Preco.Get().Where(p => p.ProdutoId == id);
        }

        // GET api/produtos/{id}/precos
        [HttpGet]
        [Route("api/produtos/produto-venda/{id}")]
        public IHttpActionResult GetPrecoAtual(int id)
        {
            var ultimoPreco = Preco.Get()
                            .Where(p => p.ProdutoId == id)
                            .OrderByDescending(p => p.Data)
                            .Take(1) ;
            if(ultimoPreco == null)
            {
                return NotFound();
            }
            return Ok(ultimoPreco);
        }

        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Max-Age", "86400"); // Tempo de cache das opções em segundos

            return response;
        }

    }
}
