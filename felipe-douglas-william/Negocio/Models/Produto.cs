using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Negocio.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public static IDatabaseRepositorio<Produto> repositorio = new PostgresRepository<Produto>();

        /* Para facilitar criar a tabela no banco 
         CREATE TABLE IF NOT EXISTS public.produtos
            (
                id integer NOT NULL DEFAULT nextval('produtos_id_seq'::regclass),
                nome character varying(100) COLLATE pg_catalog."default",
                descricao character varying(100) COLLATE pg_catalog."default",
                CONSTRAINT produtos_pkey PRIMARY KEY (id)
            )
        */

        public void Salvar()
        {
            if(Id > 0)
            {
                repositorio.Update(this);
            }
            else
            {
                Id = repositorio.Insert(this);
            }
        }

        public static List<Produto> Get()
        {
            return repositorio.Get<Produto>();
        }

        public static Produto GetById(int id)
        {
            return repositorio.GetById<Produto>(id);
        }

        public static void Excluir(int id)
        {
            repositorio.Delete<Produto>(id);
        }
    }

}