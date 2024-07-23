using Microsoft.Identity.Client.NativeInterop;
using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPrn.Controller
{
    public class MedicineControll
    {
        public void AddMedicine(string name, string description, int minAge, 
            int typeId, int quantity, decimal price, DateOnly expiredDate, int status)
        {
            var medicine = new Medecine
            {
                Name = name,
                Decription = description,
                MinAge = minAge,
                TypeId = typeId,
                Quantity = quantity,
                Price = price,
                ExpiredDate = expiredDate,
                Status = status
            };
            Prn212medicineContext.INSTANCE.Add(medicine);
            Prn212medicineContext.INSTANCE.SaveChanges();
        }

        public void UpdateMedicine(int id, string name, string description, int minAge,
            int typeId, int quantity, decimal price, DateOnly expiredDate, int status)
        {
            var x = Prn212medicineContext.INSTANCE.Medecines.FirstOrDefault(m => m.Id == id);
            if (x != null)
            {
                x.Name = name;
                x.Decription = description;
                x.MinAge = minAge;
                x.TypeId = typeId;
                x.Quantity = quantity;
                x.Price = price;
                x.ExpiredDate = expiredDate;
                x.Status = status;

                Prn212medicineContext.INSTANCE.Medecines.Update(x);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }

        public void DeleteMedicine(int id)
        {
            var x = Prn212medicineContext.INSTANCE.Medecines.FirstOrDefault(x => x.Id == id);
            if (x != null)
            {
                Prn212medicineContext.INSTANCE.Remove(x);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }

        public void CreateNewTypeMedicine(string name)
        {
            var type = new MedicineType
            {
                Type = name
            };
            Prn212medicineContext.INSTANCE.Add(type);
            Prn212medicineContext.INSTANCE.SaveChanges();   
        }

    }
}
