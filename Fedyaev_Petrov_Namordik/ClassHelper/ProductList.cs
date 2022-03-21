using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fedyaev_Petrov_Namordik.EF
{
    public partial class VM_ProductList
    {
        public string GetTypeAndName { get => $"Тип продукта: {TypeProduct} | Наименование продукта: {NameProduct}"; }
        public string GetArticul { get => $"Артикул: {Articul}"; }
        public string GetMaterial { get => $"Материал: {Material}"; }
        public string GetCost { get => $"Стоимость: {Cost}"; }

        public string GetColor
        {
            get
            {
                if (QtyLastSaleProduct == 0)
                {
                    return "#ff4c5b";
                }
                else 
                {
                    return "#FDBD40";
                }
            }
        }
    }

}

