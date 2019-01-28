using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        /// <summary>
        /// TODO

        internal static int? GetCategoryId(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

           var categories = db.Categories.Where(a=>a.Name == name).SingleOrDefault();
            if (categories == null)
            {
                Category category = new Category()
                {
                    Name = name
                   
                };
                db.Categories.InsertOnSubmit(category);
                db.SubmitChanges();
            }
            return categories.CategoryId;
        }
        internal static void AddAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }
        internal static Animal  GetAnimalByID(int ID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.Animals.Where(a => a.AnimalId == ID).Single();
            
        }
        internal static void Adopt(Animal animal, Client client)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Adoption adoption = new Adoption
            {
                AnimalId = animal.AnimalId,
                ClientId = client.ClientId,
                AdoptionFee = 75,
                PaymentCollected = true
            };

            if (adoption.PaymentCollected == true)
            {
                adoption.ApprovalStatus = "true";
                animal.AdoptionStatus = "Adopted";
            }
            db.Adoptions.InsertOnSubmit(adoption);
            db.SubmitChanges();

        }
        internal static void RemoveAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal deletethis= db.Animals.Single(a => a == animal);
            Adoption adoption = db.Adoptions.Single(a => a == animal.Adoptions.Single());
            Room room = db.Rooms.Single(a => a == animal.Rooms.Single());
            db.Animals.DeleteOnSubmit(deletethis);
            db.Adoptions.DeleteOnSubmit(adoption);
            room.AnimalId = null;

            db.SubmitChanges();
        }

        internal static int? GetDietPlanId(string diet)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            var categories = db.DietPlans.Where(a => a.FoodType == diet).Single();

            return categories.DietPlanId;
        
        }

        internal static List<AnimalShot> GetShots(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var shots = db.AnimalShots.Where(s => s.AnimalId == animal.AnimalId).ToList();
            return shots;
        }
        internal static void UpdateShot(string v, Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var shotid = db.Shots.Where(s => s.Name == v.ToLower()).Single();
            var hasShot = db.Animals.Where(n => n.AnimalId == animal.AnimalId).Single();

            var animalshot = db.AnimalShots.Where(a => a.AnimalId == hasShot.AnimalId && a.ShotId == shotid.ShotId).SingleOrDefault();
            if (animalshot== null)
            {
                AnimalShot animalShot = new AnimalShot() { ShotId = shotid.ShotId,
                                                                                    Animal = animal,
                                                                                    AnimalId = hasShot.AnimalId,
                                                                                    DateReceived = DateTime.Now,
                                                                                    Shot = db.Shots.Where(s => s.Name == v.ToLower()).Single() };
                db.AnimalShots.InsertOnSubmit(animalShot);
               
            }
            else
            {
                animalshot.DateReceived = DateTime.Now;
                
            }
            db.SubmitChanges();
        }
            
        internal static void RunEmployeeQueries(Employee employee, string v)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAdoption(bool v, Adoption adoption)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            throw new NotImplementedException();
        }

        internal static Room GetRoom(int animalId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var roomID = db.Rooms.Where(s => s.AnimalId == animalId).SingleOrDefault();
            return roomID;
           
        }

        internal static List<Animal> SearchForAnimalByMultipleTraits()
        {
            throw new NotImplementedException();
        }
        internal static void EnterAnimalUpdate(Animal animal, Dictionary<int, string> updates)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Existing Code Base Do Not Make Changes to
        /// </summary>
        /// <returns></returns>
        internal static List<USState> GetStates()
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
             HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }

        internal static List<Client> GetClients()
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Client newClient = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                Password = password,
                Email = email
            };

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address
                {
                    AddressLine1 = streetAddress,
                    AddressLine2 = null,
                    Zipcode = zipCode,
                    USStateId = stateId
                };

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address
                {
                    AddressLine1 = clientAddress.AddressLine1,
                    AddressLine2 = null,
                    Zipcode = clientAddress.Zipcode,
                    USStateId = clientAddress.USStateId
                };

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if(employeeFromDb == null)
            {
                throw new NullReferenceException();            
            }
            else
            {
                return employeeFromDb;
            }            
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
             HumaneSocietyDataContext  db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }
    }
}