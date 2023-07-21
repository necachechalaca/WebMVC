    using System.Collections.Generic;
    using webmvc.Models;

 namespace webmvc.Services
 {
    public class ProdcutServices :  List<ProdcutModel>
    {
        public ProdcutServices()
        {
            this.AddRange(new ProdcutModel[]{
                new ProdcutModel(){productId = 1, productName = "IphoneX", Price =100},
                 new ProdcutModel(){productId = 2, productName = "SamSung", Price =300},
                  new ProdcutModel(){productId = 3, productName = "Xiaomi", Price =400},

            });
        }
    }
 }