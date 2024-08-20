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
        var lastPost = _dataContext.Posts.LastOrDefault();
        postModel.Id = lastPost is null ? 1 : lastPost.Id + 1;
        _dataContext.Posts.Add(postModel);
        return postModel;
    }
    public PostModel Update(PostModel postModel)
    {
        var rez = _dataContext.Posts.FindIndex(x => x.Id == postModel.Id);
        _dataContext.Posts[rez] = postModel;
        return postModel;
    }
    public PostModel Get(int id)
    {
        return _dataContext.Posts.FirstOrDefault(x => x.Id == id);
    }
    public IEnumerable<PostModel> GetAll()
    {
        return _dataContext.Posts;
    }
    public void Delete(int id)
    {
        var model = _dataContext.Posts.FirstOrDefault(x => x.Id == id);
        if (model is null) throw new Exception("Post not found");
        _dataContext.Posts.Remove(model);
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
    public List<PostModel> Posts { get; set; } = new List<PostModel>();
}