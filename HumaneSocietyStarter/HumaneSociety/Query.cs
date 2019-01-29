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
        public delegate void EmployeeDelegate(Employee employee, string v);
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

        internal static int? GetDietPlanId(string diet,int cups,string FoodType)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            var categories = db.DietPlans.Where(a => a.Name == diet).SingleOrDefault();


            if (categories == null)
            {
                DietPlan category = new DietPlan()
                {
                    Name = diet,
                    FoodAmountInCups = cups,
                    FoodType = FoodType
                };
                db.DietPlans.InsertOnSubmit(category);
                categories = category;
            }
            db.SubmitChanges();
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
           
            EmployeeDelegate employeeDelegate;

            switch (v)
            {
                case "update":
                    employeeDelegate = UpdateEmployee;
                    break;
                case "read" :
                    employeeDelegate = ReadEmployee;
                    break;
                case  "delete":
                    employeeDelegate = DeleteEmployee;
                    break;
                case "create":
                    employeeDelegate = CreateEmployee;
                    break;
            }
            
            throw new NotImplementedException();
        }
        internal static void UpdateEmployee(Employee employee, string v)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var checkagainst = db.Employees.Where(a => a.EmployeeId == employee.EmployeeId).Single();
            if (CheckEmployeeUserNameExist(checkagainst.UserName))
            {
                checkagainst.Email = employee.Email;
                checkagainst.FirstName = employee.FirstName;
                checkagainst.LastName = employee.LastName;
                checkagainst.Email = employee.Email;
            }

            db.SubmitChanges();
        }
        internal static void ReadEmployee(Employee employee, string v)
        {
            RetrieveEmployeeUser(employee.Email, (int)employee.EmployeeNumber);
        }
        internal static void DeleteEmployee(Employee employee, string v)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Employees.DeleteOnSubmit(employee);
            db.SubmitChanges();
        }
        internal static void CreateEmployee(Employee employee, string v)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Employees.DeleteOnSubmit(employee);
            db.SubmitChanges();
        }
        internal static void UpdateAdoption(bool v, Adoption adoption)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var adopted = db.Adoptions.Where(s => s.AnimalId == adoption.AnimalId).SingleOrDefault();
            adopted.ApprovalStatus = v.ToString();
            db.SubmitChanges();

        }

        internal static Room GetRoom(int animalId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var roomID = db.Rooms.Where(a => a.RoomNumber != null && a.AnimalId == animalId).SingleOrDefault();
           // var updateroom = db.Rooms.Where(a => a.RoomNumber != null).SingleOrDefault();
            if (roomID.AnimalId == null )
            {
                roomID.AnimalId = animalId;
                db.SubmitChanges();
            }
             
            
            return roomID;

        }
        internal static void SetRoom(int animalID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

        }
    internal static List<Animal> SearchForAnimalByMultipleTraits()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
           var  searchfor =  UserInterface.GetAnimalCriteria();
            List<Animal> list = new List<Animal>();
            foreach (var item in searchfor)
            {
                switch (item.Key)
                {
                    case 1:
                        var category = db.Animals.Where(a => a.Name == item.Value);
                        list.AddRange(category);
                        break;
                    case 2:
                        var name = db.Animals.Where(a => a.Name == item.Value);
                        list.AddRange(name);
                        break;
                    case 3:
                        var age = db.Animals.Where(a => (int)a.Age == Convert.ToInt32( item.Value));
                        list.AddRange(age);
                        break;
                    case 4:
                        var Demeanor = db.Animals.Where(a => a.Demeanor == item.Value);
                        list.AddRange(Demeanor);
                        break;
                    case 5:
                        var Kidfriendly = db.Animals.Where(a => a.KidFriendly == Convert.ToBoolean( item.Value));
                        list.AddRange(Kidfriendly);
                        break;
                    case 6:
                        var PetFriendly = db.Animals.Where(a => a.PetFriendly == Convert.ToBoolean(item.Value));
                        list.AddRange(PetFriendly);
                        break;
                    case 7:
                        var Weight = db.Animals.Where(a => (int)a.Weight == Convert.ToInt32(item.Value));
                        list.AddRange(Weight);
                        break;
                    case 8:
                        var ID = db.Animals.Where(a => a.AnimalId == Convert.ToInt32(item.Value));
                        list.AddRange(ID);
                        break;
                    default:
                        break;
                }
                return list;
            }
          
            throw new NotImplementedException();
            // return listofAnimals;

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