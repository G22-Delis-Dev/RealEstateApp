using AutoMapper;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Domain.Common;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public abstract class GenericService<TViewModel, TEntity> : IGenericService<TViewModel>
    where TEntity : BaseEntity
    where TViewModel : class
{
    protected readonly IBaseRepository<TEntity> _repository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    protected GenericService(IBaseRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public virtual async Task<IEnumerable<TViewModel>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TViewModel>>(entities);
    }

    public virtual async Task<TViewModel?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : _mapper.Map<TViewModel>(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new Common.Exceptions.NotFoundException(typeof(TEntity).Name, id);

        _repository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}