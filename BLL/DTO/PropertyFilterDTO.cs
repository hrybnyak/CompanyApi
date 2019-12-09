using ExpressionBuilder.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class PropertyFilterDTO
    {
        public string PropertyId { get; set; }
        public Operation Operation { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
        public FilterStatementConnector Connector { get; set; }
    }
}
