using System.Collections;
using System.Linq;
using Azure.Core;
using Chat.Domain.Contexts;
using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Repositories
{
    public class EfRepository:IRepository
    {
        private readonly ChatDbContext db;
        public EfRepository(ChatDbContext _db)
        {
            db= _db;
        }
        public Guid? ApiKey { get; set; }
        public async Task<T> Create<T>(T entity) where T : BaseEntity
        {
            db.Set<T>().Add(entity);
            return entity;
        }

        public async Task CreateRange<T>(ICollection<T> entity) where T : BaseEntity
        {
            db.Set<ICollection<T>>().AddRange(entity);
        }

        public async Task<ICollection<T>> ReadAll<T>(ISpecification<T> specification = null,int? skip=null, int? take=null) where T : BaseEntity
        {
	        IQueryable<T> Request = db.Set<T>().Where(i => i.ApiKey == this.ApiKey);
			//if (typeof(T) == typeof(IEnumerable<Roles>))
			//{

			//}
			//if (typeof(T) == typeof(IEnumerable<UsersService>))
			//{

			//}
			//if (typeof(T) == typeof(IEnumerable<Users>))
			//{

			//}      
			if (typeof(T) == typeof(ChatHistory))
            {
	            if (specification != null && skip != null && take != null)
	            {
		            Request = db.Set<T>().OrderByDescending(x => x.CreateAt).Where(specification.Criteria).Skip(skip ?? new int()).Take(take ?? new int()).OrderBy(x=>x.CreateAt);
		            var rr = await db.Set<T>().Where(specification.Criteria).CountAsync();
				}
	            else if (specification != null)
	            {
		            Request = db.Set<T>().OrderByDescending(x=>x.CreateAt).Where(specification.Criteria);
	            }
	            return await Request.ToListAsync();
			}

            
            if (specification != null && skip != null && take != null)
            {
                Request = db.Set<T>().Where(specification.Criteria).Skip(skip ?? new int()).Take(take ?? new int());
            }
            else if (specification != null)
            {
                Request = db.Set<T>().Where(specification.Criteria);
            }
            else if (skip != null && take != null)
            {
	            Request = db.Set<T>().Skip(skip ?? new int()).Take(take ?? new int());
            }
			else 
            {
	            Request = db.Set<T>();
            }

			return await Request.ToListAsync();
        }

        public async Task<T> ReadById<T>(long id) where T : BaseEntity
        {
            var query = db.Set<T>().SingleOrDefault(a => a.ApiKey == this.ApiKey && a.Id == id);
            return query;
        }



        public async Task Update<T>(T entity) where T : BaseEntity
        {
            db.Update(entity);
        }

        public async Task UpdateRange<T>(ICollection<T> entity) where T : BaseEntity
        {
            db.UpdateRange(entity);
        }

        public async Task Remove<T>(T entity) where T : BaseEntity
        {
            db.Remove(entity);
        }

        public async Task RemoveRange<T>(IEnumerable<T> entity) where T : BaseEntity
        {
            db.RemoveRange(entity);
        }

        public async Task SaveChange()
        {
            await db.SaveChangesAsync();
        }

    

        public async Task<IEnumerable<long?>> GetUniqueIds()
        {
	       
			var request = await db.Set<ChatHistory>().Select(i => i.ToUserId ).Distinct().ToListAsync();
			return request;
        }

        public async Task<long> GetCount<T>(ISpecification<T> specification = null) where T : BaseEntity
		{
			var request = await db.Set<T>().Where(specification.Criteria).CountAsync();
			return request;
		}
    }
}
