using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    internal class DBconnection
    {
        public static ShopEntities shop = new ShopEntities();
        public static User user = new User();
    }
}
