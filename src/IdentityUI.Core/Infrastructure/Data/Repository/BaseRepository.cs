﻿using SSRD.IdentityUI.Core.Data.Entities;
using SSRD.IdentityUI.Core.Data.Models;
using SSRD.IdentityUI.Core.Infrastructure.Data.Extensions;
using SSRD.IdentityUI.Core.Interfaces.Data;
using SSRD.IdentityUI.Core.Interfaces.Data.Repository;
using SSRD.IdentityUI.Core.Interfaces.Data.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSRD.IdentityUI.Core.Infrastructure.Data.Repository
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly IdentityDbContext _context;

        public BaseRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public bool Add(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            int changes = _context.SaveChanges();

            return changes > 0;
        }

        public virtual bool Exist(IBaseSpecification<TEntity> specification)
        {
            return _context
                .Set<TEntity>()
                .ApplayBaseSpecification(specification)
                .Any();
        }

        public virtual TEntity Get(IBaseSpecification<TEntity> specification)
        {
            return _context
                .Set<TEntity>()
                .ApplayBaseSpecification(specification)
                .FirstOrDefault();
        }

        public TData Get<TData>(ISelectSpecification<TEntity, TData> specification)
        {
            return _context
                .Set<TEntity>()
                .ApplaySelectSpecification(specification)
                .FirstOrDefault();
        }

        public List<TData> GetList<TData>(ISelectSpecification<TEntity, TData> specification)
        {
            return _context
                .Set<TEntity>()
                .ApplaySelectSpecification(specification)
                .ToList();
        }

        public PaginatedData<TData> GetPaginated<TData>(IPaginationSpecification<TEntity, TData> specification)
        {
            List<TData> data = _context
                .Set<TEntity>()
                .ApplyPaginationSpecification(specification)
                .ToList();

            int count = _context
                .Set<TEntity>()
                .ApplayBaseSpecification(specification)
                .Count();

            return new PaginatedData<TData>(
                data: data,
                count: count);
        }

        public bool Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            int changes = _context.SaveChanges();

            return changes > 0;
        }
    }
}