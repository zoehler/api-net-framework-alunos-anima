using felipe_douglas_william.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace felipe_douglas_william.Controllers
{
    public class ProdutosController : ApiController
    {
        private static List<Produto> produtos = new List<Produto>();
        // GET api/produtos
        public IEnumerable<Produto> Get()
        {
            return produtos;
        }

        // GET api/produtos/{id}
        public Produto Get(int id)
        {
            return produtos.FirstOrDefault(p => p.Id == id);
        }

        // POST api/produtos
        public HttpResponseMessage Post(Produto produto)
        {
            produto.Id = GenerateId(); produtos.Add(produto);
            var response = Request.CreateResponse(HttpStatusCode.Created, produto);
            response.Headers.Location = new Uri(Request.RequestUri + "/" + produto.Id);
            return response;
        }

        // PUT api/produtos/{id}
        public HttpResponseMessage Put(int id, Produto produto)
        {
            var existingProduto = produtos.FirstOrDefault(p => p.Id == id);
            if (existingProduto != null)
            {
                existingProduto.Nome = produto.Nome;
                existingProduto.Descricao = produto.Descricao;
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
            var produto = produtos.FirstOrDefault(p => p.Id == id); 
            if (produto != null) 
            { 
                produtos.Remove(produto); 
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
            return PrecosController.precos.Where(p => p.ProdutoId == id);
        }

        // GET api/produtos/{id}/precos
        [HttpGet]
        [Route("api/produtos/produto-venda/{id}")]
        public IHttpActionResult GetPrecoAtual(int id)
        {
            var ultimoPreco = PrecosController.precos
                            .Where(p => p.ProdutoId == id)
                            .OrderByDescending(p => p.Data)
                            .Take(1) ;
            if(ultimoPreco == null)
            {
                return NotFound();
            }
            return Ok(ultimoPreco);
        }

        private int GenerateId() 
        {
            return produtos.Count > 0 ? produtos.Max(p => p.Id) + 1 : 1; 
        }

    }
}
