using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ThesisDemoWebApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DataContext context;  
    private DbSet<T> dbSet;  

    public Repository()
        {
            context = new DataContext();
            dbSet = context.Set<T>();
        }
        public IEnumerable<T> GetAll() {  
        return dbSet.ToList();  
    }
    public T GetById(object id) 
    {
        return dbSet.Find(id);
    }
    public T Insert(T obj) 
    {
        dbSet.Add(obj);
        //Save(); 
        context.SaveChanges(); 
        return obj;
    }
    public void Delete(object id) 
    {
        T entityToDelete = dbSet.Find(id);
        Delete(entityToDelete);
    }
    public void Delete(T entityToDelete)
    { 
    //while  {  
    //    if (context.Entry(entityToDelete).State == EntityState.Detached) {
    //    dbSet.Attach(entityToDelete);
    //    }
    dbSet.Remove(entityToDelete);
    }

//    public T Update(T obj)
//        {  
//        dbSet.Attach(obj);  
//        context.Entry(obj).State = EntityState.Modified;  
//        Save();  
//        return obj;  
//    }
//public void Save()
//{
//    try
//    {
//        context.SaveChanges();
//        }
//    catch (DbEntityValidationException dbEx)
//    {
//        foreach (var validationErrors in dbEx.EntityValidationErrors)
//        {
//            foreach (var validationError in validationErrors.ValidationErrors)
//            {
//                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
//                --- > you just put the log to know the errors
//                }
//        }
//    }
//}
//protected virtual void Dispose(bool disposing)
//        {  
//        if (disposing) {
//    if (context != null)
//    {
//        context.Dispose();
//        context = null;
//    }
}
}  
