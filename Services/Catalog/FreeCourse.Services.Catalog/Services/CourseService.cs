using Amazon.Runtime.Internal.Auth;
using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.CourseDtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CourseService:ICourseService
{
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper,IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();
        if (courses.Count != 0)
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = [];
        }
        return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses),200);
    }

    public async Task<Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (course==null)
        {
            return Response<CourseDto>.Fail("Course Not Found",404);
        }
        course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
        return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }

    public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();
        if (courses.Count != 0)
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = [];
        }
        return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses),200);
    }

    public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);
        newCourse.CreatedTime=DateTime.Now;
        await _courseCollection.InsertOneAsync(newCourse);
        return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
    }

    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Course>(courseUpdateDto);
        var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);
        if (result==null)
        {
            return Response<NoContent>.Fail("Course Not Found", 404);
        }

        return Response<NoContent>.Success(204);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
        var course = await _courseCollection.DeleteOneAsync(x => x.Id == id);
        return course.DeletedCount>0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Course Not Found", 404);
    }
}