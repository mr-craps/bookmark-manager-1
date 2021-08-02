using Entity;
using DTO;

namespace Common.Mappers
{
    public static class BookmarkMapper
    {
        public static Bookmark MapDtoToEntity(BookmarkDTO dto)
        {
            Bookmark entity = new Bookmark()
            {
                ID = dto.ID,
                URL = (dto.URL.Trim().StartsWith("http://") || dto.URL.Trim().StartsWith("https://"))
                        ? dto.URL.Trim() 
                        : "http://" + dto.URL.Trim(),
                ShortDescription = dto.ShortDescription,
                CategoryId = dto.CategoryId
            };            

            return entity;
        }

        public static BookmarkDTO MapEntityToDto(Bookmark entity)
        {
            BookmarkDTO dto = new BookmarkDTO()
            {
                ID = entity.ID,
                URL = (entity.URL.Trim().StartsWith("http://") || entity.URL.Trim().StartsWith("https://"))
                        ? entity.URL.Trim()
                        : "http://" + entity.URL.Trim(),
                ShortDescription = entity.ShortDescription,
                CategoryId = (int)entity.CategoryId,
                CategoryName = entity.Category?.Name
            };            

            return dto;
        }
    }
}
