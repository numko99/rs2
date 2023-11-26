﻿using AutoMapper;
using Iter.Repository;

namespace Iter.Services
{
    public class BaseCrudService<T, TInsert, TUpdate, TGet> : BaseReadService<T, TGet> where TInsert : class where T : class where TGet : class
    {
        private readonly IBaseCrudRepository<T, TInsert, TUpdate, TGet> baseCrudRepository;
        private readonly IMapper mapper;

        public BaseCrudService(IBaseCrudRepository<T, TInsert, TUpdate, TGet> baseCrudRepository, IMapper mapper)
            : base(baseCrudRepository, mapper)
        {
            this.baseCrudRepository = baseCrudRepository;
            this.mapper = mapper;
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await this.baseCrudRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }

            await this.baseCrudRepository.DeleteAsync(entity);
        }

        public virtual async Task Insert(TInsert request)
        {
            var entity = this.mapper.Map<T>(request);
            await this.baseCrudRepository.AddAsync(entity);
        }

        public virtual async Task Update(Guid id, TUpdate request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid request");
            }

            var entity = await this.baseCrudRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid id");
            }

            this.mapper.Map(request, entity);
            await this.baseCrudRepository.UpdateAsync(entity);
        }
    }
}