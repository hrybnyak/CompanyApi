using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;
using BLL.DTO;
namespace BLL.Mappers
{
    public class CategoryMapper : MapperBase<Category, CategoryDTO>
    {
        public override Category Map(CategoryDTO element)
        {
            return new Category
            {
                Name = element.Name
            };
        }

        public override CategoryDTO Map(Category element)
        {
            return new CategoryDTO
            {
                Id = element.Id,
                Name = element.Name
            };
        }
    }
}
