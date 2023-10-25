using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
   public class PostService :IPost
    {
        IUnitOfWork Database { get; set; }

        public PostService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddPost(PostDTO pDto)
        {
            var a = new Post()
            {
                Name = pDto.Name               
            };
            await Database.Posts.AddItem(a);
            await Database.Save();
        }
        public async Task<PostDTO> GetPost(int id)
        {
            Post a = await Database.Posts.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
             return new PostDTO
             {
                 Id = a.Id,
                 Name = a.Name,
             };
        }
        public async Task<IEnumerable<PostDTO>> GetAllPosts()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Post, PostDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(await Database.Posts.GetAll());
            }
            catch { return null; }
        }
        public async Task DeletePost(int id)
        {
            await Database.Posts.Delete(id);
            await Database.Save();
        }
        public async Task UpdatePost(PostDTO a)
        {
            Post post = await Database.Posts.Get(a.Id);
            post.Name = a.Name;
            await Database.Posts.Update(post);
            await Database.Save();
        }
    }
}
