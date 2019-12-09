using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class CategoryDTO: BaseDTO
    {
        public override int? Id { get; set; }
        private string _name;
        public string Name
        {
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
                else if(value.Length > 150)
                {
                    throw new ArgumentException();
                }
                else
                {
                    _name = value;
                }
            }
        }
    }
}
