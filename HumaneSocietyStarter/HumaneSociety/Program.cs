﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            // PointOfEntry.Run();        

            // Query.AddAnimal(animal);
              var animal = Query.GetAnimalByID(4);
            //var client =   Query.GetClient("msconv", "Deamon1!");
            // Query.Adopt(animal, client);
            // Query.GetAnimalByID(3);
            //Query.GetCategoryId("Dog");
            // Query.GetDietPlanId("chow");
            //Query.RemoveAnimal(animal);
            //Query.GetShots(animal);
           // Query.UpdateShot("booster", animal);
                Query.EnterAnimalUpdate(animal,null);
        }
    }
}
