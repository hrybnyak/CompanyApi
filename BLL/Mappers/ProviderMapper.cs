using System;
using BLL.DTO;
using DAL.Models;

namespace BLL.Mappers
{
    public class ProviderMapper : MapperBase<Provider, ProviderDTO>
    {
        public override Provider Map(ProviderDTO element)
        {
            return new Provider
            {
                Name = element.Name,
                PhoneNumber = element.PhoneNumber
            };
        }

        public override ProviderDTO Map(Provider element)
        {
            return new ProviderDTO
            {
                Name = element.Name,
                PhoneNumber = element.PhoneNumber
            };
        }
    }
}
