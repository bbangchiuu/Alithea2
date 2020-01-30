using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alithea2.Controllers.Respository.AttributeManager
{
    public class AttributeRepository : BaseRepository<Models.Attribute>
    {
        public List<Models.Attribute> GetAttributesOfProduct(int id)
        {
            var attribute = new List<Models.Attribute>();
            try
            {
                attribute = _db.Attributes.Where(a => a.ProductID == id).ToList();
                return attribute;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}