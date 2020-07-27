using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MordorFanficWeb.Models.BaseModels;
using MordorFanficWeb.Models;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface ICompositionService
    {
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity;
        Task CreateAsync<T>(T entity) where T : BaseEntity;
        Task<List<T>> GetAllAsync<T>() where T : BaseEntity;
        Task<List<Composition>> GetAllCompositionsOfAccount(int id);
        Task<Composition> GetCompositionById(int id);
        Task<int> CreateComposition(Composition composition);
        Task<List<Composition>> GetLastAdded();
        Task<List<Composition>> FindInCompositions(string keyword);
    }
}
