using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IPostService _postService;
    public UsersController(IPostService postService)
    {
        _postService = postService;
    }
    [HttpPost]
    public PostModel Create(PostModel postModel)
    {
        return _postService.Create(postModel);
    }
    
    [HttpPatch]
    public PostModel Update(PostModel postModel)
    {
        return _postService.Update(postModel);
    }
    
    [HttpGet("{id:int}")]
    public PostModel Get(int id)
    {
        return _postService.Get(id);
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var rez = _postService.GetAll();
        return Ok(rez);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _postService.Delete(id);
        return Ok();
    }
}

public interface IPostService
{
    PostModel Create(PostModel postModel);
    PostModel Update(PostModel postModel);
    PostModel Get(int id);
    IEnumerable<PostModel> GetAll();
    void Delete(int id);
}

public class PostService : IPostService
{
    private readonly MyDataContext _dataContext;
    public PostService(MyDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public PostModel Create(PostModel postModel)
    {
        var posts = _dataContext.Posts.ToList();
        var lastPost = posts?.OrderBy(x => x.Id).LastOrDefault();
        postModel.Id = lastPost is null ? 1 : lastPost.Id + 1;
        posts?.Add(postModel);
        _dataContext.Posts = posts;
        return postModel;
    }
    public PostModel Update(PostModel postModel)
    {
        if (_dataContext.Posts.Count == 0) throw new Exception("Post not found");
        var posts = _dataContext.Posts.ToList();
        var rez = posts.FindIndex(x => x.Id == postModel.Id);
        posts[rez] = postModel;
        _dataContext.Posts = posts;
        return postModel;
    }
    public PostModel Get(int id)
    {
        return _dataContext.Posts.FirstOrDefault(x => x.Id == id)!;
    }
    public IEnumerable<PostModel> GetAll()
    {
        return _dataContext.Posts;
    }
    public void Delete(int id)
    {
        var posts = _dataContext.Posts.ToList();
        var model = posts.FirstOrDefault(x => x.Id == id);
        if (model is null) throw new Exception("Post not found");
        posts!.Remove(model);
        _dataContext.Posts = posts;
    }
}

public class PostModel
{
    public int Id { get; set; }
    public string Header { get; set; }
    public string Blog { get; set; }
}

public class MyDataContext
{
    private const string _fileName = "posts.json";
    public List<PostModel> Posts
    {
        get => Deserialize();
        set => Serialize(value);
    }
    public MyDataContext()
    {
    }

    private List<PostModel> Deserialize()
    {
        var jsonString = File.ReadAllText(_fileName);
            return JsonSerializer.Deserialize<List<PostModel>>(jsonString) ?? new List<PostModel>();
    }

    private void Serialize(List<PostModel> newCollection)
    {
        var result = JsonSerializer.Serialize(newCollection);
        File.WriteAllText(_fileName, result);
    }
}