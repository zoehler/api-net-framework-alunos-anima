using felipe_douglas_william.Models;
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
            return precos.FirstOrDefault(p => p.Id == id);
        }
        

        // POST api/precos
        public HttpResponseMessage Post(Preco preco) 
        { 
            preco.Id = GenerateId();
            precos.Add(preco); 
            var response = Request.CreateResponse(HttpStatusCode.Created, preco); 
            response.Headers.Location = new Uri(Request.RequestUri + "/" + preco.Id); return response; 
        } 

        // PUT api/precos/{id}
        public HttpResponseMessage Put(int id, Preco preco) 
        { 
            var existingPreco = precos.FirstOrDefault(p => p.Id == id);
            if (existingPreco != null) 
            { 
                existingPreco.ProdutoId = preco.ProdutoId; 
                existingPreco.Valor = preco.Valor; 
                existingPreco.Data = preco.Data;
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
            var preco = precos.FirstOrDefault(p => p.Id == id); 
            if (preco != null) 
            { 
                precos.Remove(preco);
                return Request.CreateResponse(HttpStatusCode.OK); 
            }
            else 
            { 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Preço not found.");
            }
        } 

        private int GenerateId() 
        { 
            return precos.Count > 0 ? precos.Max(p => p.Id) + 1 : 1; 
        }
    }
}