using SportClub.BLL.DTO;


namespace SportClub.BLL.Interfaces
{
    public interface IPost
    {
        Task AddPost(PostDTO postDto);
        Task<PostDTO> GetPost(int id);
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task DeletePost(int id);
        Task UpdatePost(PostDTO a);
    }
}
