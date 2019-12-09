using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class ProductDTO:BaseDTO
    {
        public override int? Id { get; set; }
        private string _name;
        private decimal _price;
        public string Name { 
            get 
            {
                if (_name != null)
                {
                    return _name;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                else if (value.Length > 150)
                {
                    throw new ArgumentException();
                }
                else
                {
                    _name= value;
                }
            } 
        }
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    _price = value;
                }
            }
        }
        public string CategoryName { get; set; }
        public string ProviderName { get; set; }
    }
}
