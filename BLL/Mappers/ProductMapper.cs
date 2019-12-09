using BLL.DTO;
using DAL.Models;
using DAL.UnitOfWork;
using System.Linq;
using System;
namespace BLL.Mappers
{
    public class ProductMapper : MapperBase<Product, ProductDTO>
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        public override Product Map(ProductDTO element)
        {
            return new Product
            {
                Name = element.Name,
                Price = element.Price,
                CategoryId = _unitOfWork.CategoryRepository.Get((c => c.Name == element.CategoryName)).FirstOrDefault()?.Id,
                ProviderId = _unitOfWork.ProviderRepository.Get((pr => pr.Name == element.ProviderName)).FirstOrDefault()?.Id
            };
        }

        public override ProductDTO Map(Product element)
        {
            return new ProductDTO
            {
                Name = element.Name,
                Price = element.Price,
                CategoryName = _unitOfWork.CategoryRepository.GetById(element.CategoryId ?? throw new ArgumentNullException()).Name,
                ProviderName = _unitOfWork.ProviderRepository.GetById(element.ProviderId ?? throw new ArgumentNullException()).Name
            };
        }
    }
}
