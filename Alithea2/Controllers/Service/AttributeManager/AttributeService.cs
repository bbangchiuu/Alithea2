using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.AttributeManager;

namespace Alithea2.Controllers.Service.AttributeManager
{
    public class AttributeService : BaseService<Models.Attribute>
    {
        private AttributeRepository _attributeRepository = new AttributeRepository();

        public List<Models.Attribute> GetAttributesOfProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return _attributeRepository.GetAttributesOfProduct(id.Value);
        }
    }
}