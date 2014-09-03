Tips:

// How to use transactions with UnitOfWork
using(var unitOfWork = new UnitOfWork())
{
    var transaction = unitOfWork.Database.BeginTransaction();
    // Do database operations and return a result
    if (!result)
    {
        transaction.Rollback();
    }
    else
    {
        transaction.Commit();
    }
}