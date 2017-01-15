using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcessoDados
{
    public interface IRepositorio<T> where T : class,  new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
   
        IQueryable<T> ObterTodos();
        T Obter(int id);
        IQueryable<T> Obter(Expression<Func<T, bool>> predicate);
        void Atualizar(T entity);
        void Excluir(T entity);
        void Adicionar(T entity);

        /*
        IQueryable<T> GetAll();
        IQueryable<T> Get(Func<T, bool> predicate);
        T Find(params object[] key);
        void Atualizar(T obj);
        void SalvarTodos();
        void Adicionar(T obj);
        void Excluir(Func<T, bool> predicate);*/
    }
}
