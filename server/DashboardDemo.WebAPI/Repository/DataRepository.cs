using System.Linq;

namespace DashboardDemo.WebAPI.Repository
{
    public class DataRepository {
        public static string[] DataSource () => new [] { "Fruit", "Animals" };
    
        public static IQueryable<Fruit> GetFruits() {
            var data = new Fruit[] { 
                new Fruit { Name = "Apple", Amount = 15 }, 
                new Fruit { Name = "Mango", Amount = 35 },
                new Fruit { Name = "Banana", Amount = 45 },};
            
            return data.AsQueryable();    
        }
    
        public static IQueryable<Animal> GetAnimals() {
            var data = new Animal[] { 
                new Animal { Name = "Elephant", InWild = 200 }, 
                new Animal { Name = "Tiger", InWild = 10 },
                new Animal { Name = "Some cool bird", InWild = 100 },};
            
            return data.AsQueryable();    
        }
    }
}