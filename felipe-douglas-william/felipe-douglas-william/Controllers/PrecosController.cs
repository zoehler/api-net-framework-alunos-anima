using Negocio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace felipe_douglas_william.Controllers
{
    public class PrecosController : ApiController
    {
        public static List<Preco> precos = new List<Preco>();

        // GET api/precos
        public IEnumerable<Preco> Get() 
        {
            return precos;
        } 

        // GET api/precos/{id}
        public Preco Get(int id) 
        {
            return Preco.GetById(id);
        }
        

        // POST api/precos
        public HttpResponseMessage Post(Preco preco) 
        {
            preco.Salvar();

            var response = Request.CreateResponse(HttpStatusCode.Created, preco); 
            response.Headers.Location = new Uri(Request.RequestUri + "/" + preco.Id); return response; 
        } 

        // PUT api/precos/{id}
        public HttpResponseMessage Put(int id, Preco preco) 
        { 
            var existingPreco = Preco.GetById(id); ;
            if (existingPreco != null) 
            {
                preco.Salvar();
                return Request.CreateResponse(HttpStatusCode.OK); 
            } 
            else 
            { 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Preço not found.");
            } 
        } 

        // DELETE api/precos/{id}
        public HttpResponseMessage Delete(int id) 
        { 
            var preco = Preco.GetById(id);
            if (preco != null)
            {
                Produto.Excluir(id);
                return Request.CreateResponse(HttpStatusCode.OK); 
            }
            else 
            { 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Preço not found.");
            }
        } 
    }
}