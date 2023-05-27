using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Negocio.Models
{
    public class Preco
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }

        public static IDatabaseRepositorio<Produto> repositorio = new PostgresRepository<Produto>();

        /* Para facilitar criar a tabela no banco 
         CREATE TABLE IF NOT EXISTS public.precos
            (
                id integer NOT NULL DEFAULT nextval('precos_id_seq'::regclass),
                produtoid integer,
                valor numeric(10,2),
                data date DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT precos_pkey PRIMARY KEY (id),
                CONSTRAINT precos_id_produto_fkey FOREIGN KEY (produtoid)
                    REFERENCES public.produtos (id) MATCH SIMPLE
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
            )
        */

        public void Salvar()
        {
            if (Id > 0)
            {
                repositorio.Update(this);
            }
            else
            {
                Id = repositorio.Insert(this);
            }
        }

        public static List<Preco> Get()
        {
            return repositorio.Get<Preco>();
        }

        public static Preco GetById(int id)
        {
            return repositorio.GetById<Preco>(id);
        }

        public static void Excluir(int id)
        {
            repositorio.Delete<Preco>(id);
        }

    }
}