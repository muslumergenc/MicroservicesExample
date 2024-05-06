using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.CategoryDtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CategoryService:ICategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;
    public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _mapper = mapper;
        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
    }

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryCollection.Find(_category => true).ToListAsync();
        return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
    }
    public async Task<Response<CategoryDto>> CreateAsync(Category category)
    {
        await _categoryCollection.InsertOneAsync(category);
        return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
    }
    public async Task<Response<CategoryDto>> GetByIdAsync(string id)
    {
        var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        return category is null 
            ? Response<CategoryDto>.Fail("Category not found",404) 
            : Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
    }
}