using Entity;
using DTO;

namespace Common.Mappers
{
    public static class CategoryMapper
    {
        public static Category MapDtoToEntity(CategoryDTO dto)
        {
            Category entity = new Category()
            {
                ID = dto.ID,
                Name = dto.Name
            };

            return entity;
        }

        public static CategoryDTO MapEntityToDto(Category entity)
        {
            CategoryDTO dto = new CategoryDTO()
            {
                ID = entity.ID,
                Name = entity.Name
            };

            return dto;
        }
    }
}
