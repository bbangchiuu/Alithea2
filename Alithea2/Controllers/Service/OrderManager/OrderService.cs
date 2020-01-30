using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alithea2.Controllers.Respository.OrderManager;
using Alithea2.Models;
using Microsoft.Ajax.Utilities;

namespace Alithea2.Controllers.Service.OrderManager
{
    public class OrderService : BaseService<Order>
    {
        private OrderRespository _orderRespository = new OrderRespository();

        public List<Order> listPagination(int? page, int? limit, string start, string end)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }

            if (start.IsNullOrWhiteSpace() || end.IsNullOrWhiteSpace())
            {
                return _orderRespository.listPagination((page.Value - 1) * limit.Value, limit.Value);
            }

            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-1);
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return _orderRespository.listPaginationOrderByDate((page.Value - 1) * limit.Value, limit.Value, startTime, endTime);
        }

        public int ListCountOrder(string start, string end)
        {
            if (start.IsNullOrWhiteSpace() || end.IsNullOrWhiteSpace())
            {
                return _orderRespository.GetAll().Count();
            }

            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-1);
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return _orderRespository.ListCountOrderByDate(startTime, endTime);
        }
    }
}